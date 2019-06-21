﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PhotonController
{
    public partial class frmMain : Form
    {

        int receiverPort = 4024;

        IPEndPoint ipEndPoint;
        UdpClient receiver;
        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        string gcodeCmd = "";
        frmLog logForm = new frmLog();

        public frmMain()
        {
            InitializeComponent();
        }

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (logForm.txtResponse.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                logForm.txtResponse.Text = text;
                logForm.txtResponse.SelectionStart = logForm.txtResponse.Text.Length;
                logForm.txtResponse.ScrollToCaret();
                logForm.txtResponse.Refresh();
            }
        }

        delegate void SetProgressCallback(int value);

        private void SetProgress(int value)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.PrintProgress.InvokeRequired)
            {
                SetProgressCallback d = new SetProgressCallback(SetProgress);
                this.Invoke(d, new object[] { value });
            }
            else
            {
                this.PrintProgress.Value = value;
            }
        }
        delegate void SetLblCallback(string text);

        private void SetLbl(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.PrintProgress.InvokeRequired)
            {
                SetLblCallback d = new SetLblCallback(SetLbl);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblPercentDone.Text = text;
            }
        }
        private void DataReceived(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)ar.AsyncState;
            IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

            // Convert data to ASCII and print in console
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

            SetText(logForm.txtResponse.Text + receivedText);
            //logForm.txtResponse.SelectionStart = logForm.txtResponse.Text.Length;

            // Restart listening for udp data packages
            c.BeginReceive(DataReceived, ar.AsyncState);

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            receiver = new UdpClient(receiverPort);
            //receiver.BeginReceive(DataReceived, receiver);
            ipEndPoint = new IPEndPoint(IPAddress.Parse(txtIP.Text), int.Parse(txtPort.Text));
            receiver.Connect(ipEndPoint);

            gcodeCmd = "M114";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            if (receivedText.IndexOf("X:0.000000") != -1)
                lblStatus.Text = "Connected";
            else
                lblStatus.Text = "Not Connected";
            gcodeCmd = "M27";
            updatefileList();
            //receiver.BeginReceive(DataReceived, receiver);
        }

        public void SendGcode()
        {

            //using (UdpClient sender1 = new UdpClient(ipEndPoint))
            gcodeCmd = logForm.txtCmd.Text;
            receiver.Send(Encoding.ASCII.GetBytes(logForm.txtCmd.Text), logForm.txtCmd.Text.Length);
        }

        private void pollTimer_Tick(object sender, EventArgs e)
        {
            if (gcodeCmd == "M27" || gcodeCmd == "M114" || gcodeCmd == "M4000")
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
        }

        private void getPrintPercent()
        {
            gcodeCmd = "M27";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M25";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = false;
            //btnStart.Enabled = true;
            //btnStop.Enabled = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M24";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = true;
            //btnStart.Enabled = false;
            //btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            gcodeCmd = "M29";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = false;
            //btnStart.Enabled = true;
            //btnStop.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logForm.Show();
        }
        private void updatefileList()
        {
            gcodeCmd = "M20";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            while (receivedText.IndexOf("Begin file list") < 0)
            {
                receivedBytes = receiver.Receive(ref ipEndPoint);
                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            }
            if(receivedText.IndexOf("Begin file list")>=0)
            {
                int count = 0;
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Num",28, HorizontalAlignment.Left);
                listView1.Columns.Add("Finename",135, HorizontalAlignment.Left);
                listView1.Columns.Add("Size",55, HorizontalAlignment.Left);
                while (receivedText.IndexOf("End file list") <0)
                {
                    receivedBytes = receiver.Receive(ref ipEndPoint);
                    receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                    if (receivedText.IndexOf("End file list") < 0)
                    {
                        count++;
                        string[] temp = receivedText.Trim().Split(' ');
                        string temp1 = "";
                        for (int i = 0; i < temp.Length - 1; i++)
                        {
                            temp1 += temp[i] + " ";
                        }
                        string[] myItems = new string[] { count.ToString(), temp1, SizeSuffix(int.Parse(temp[temp.Length - 1])) };
                        listView1.Items.Add(new ListViewItem(myItems));
                    }
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Browse File to upload";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Byte[] receivedBytes;
                gcodeCmd = "M28 " + Path.GetFileName(openFileDialog1.FileName);
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                receivedBytes = receiver.Receive(ref ipEndPoint);
                string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                FileStream target_local_file_fi = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                int count = 0x500;
                byte[] buffer = new byte[count + 6];
                byte[] destinationArray;
                int index = 0;
                while (target_local_file_fi.Position < target_local_file_fi.Length)
                {
                    int position = (int)target_local_file_fi.Position;
                    index = target_local_file_fi.Read(buffer, 0, count);
                    if (index > 0)
                    {
                        byte num8 = 0;
                        byte[] bytes = BitConverter.GetBytes(position);
                        buffer[index] = bytes[0];
                        buffer[index + 1] = bytes[1];
                        buffer[index + 2] = bytes[2];
                        buffer[index + 3] = bytes[3];
                        for (int i = 0; i < (index + 4); i++)
                        {
                            num8 = (byte)(num8 ^ buffer[i]);
                        }
                        buffer[index + 4] = num8;
                        buffer[index + 5] = 0x83;
                        destinationArray = new byte[index + 6];
                        Array.Copy(buffer, 0, destinationArray, 0, index + 6);
                        receiver.Send(destinationArray, destinationArray.Length);
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                    }
                }
                try
                {
                    target_local_file_fi.Close();
                    target_local_file_fi = null;
                }
                catch (Exception) { }
                gcodeCmd = "M29";
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                receivedBytes = receiver.Receive(ref ipEndPoint);
                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                updatefileList();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            updatefileList();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                gcodeCmd = "M30 " + item.SubItems[1].Text;
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
                string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                updatefileList();
            }
        }
    }
}