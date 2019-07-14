using System;
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
        bool ispaused = false;
        bool isPrinting = false;
        double progressBefore = -9999;
        int framenumber = 0;
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
        //private void DataReceived(IAsyncResult ar)
        //{
        //    UdpClient c = null;
        //    c = (UdpClient)ar.AsyncState;
        //    IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    Byte[] receivedBytes = c.EndReceive(ar, ref receivedIpEndPoint);

        //    // Convert data to ASCII and print in console
        //    string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

        //    //SetText(logForm.txtResponse.Text + receivedText);
        //    //logForm.txtResponse.SelectionStart = logForm.txtResponse.Text.Length;
        //    if (gcodeCmd == "M27")     //progress report
        //    {
        //        string[] lines = receivedText.Split('\n');
        //        lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //        double[] progress = new double[2];
        //        if ((lines[0].Substring(0, 5).ToUpper() != "ERROR") && (lines[0].Substring(0, 2).ToUpper() != "OK"))
        //        {
        //            isPrinting = true;
        //            lines = lines[0].Split(new[] { "byte" }, StringSplitOptions.None);
        //            lines = lines[1].Split('/');
        //            progress[0] = double.Parse(lines[0].Trim());
        //            progress[1] = double.Parse(lines[1].Trim());
        //            //SetText(logForm.txtResponse.Text + progressCount.ToString()+"\r\n");
        //            if (progress[0] > progressBefore)
        //            {
        //                int PrintProgress = (int)Math.Round((progress[0] * 100) / progress[1]);
        //                SetProgress(PrintProgress);
        //                SetLbl("Percent done : " + PrintProgress.ToString());
        //                progressBefore = progress[0];
        //                //SetText(logForm.txtResponse.Text + "Frame = " + framenumber.ToString() + ", ");
        //                //framenumber++;
        //            }

        //            //txtResponse.SelectionStart = txtResponse.Text.Length;
        //            //txtResponse.ScrollToCaret();
        //            //txtResponse.Refresh();
        //        }
        //        else if (lines[0].Substring(0, 5).ToUpper() == "ERROR")
        //        {
        //            if (isPrinting)      //means it was printing and now it finsihed
        //            {
        //                SetProgress(100);
        //                SetLbl("Percent done : 100");
        //            }
        //            isPrinting = false;
        //        }

        //    }
        //    // Restart listening for udp data packages
        //    c.BeginReceive(DataReceived, ar.AsyncState);

        //}

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
            {
                
                lblStatus.Text = getVersion().Trim() +" connected"; 
            }
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
            ////if (gcodeCmd == "M27")
            ////{
            //gcodeCmd = "M27";
            //receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            //string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            ////receiver.BeginReceive(DataReceived, receiver);
            ////if(c!=null)
            ////    c.BeginReceive(DataReceived, receiver);
            //string[] lines = receivedText.Split('\n');
            //lines = lines.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            //double[] progress = new double[2];
            //if ((lines[0].Substring(0, 5).ToUpper() != "ERROR") && (lines[0].Substring(0, 2).ToUpper() != "OK"))
            //{
            //    isPrinting = true;
            //    lines = lines[0].Split(new[] { "byte" }, StringSplitOptions.None);
            //    lines = lines[1].Split('/');
            //    progress[0] = double.Parse(lines[0].Trim());
            //    progress[1] = double.Parse(lines[1].Trim());
            //    //SetText(logForm.txtResponse.Text + progressCount.ToString()+"\r\n");
            //    if (progress[0] > progressBefore)
            //    {
            //        int PrintProgress = (int)Math.Round((progress[0] * 100) / progress[1]);
            //        SetProgress(PrintProgress);
            //        SetLbl("Percent done : " + PrintProgress.ToString());
            //        progressBefore = progress[0];
            //        //SetText(logForm.txtResponse.Text + "Frame = " + framenumber.ToString() + ", ");
            //        //framenumber++;
            //    }

            //    //txtResponse.SelectionStart = txtResponse.Text.Length;
            //    //txtResponse.ScrollToCaret();
            //    //txtResponse.Refresh();
            //}
            //else if (lines[0].Substring(0, 5).ToUpper() == "ERROR")
            //{
            //    if (isPrinting)      //means it was printing and now it finsihed
            //    {
            //        SetProgress(100);
            //        SetLbl("Percent done : 100");
            //    }
            //    isPrinting = false;
            //}
            //}
        }

        private void getPrintPercent()
        {
            gcodeCmd = "M27";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (ispaused)
            {
                gcodeCmd = "M24";
                ispaused = false;
                isPrinting = true;
                btnPause.Text = "Pause";
            }
            else
            {
                gcodeCmd = "M25";
                ispaused = true;
                isPrinting = false;
                btnPause.Text = "Resume";
                pollTimer.Enabled = false;
            }
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            //btnPause.Enabled = false;
            //btnStart.Enabled = true;
            //btnStop.Enabled = true;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //gcodeCmd = "M24";
            //receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            //btnPause.Enabled = true;
            //btnStart.Enabled = false;
            //btnStop.Enabled = true;
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                //gcodeCmd = "M6030 ':" + item.SubItems[1].Text + "'";
                gcodeCmd = "M6030 ':" + item.SubItems[1].Text + "'";
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
                string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                ispaused = false;
                isPrinting = true;
                //btnPause.Enabled = true;
                btnStart.Enabled = false;
                //btnStop.Enabled = true;
                gcodeCmd = "M27";
                pollTimer.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please select a file before starting print");
            }
            
        }
        private string getVersion()
        {
            gcodeCmd = "M4002";
            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
            Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
            string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
            return receivedText.Substring(3);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Want to stop the current print？", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
            if (dialogResult == DialogResult.Yes)
            {
                gcodeCmd = "M33 I5";
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                Byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
                string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                gcodeCmd = "M29";
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                receivedBytes = receiver.Receive(ref ipEndPoint);
                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                gcodeCmd = "G90, G0 Z 150 F300";
                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                receivedBytes = receiver.Receive(ref ipEndPoint);
                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                //btnPause.Enabled = false;
                btnStart.Enabled = true;
                //btnStop.Enabled = false;
                ispaused = false;
                isPrinting = false;
                pollTimer.Enabled = false;
            }
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
            if (receivedText.IndexOf("Begin file list") >= 0)
            {
                int count = 0;
                listView1.Items.Clear();
                listView1.Columns.Clear();
                listView1.Columns.Add("Num", 28, HorizontalAlignment.Left);
                listView1.Columns.Add("Finename", 135, HorizontalAlignment.Left);
                listView1.Columns.Add("Size", 55, HorizontalAlignment.Left);
                while (receivedText.IndexOf("End file list") < 0)
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
        private float getValue(string str, string prefix, float default_val) =>
            this.getValue(str, prefix, default_val, null);

        private float getValue(string str, string prefix, float default_val, string ahead)
        {
            try
            {
                int startIndex = 0;
                if (ahead != null)
                {
                    startIndex = str.IndexOf(ahead);
                    if (startIndex != -1)
                    {
                        startIndex += ahead.Length;
                    }
                }
                int index = str.IndexOf(prefix, startIndex);
                if (index != -1)
                {
                    index += prefix.Length;
                    int length = 0;
                    char[] chArray = str.ToCharArray();
                    for (int i = index; i < str.Length; i++)
                    {
                        char ch = chArray[i];
                        if (((ch < '0') || (ch > '9')) && ((ch != '.') && (ch != '-')))
                        {
                            break;
                        }
                        length++;
                    }
                    if (length > 0)
                    {
                        return (float)Convert.ToDouble(str.Substring(index, length));
                    }
                    return default_val;
                }
                return default_val;
            }
            catch (Exception)
            {
                return default_val;
            }
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Browse File to upload";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                progressFile.Value = 0;
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
                        if (receivedText.Contains("resend"))
                        {
                            long resend_index = (long)getValue(receivedText, "resend ", -1f);
                            target_local_file_fi.Seek((long)resend_index, SeekOrigin.Begin);
                        }
                        progressFile.Value = (int)((100 * target_local_file_fi.Position) / target_local_file_fi.Length);
                        Application.DoEvents();
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

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                bool isError = false;
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Save downloaded file as . . .";
                ListViewItem item = listView1.SelectedItems[0];
                saveFileDialog1.FileName = item.SubItems[1].Text;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    gcodeCmd = "M6032 '" + item.SubItems[1].Text + "'";
                    receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                    byte[] receivedBytes = receiver.Receive(ref ipEndPoint);
                    string receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                    if (receivedText.Contains("ok N"))
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                    receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                    long fileLength = 0;
                    if (receivedText.Contains("Error"))
                    {
                        isError = true;
                        gcodeCmd = "M22";
                        receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);

                        gcodeCmd = "M6032 '" + item.SubItems[1].Text + "'";
                        receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                        //receivedBytes = receiver.Receive(ref ipEndPoint);
                        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                        if (receivedText.Contains("Error"))
                            MessageBox.Show("Error occured while downloading this file");
                        else
                        {
                            fileLength = (long)getValue(receivedText, "L:", -1f);
                            if (fileLength != -1)
                                isError = false;
                        }
                    }
                    if (!isError)
                    {
                        fileLength = (long)getValue(receivedText, "L:", -1f);
                        long CurrentFileLength = 0;
                        FileStream saved_local_file_fi = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                        gcodeCmd = "M3000";
                        progressFile.Value = 0;
                        while (CurrentFileLength < fileLength)
                        {
                            receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                            receivedBytes = receiver.Receive(ref ipEndPoint);
                            uint maxValue = uint.MaxValue;
                            if ((receivedBytes.Length >= 6) && (receivedBytes[receivedBytes.Length - 1] == 0x83))
                            {
                                maxValue = BitConverter.ToUInt32(receivedBytes, receivedBytes.Length - 6);
                                if (maxValue == CurrentFileLength)
                                {
                                    byte num3 = 0;
                                    for (int i = 0; i < (receivedBytes.Length - 2); i++)
                                    {
                                        num3 = (byte)(num3 ^ receivedBytes[i]);
                                    }
                                    if (num3 == receivedBytes[receivedBytes.Length - 2])
                                    {

                                        //Write this data to file
                                        saved_local_file_fi.Write(receivedBytes, 0, receivedBytes.Length - 6);
                                        CurrentFileLength += receivedBytes.Length - 6;
                                        progressFile.Value = (int)((100 * CurrentFileLength) / fileLength);
                                        Application.DoEvents();
                                    }
                                    else
                                    {
                                        gcodeCmd = "M3001 I" + CurrentFileLength;
                                    }
                                }
                                else
                                    gcodeCmd = "M3001 I" + CurrentFileLength;
                            }
                            else
                            {
                                //saved_local_file_fi.Close();
                                gcodeCmd = "M22";
                                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                receivedBytes = receiver.Receive(ref ipEndPoint);
                                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                                //MessageBox.Show("Unknown error occured while downloading. Please try again");
                                //break;
                                gcodeCmd = "M6032 '" + item.SubItems[1].Text + "'";
                                receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                                receivedBytes = receiver.Receive(ref ipEndPoint);
                                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                                if (receivedText.Contains("ok N"))
                                    receivedBytes = receiver.Receive(ref ipEndPoint);
                                receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                                fileLength = (long)getValue(receivedText, "L:", -1f);
                                gcodeCmd = "M3000";
                            }
                        }

                        saved_local_file_fi.Close();

                        gcodeCmd = "M22";
                        receiver.Send(Encoding.ASCII.GetBytes(gcodeCmd), gcodeCmd.Length);
                        receivedBytes = receiver.Receive(ref ipEndPoint);
                        receivedText = ASCIIEncoding.ASCII.GetString(receivedBytes);
                    }
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }
    }
}
