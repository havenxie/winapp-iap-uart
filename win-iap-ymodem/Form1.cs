using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace win_iap_ymodem
{
    public partial class Form1 : Form
    {
        private bool hasOpenPort = false;
        public bool HasOpenPort
        {
            get { return hasOpenPort; }
            set
            {
                hasOpenPort = value;
                if (hasOpenPort && hasSelectBin)
                    openControlBtn();
                else
                    closeControlBtn();
            }
        }
        private bool hasSelectBin = false;
        public bool HasSelectBin
        {
            get { return hasSelectBin; }
            set
            {
                hasSelectBin = value;
                if (hasOpenPort && hasSelectBin)
                    openControlBtn();
                else
                    closeControlBtn();
            }
        }

        public static string filePath = "";
        string sendCmd = "";
        int fsLen = 0;

        /* packet define */
        const byte C = 67;   // capital letter C
        byte STX = 2;  // Start Of Text
        //const byte PACKET_SEQNO_INDEX = 1;
        //const byte PACKET_SEQNO_COMP_INDEX = 2;
        //const byte PACKET_HEADER = 3;
        //const byte PACKET_TRAILER = 2;
        //const byte PACKET_OVERHEAD = 2 + 3;

        const int FILE_NAME_LENGTH = 256;
        const byte FILE_SIZE_LENGTH = 16;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            closeControlBtn();
            EnumComportfromReg(cbx_Port);
            serialPort1.Encoding = Encoding.GetEncoding("gb2312");//串口接收编码GB2312码
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//忽略程序跨越线程运行导致的错误.没有此句将会产生错误
            cbx_Baud.SelectedIndex = 13;
            cbx_PageSize.SelectedIndex = 4;
        }


        /// <summary>
        /// enabled all button
        /// </summary>
        private void openControlBtn()
        {
            btn_Update.Enabled = true;
            btn_Upload.Enabled = true;
            //btn_Erase.Enabled = true;
            btn_IAPMenu.Enabled = true;
            btn_RunApp.Enabled = true;
        }


        /// <summary>
        /// disabled all button
        /// </summary>
        private void closeControlBtn()
        {
            btn_Update.Enabled = false;
            btn_Upload.Enabled = false;
            //btn_Erase.Enabled = false;
            btn_IAPMenu.Enabled = false;
            btn_RunApp.Enabled = false;

        }


        /// <summary>
        /// the button for select bin file or hex file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        

        /// <summary>
        /// has been selected the right file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            filePath = openFileDialog1.FileName;
            //Get the extension of the file
            string extName = Path.GetExtension(filePath);
            if (extName == ".hex")
            {
                //we shoule convert the hex file to bin file.
                if (HexToBin.convertHexToBin(filePath))
                {
                    tbx_show.AppendText("> 文件转换完成!\r\n");
                }
                else
                {
                    tbx_show.AppendText("> 文件转换失败!\r\n");
                }
            }

            txb_FilePath.Text = filePath;

            HasSelectBin = true;//flag has been select file.

            FileStream fileStream = new FileStream(@filePath, FileMode.Open, FileAccess.Read);
            fsLen = (int)fileStream.Length;
            tbx_show.AppendText("文件大小: " + fsLen.ToString() + "\r\n");
        }


        /// <summary>
        /// get all port and add to cbx_Port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_Port_DropDown(object sender, EventArgs e)
        {
            if (!HasOpenPort)
            {
                EnumComportfromReg(cbx_Port);
            }
        }


        /// <summary>
        /// open or close port.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Port_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)//try to close
            {
                try
                {
                    serialPort1.Close();
                    btn_Port.Text = "打开";
                    HasOpenPort = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("关闭失败");
                    return;
                }
            }
            else //try to open
            {
                if (cbx_Port.Items.Count == 0)
                    return;
                int baud = int.Parse(cbx_Baud.Text);
                serialPort1.PortName = cbx_Port.Text;
                serialPort1.BaudRate = baud;
                try
                {
                    serialPort1.Open();
                    btn_Port.Text = "关闭";
                    HasOpenPort = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("打开失败");
                    return;
                }
            }
        }


        /// <summary>
        /// Get the port list from the registry
        /// </summary>
        /// <param name="Combobox"></param>
        private void EnumComportfromReg(ComboBox Combobox)
        {
            Combobox.Items.Clear();
            ///定义注册表子Path
            string strRegPath = @"Hardware\\DeviceMap\\SerialComm";
            ///创建两个RegistryKey类，一个将指向Root Path，另一个将指向子Path
            RegistryKey regRootKey;
            RegistryKey regSubKey;
            ///定义Root指向注册表HKEY_LOCAL_MACHINE节点
            regRootKey = Registry.LocalMachine;
            regSubKey = regRootKey.OpenSubKey(strRegPath);
            if (regSubKey.GetValueNames() == null)
            {
                MessageBox.Show("获取串口设备失败");
                return;
            } 
            string[] strCommList = regSubKey.GetValueNames();
            foreach (string VName in strCommList)
            {
                //向listbox1中添加字符串的名称和数据，数据是从rk对象中的GetValue(it)方法中得来的
                Combobox.Items.Add(regSubKey.GetValue(VName));
            }
            if (Combobox.Items.Count > 0)
            {
                Combobox.SelectedIndex = 0;
            }
            else
            {
                Combobox.Text = "";
            }
            regSubKey.Close();
            regRootKey.Close();
        }


        /// <summary>
        /// once has date in. we should show it on the txb.(找到了bug的原因，就是有数据来了之后首先主动去读取了一次数据，然后又通过这个服务去被动读取了一次，所以会出现问题。)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                string revData = serialPort1.ReadExisting();
                tbx_show.AppendText(revData);
            }
            catch
            {
                 MessageBox.Show("串口出现故障");
            }
        }


        /// <summary>
        /// A thread to update firmware.
        /// </summary>
        private void updateFileThread()
        {
            YmodemUpdateFile(txb_FilePath.Text);
        }


        /// <summary>
        /// upload the firmware when the app is runing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            if ( !File.Exists(@txb_FilePath.Text) || (Path.GetExtension(@txb_FilePath.Text) != ".bin"))
            {
                MessageBox.Show("请选择有效的bin文件", "提示");
                return;
            }

            progressBar1.Value = 0;
            this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            serialPort1.Write("update\r\n");//发送更新命令
            serialPort1.ReadTimeout = 1000;
            try
            {
                string rec = serialPort1.ReadTo("C");
                Thread UploadThread = new Thread(updateFileThread);
                UploadThread.Start();
            }
            catch
            {
                MessageBox.Show("后台下载失败");
            }
            finally
            {
                this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            }
        }


        /// <summary>
        /// erase all flash when the button has checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Erase_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            this.serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);

            serialPort1.Write("erase\r\n");
            serialPort1.ReadTimeout = 1000;
            try
            {
                string rec = serialPort1.ReadTo("@");
                rec = serialPort1.ReadTo("@");
                progressBar1.Maximum = Convert.ToInt32(rec);
                tbx_show.AppendText("\r\n\n> 需要擦除 " + rec + " 页:\r\n");
                while (true)
                {
                    rec = serialPort1.ReadTo("@");
                    if (rec != "")
                    {
                        int val = Convert.ToInt32(rec);
                        if (val < progressBar1.Maximum)
                        {
                            progressBar1.Value = val;
                            tbx_show.AppendText("> 正在擦除 " + val + " 页.\r\n");
                        }
                        else
                        {
                            progressBar1.Value = progressBar1.Maximum;
                            tbx_show.AppendText("> 正在擦除 " + val + " 页.\r\n");
                            tbx_show.AppendText("> 擦除完成.\r\n");
                            break;
                        }
                    }
                }
            }
            catch (TimeoutException)
            {
                tbx_show.AppendText("> 响应超时.\r\n");
            }
            finally {
                this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            }
        }


        /// <summary>
        /// upload the app file form stm32 to PC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Upload_Click(object sender, EventArgs e)
        {
            //to do.
            MessageBox.Show("不好意思，这个功能还没有做。");
        }


        /// <summary>
        /// into iap menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_IAPMenu_Click(object sender, EventArgs e)
        {
            serialPort1.Write("menu\r\n");
        }


        private void btn_RunApp_Click(object sender, EventArgs e)
        {
            serialPort1.Write("runapp\r\n");
        }

        /// <summary>
        /// reset the stm32 when the button has been checkde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            serialPort1.Write("reset\r\n");
        }


        /// <summary>
        /// clear tbx_show window when the button has been checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            tbx_show.Text = "";
        }



        /// <summary>
        /// when user is input the key on the tbx_show, it should be send to stm32.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbx_show_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //tbx_show.AppendText("\r\n" + sendCmd + "\r\n");
                switch (sendCmd)
                {
                    case "update":
                        btn_Update_Click(null, null);
                        break;
                    case "upload":
                        btn_Upload_Click(null, null);
                        break;
                    case "erase":
                        btn_Erase_Click(null, null);
                        break;
                    case "menu":
                        btn_IAPMenu_Click(null, null);
                        break;
                    case "runapp":
                        btn_RunApp_Click(null, null);
                        break;
                    default:
                        serialPort1.Write(sendCmd + "\r\n");
                        break;
                }
                sendCmd = "";
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                if(sendCmd.Length > 0)
                    sendCmd = sendCmd.Substring(0, sendCmd.Length - 1);
            }
            else
            {
                sendCmd += e.KeyChar.ToString();
            }
        }


        private byte getPageHead(string text)
        {
            switch (Convert.ToInt32(text))
            {
                //case 8:
                //    return 0xA1;
                //case 16:
                //    return 0xA2;
                case 32:
                    return 0xA3;
                case 64:
                    return 0xA4;
                case 128:
                    return 0xA5;
                case 256:
                    return 0xA6;
                case 512:
                    return 0xA7;
                case 1024:
                    return 0xA8;
                //case 2048:
                //    return 0xA9;
                default:
                    return 2;
            }
        }
        private bool YmodemUpdateFile(string filePath)
        {
            const byte SOH = 1;  // Start Of Head

            const byte EOT = 4;  // End Of Transmission
            const byte ACK = 6;  // Positive ACknowledgement
            const int crcSize = 2;

            //byte HEAD = 0;
            // int dataSize = 1024;
            int dataSize = Convert.ToInt32(cbx_PageSize.Text);
            STX = getPageHead(cbx_PageSize.Text);

            /* header: 3 bytes */
            int proprassVal = 0;
            int packetNumber = 0;
            int invertedPacketNumber = 255;
            /* data: 1024 bytes */
            byte[] data = new byte[dataSize];
            /* footer: 2 bytes */
            byte[] CRC = new byte[crcSize];

            /* get the file */
            FileStream fileStream = new FileStream(@filePath, FileMode.Open, FileAccess.Read);
            int packetCnt = (fileStream.Length % dataSize) == 0 ? (int)(fileStream.Length / dataSize) : (int)(fileStream.Length / dataSize) + 1;
            progressBar1.Maximum = packetCnt;

            try
            {
                if (!sendYmodemInitialPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, filePath, fileStream, CRC, crcSize)) return false;

                if (serialPort1.ReadByte() != ACK)
                {
                    Console.WriteLine("Can't send the initial packet.");
                    return false;
                }

                if (serialPort1.ReadByte() != C)
                    return false;

                /* send packets with a cycle until we send the last byte */
                int fileReadCount;
                do
                {
                    /* if this is the last packet fill the remaining bytes with 0 */
                    fileReadCount = fileStream.Read(data, 0, dataSize);
                    if (fileReadCount == 0) break;
                    if (fileReadCount != dataSize)
                        for (int i = fileReadCount; i < dataSize; i++)
                            data[i] = 0;

                    /* calculate packetNumber */
                    packetNumber++;
                    if (packetNumber > 255)
                        packetNumber -= 256;
                    Console.WriteLine(packetNumber);

                    /* calculate invertedPacketNumber */
                    invertedPacketNumber = 255 - packetNumber;

                    /* calculate CRC */
                    Crc16Ccitt crc16Ccitt = new Crc16Ccitt(InitialCrcValue.Zeros);
                    CRC = crc16Ccitt.ComputeChecksumBytes(data);

                    /* send the packet */
                    if (!sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize)) return false;
                    progressBar1.Value = ++proprassVal;
                    /* wait for ACK */
                    if (serialPort1.ReadByte() != ACK)
                    {
                        Console.WriteLine("Couldn't send a packet.");
                        return false;
                    }
                } while (dataSize == fileReadCount);

                /* send EOT (tell the downloader we are finished) */
                serialPort1.Write(new byte[] { EOT }, 0, 1);
                /* send closing packet */
                packetNumber = 0;
                invertedPacketNumber = 255;
                data = new byte[dataSize];
                CRC = new byte[crcSize];
                if (!sendYmodemClosingPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize)) return false;
                /* get ACK (downloader acknowledge the EOT) */
                if (serialPort1.ReadByte() != ACK)
                {
                    Console.WriteLine("Can't complete the transfer.");
                    return false;
                }
            }
            catch (TimeoutException)
            {
                progressBar1.Value = 0;
                MessageBox.Show("数据传输超时");
            }
            catch (InvalidOperationException)
            {
                progressBar1.Value = 0;
                MessageBox.Show("端口打开失败");
            }
            catch (IOException)
            {
                progressBar1.Value = 0;
                MessageBox.Show("端口被中断，请检查连接");
            }
            finally
            {
                fileStream.Close();
            }

            Console.WriteLine("File transfer is succesful");
            return true;
        }


        private bool sendYmodemPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, byte[] CRC, int crcSize)
        {
            try
            {
                serialPort1.Write(new byte[] { STX }, 0, 1);
                serialPort1.Write(new byte[] { (byte)packetNumber }, 0, 1);
                serialPort1.Write(new byte[] { (byte)invertedPacketNumber }, 0, 1);
                serialPort1.Write(data, 0, dataSize);
                serialPort1.Write(CRC, 0, crcSize);
                return true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("端口打开失败");
                return false;
            }
            catch (TimeoutException)
            {
                MessageBox.Show("数据传输超时");
                return false;
            }
            catch (IOException)
            {
                MessageBox.Show("端口被中断，请检查连接");
                return false;
            }

        }


        private bool sendYmodemInitialPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, string path, FileStream fileStream, byte[] CRC, int crcSize)
        {
            string fileName = System.IO.Path.GetFileName(path);
            string fileSize = fileStream.Length.ToString();


            /* add filename to data */
            int i;
            for (i = 0; i < fileName.Length && (fileName.ToCharArray()[i] != 0); i++)
            {
                data[i] = (byte)fileName.ToCharArray()[i];
            }
            data[i] = 0;

            /* add filesize to data */
            int j;
            for (j = 0; j < fileSize.Length && (fileSize.ToCharArray()[j] != 0); j++)
            {
                data[(i + 1) + j] = (byte)fileSize.ToCharArray()[j];
            }
            data[(i + 1) + j] = 0;

            /* fill the remaining data bytes with 0 */
            for (int k = ((i + 1) + j) + 1; k < dataSize; k++)
            {
                data[k] = 0;
            }

            /* calculate CRC */
            Crc16Ccitt crc16Ccitt = new Crc16Ccitt(InitialCrcValue.Zeros);
            CRC = crc16Ccitt.ComputeChecksumBytes(data);

            /* send the packet */
            if (!sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize)) return false;
            return true;
        }


        private bool sendYmodemClosingPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, byte[] CRC, int crcSize)
        {
            /* calculate CRC */
            Crc16Ccitt crc16Ccitt = new Crc16Ccitt(InitialCrcValue.Zeros);
            CRC = crc16Ccitt.ComputeChecksumBytes(data);

            /* send the packet */
            if (!sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize)) return false;

            return true;
        }


    }
}
