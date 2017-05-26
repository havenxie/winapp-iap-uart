namespace win_iap_ymodem
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_Port = new System.Windows.Forms.Button();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbl_Bps = new System.Windows.Forms.Label();
            this.lbl_Pass = new System.Windows.Forms.Label();
            this.cbx_Port = new System.Windows.Forms.ComboBox();
            this.cbx_Baud = new System.Windows.Forms.ComboBox();
            this.txb_FilePath = new System.Windows.Forms.TextBox();
            this.btn_SelectHEX = new System.Windows.Forms.Button();
            this.btn_Update = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Erase = new System.Windows.Forms.Button();
            this.btn_Upload = new System.Windows.Forms.Button();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.tbx_rev = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Port
            // 
            this.btn_Port.Location = new System.Drawing.Point(22, 120);
            this.btn_Port.Name = "btn_Port";
            this.btn_Port.Size = new System.Drawing.Size(155, 32);
            this.btn_Port.TabIndex = 0;
            this.btn_Port.Text = "打开";
            this.btn_Port.UseVisualStyleBackColor = true;
            this.btn_Port.Click += new System.EventHandler(this.btn_Port_Click);
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(20, 41);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(53, 12);
            this.lbl_Port.TabIndex = 1;
            this.lbl_Port.Text = "串口号：";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(27, 293);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(601, 28);
            this.progressBar1.TabIndex = 2;
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "bin文件(*.bin)|*.bin|hex文件(*.hex)|*.hex";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // lbl_Bps
            // 
            this.lbl_Bps.AutoSize = true;
            this.lbl_Bps.Location = new System.Drawing.Point(20, 81);
            this.lbl_Bps.Name = "lbl_Bps";
            this.lbl_Bps.Size = new System.Drawing.Size(53, 12);
            this.lbl_Bps.TabIndex = 3;
            this.lbl_Bps.Text = "波特率：";
            // 
            // lbl_Pass
            // 
            this.lbl_Pass.AutoSize = true;
            this.lbl_Pass.Location = new System.Drawing.Point(16, 40);
            this.lbl_Pass.Name = "lbl_Pass";
            this.lbl_Pass.Size = new System.Drawing.Size(65, 12);
            this.lbl_Pass.TabIndex = 4;
            this.lbl_Pass.Text = "文件路径：";
            // 
            // cbx_Port
            // 
            this.cbx_Port.FormattingEnabled = true;
            this.cbx_Port.Location = new System.Drawing.Point(79, 38);
            this.cbx_Port.Name = "cbx_Port";
            this.cbx_Port.Size = new System.Drawing.Size(98, 20);
            this.cbx_Port.TabIndex = 5;
            this.cbx_Port.DropDown += new System.EventHandler(this.cbx_Port_DropDown);
            // 
            // cbx_Baud
            // 
            this.cbx_Baud.FormattingEnabled = true;
            this.cbx_Baud.Items.AddRange(new object[] {
            "Custom",
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "256000"});
            this.cbx_Baud.Location = new System.Drawing.Point(79, 78);
            this.cbx_Baud.Name = "cbx_Baud";
            this.cbx_Baud.Size = new System.Drawing.Size(98, 20);
            this.cbx_Baud.TabIndex = 6;
            // 
            // txb_FilePath
            // 
            this.txb_FilePath.Location = new System.Drawing.Point(87, 36);
            this.txb_FilePath.Name = "txb_FilePath";
            this.txb_FilePath.Size = new System.Drawing.Size(206, 21);
            this.txb_FilePath.TabIndex = 7;
            // 
            // btn_SelectHEX
            // 
            this.btn_SelectHEX.Location = new System.Drawing.Point(310, 35);
            this.btn_SelectHEX.Name = "btn_SelectHEX";
            this.btn_SelectHEX.Size = new System.Drawing.Size(64, 23);
            this.btn_SelectHEX.TabIndex = 8;
            this.btn_SelectHEX.Text = "选择文件";
            this.btn_SelectHEX.UseVisualStyleBackColor = true;
            this.btn_SelectHEX.Click += new System.EventHandler(this.btn_SelectHex_Click);
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(27, 244);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(116, 28);
            this.btn_Update.TabIndex = 9;
            this.btn_Update.Text = "更新固件";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Port);
            this.groupBox1.Controls.Add(this.lbl_Bps);
            this.groupBox1.Controls.Add(this.lbl_Port);
            this.groupBox1.Controls.Add(this.cbx_Port);
            this.groupBox1.Controls.Add(this.cbx_Baud);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 188);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口操作";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbx_rev);
            this.groupBox2.Controls.Add(this.txb_FilePath);
            this.groupBox2.Controls.Add(this.lbl_Pass);
            this.groupBox2.Controls.Add(this.btn_SelectHEX);
            this.groupBox2.Location = new System.Drawing.Point(248, 18);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 182);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件操作";
            // 
            // btn_Erase
            // 
            this.btn_Erase.Location = new System.Drawing.Point(192, 244);
            this.btn_Erase.Name = "btn_Erase";
            this.btn_Erase.Size = new System.Drawing.Size(116, 28);
            this.btn_Erase.TabIndex = 12;
            this.btn_Erase.Text = "擦除固件";
            this.btn_Erase.UseVisualStyleBackColor = true;
            this.btn_Erase.Click += new System.EventHandler(this.btn_Erase_Click);
            // 
            // btn_Upload
            // 
            this.btn_Upload.Location = new System.Drawing.Point(356, 244);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(116, 28);
            this.btn_Upload.TabIndex = 13;
            this.btn_Upload.Text = "读取固件";
            this.btn_Upload.UseVisualStyleBackColor = true;
            this.btn_Upload.Click += new System.EventHandler(this.btn_Upload_Click);
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(512, 244);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(116, 28);
            this.btn_Reset.TabIndex = 14;
            this.btn_Reset.Text = "复位MCU";
            this.btn_Reset.UseVisualStyleBackColor = true;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // tbx_rev
            // 
            this.tbx_rev.Location = new System.Drawing.Point(18, 86);
            this.tbx_rev.Multiline = true;
            this.tbx_rev.Name = "tbx_rev";
            this.tbx_rev.Size = new System.Drawing.Size(362, 76);
            this.tbx_rev.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 343);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.btn_Upload);
            this.Controls.Add(this.btn_Erase);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Port;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lbl_Bps;
        private System.Windows.Forms.Label lbl_Pass;
        private System.Windows.Forms.ComboBox cbx_Port;
        private System.Windows.Forms.ComboBox cbx_Baud;
        private System.Windows.Forms.TextBox txb_FilePath;
        private System.Windows.Forms.Button btn_SelectHEX;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Erase;
        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.TextBox tbx_rev;
    }
}

