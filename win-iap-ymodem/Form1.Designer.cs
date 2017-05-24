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
            this.btn_Uart = new System.Windows.Forms.Button();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbl_Bps = new System.Windows.Forms.Label();
            this.lbl_Pass = new System.Windows.Forms.Label();
            this.cbx_Port = new System.Windows.Forms.ComboBox();
            this.cbx_Bps = new System.Windows.Forms.ComboBox();
            this.tbx_Pass = new System.Windows.Forms.TextBox();
            this.btn_Pass = new System.Windows.Forms.Button();
            this.btn_download = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_Erase = new System.Windows.Forms.Button();
            this.btn_Upload = new System.Windows.Forms.Button();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Uart
            // 
            this.btn_Uart.Location = new System.Drawing.Point(22, 120);
            this.btn_Uart.Name = "btn_Uart";
            this.btn_Uart.Size = new System.Drawing.Size(155, 32);
            this.btn_Uart.TabIndex = 0;
            this.btn_Uart.Text = "打开";
            this.btn_Uart.UseVisualStyleBackColor = true;
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
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
            // 
            // cbx_Bps
            // 
            this.cbx_Bps.FormattingEnabled = true;
            this.cbx_Bps.Location = new System.Drawing.Point(79, 78);
            this.cbx_Bps.Name = "cbx_Bps";
            this.cbx_Bps.Size = new System.Drawing.Size(98, 20);
            this.cbx_Bps.TabIndex = 6;
            // 
            // tbx_Pass
            // 
            this.tbx_Pass.Location = new System.Drawing.Point(87, 36);
            this.tbx_Pass.Name = "tbx_Pass";
            this.tbx_Pass.Size = new System.Drawing.Size(206, 21);
            this.tbx_Pass.TabIndex = 7;
            // 
            // btn_Pass
            // 
            this.btn_Pass.Location = new System.Drawing.Point(310, 35);
            this.btn_Pass.Name = "btn_Pass";
            this.btn_Pass.Size = new System.Drawing.Size(64, 23);
            this.btn_Pass.TabIndex = 8;
            this.btn_Pass.Text = "选择文件";
            this.btn_Pass.UseVisualStyleBackColor = true;
            // 
            // btn_download
            // 
            this.btn_download.Location = new System.Drawing.Point(27, 244);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(116, 28);
            this.btn_download.TabIndex = 9;
            this.btn_download.Text = "更新固件";
            this.btn_download.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Uart);
            this.groupBox1.Controls.Add(this.lbl_Bps);
            this.groupBox1.Controls.Add(this.lbl_Port);
            this.groupBox1.Controls.Add(this.cbx_Port);
            this.groupBox1.Controls.Add(this.cbx_Bps);
            this.groupBox1.Location = new System.Drawing.Point(27, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 188);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口操作";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbx_Pass);
            this.groupBox2.Controls.Add(this.lbl_Pass);
            this.groupBox2.Controls.Add(this.btn_Pass);
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
            // 
            // btn_Upload
            // 
            this.btn_Upload.Location = new System.Drawing.Point(356, 244);
            this.btn_Upload.Name = "btn_Upload";
            this.btn_Upload.Size = new System.Drawing.Size(116, 28);
            this.btn_Upload.TabIndex = 13;
            this.btn_Upload.Text = "读取固件";
            this.btn_Upload.UseVisualStyleBackColor = true;
            // 
            // btn_Reset
            // 
            this.btn_Reset.Location = new System.Drawing.Point(512, 244);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(116, 28);
            this.btn_Reset.TabIndex = 14;
            this.btn_Reset.Text = "复位MCU";
            this.btn_Reset.UseVisualStyleBackColor = true;
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
            this.Controls.Add(this.btn_download);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Uart;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lbl_Bps;
        private System.Windows.Forms.Label lbl_Pass;
        private System.Windows.Forms.ComboBox cbx_Port;
        private System.Windows.Forms.ComboBox cbx_Bps;
        private System.Windows.Forms.TextBox tbx_Pass;
        private System.Windows.Forms.Button btn_Pass;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_Erase;
        private System.Windows.Forms.Button btn_Upload;
        private System.Windows.Forms.Button btn_Reset;
    }
}

