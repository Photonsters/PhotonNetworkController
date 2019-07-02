namespace PhotonController
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblPercentDone = new System.Windows.Forms.Label();
            this.PrintProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressFile = new System.Windows.Forms.ProgressBar();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pollTimer
            // 
            this.pollTimer.Interval = 1000;
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.lblPercentDone);
            this.groupBox1.Controls.Add(this.PrintProgress);
            this.groupBox1.Location = new System.Drawing.Point(17, 158);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(349, 138);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Status";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(233, 27);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 28);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(125, 27);
            this.btnPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(100, 28);
            this.btnPause.TabIndex = 15;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(17, 27);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 28);
            this.btnStart.TabIndex = 14;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // lblPercentDone
            // 
            this.lblPercentDone.AutoSize = true;
            this.lblPercentDone.Location = new System.Drawing.Point(13, 68);
            this.lblPercentDone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPercentDone.Name = "lblPercentDone";
            this.lblPercentDone.Size = new System.Drawing.Size(95, 17);
            this.lblPercentDone.TabIndex = 13;
            this.lblPercentDone.Text = "Precent Done";
            // 
            // PrintProgress
            // 
            this.PrintProgress.Location = new System.Drawing.Point(13, 87);
            this.PrintProgress.Margin = new System.Windows.Forms.Padding(4);
            this.PrintProgress.Name = "PrintProgress";
            this.PrintProgress.Size = new System.Drawing.Size(320, 28);
            this.PrintProgress.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Controls.Add(this.btnConnect);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPort);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Location = new System.Drawing.Point(17, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(349, 135);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Connection Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 103);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 17);
            this.lblStatus.TabIndex = 19;
            this.lblStatus.Text = "Not Connected";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(233, 91);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 28);
            this.btnConnect.TabIndex = 18;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 59);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 17);
            this.label3.TabIndex = 17;
            this.label3.Text = "Default Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 16;
            this.label2.Text = "Photon IP Address";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(183, 55);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(149, 22);
            this.txtPort.TabIndex = 15;
            this.txtPort.Text = "3000";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(183, 23);
            this.txtIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(149, 22);
            this.txtIP.TabIndex = 14;
            this.txtIP.Text = "192.168.1.222";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(397, 30);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 15;
            this.button1.Text = "Log";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(233, 196);
            this.btnUpload.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(100, 28);
            this.btnUpload.TabIndex = 20;
            this.btnUpload.Text = "Upload File";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressFile);
            this.groupBox3.Controls.Add(this.btnDownload);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnRefresh);
            this.groupBox3.Controls.Add(this.btnUpload);
            this.groupBox3.Location = new System.Drawing.Point(17, 303);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(349, 267);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Files";
            // 
            // progressFile
            // 
            this.progressFile.Location = new System.Drawing.Point(11, 231);
            this.progressFile.Margin = new System.Windows.Forms.Padding(4);
            this.progressFile.Name = "progressFile";
            this.progressFile.Size = new System.Drawing.Size(215, 28);
            this.progressFile.TabIndex = 23;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(233, 231);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 28);
            this.btnDownload.TabIndex = 23;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(11, 196);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 28);
            this.btnDelete.TabIndex = 23;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(125, 196);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(31, 326);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(319, 164);
            this.listView1.TabIndex = 22;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 581);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "Photonsters Print Manager";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer pollTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblPercentDone;
        private System.Windows.Forms.ProgressBar PrintProgress;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ProgressBar progressFile;
        private System.Windows.Forms.Button btnDownload;
    }
}

