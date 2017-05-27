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

        private string filePath = "";
        private int fsLen;

        /* packet define */
        const byte C = 67;   // capital letter C
        const byte PACKET_SEQNO_INDEX = 1;
        const byte PACKET_SEQNO_COMP_INDEX = 2;
        const byte PACKET_HEADER = 3;
        const byte PACKET_TRAILER = 2;
        const byte PACKET_OVERHEAD = 2 + 3;
        const byte PACKET_SIZE = 128;
        const int PACKET_1K_SIZE = 1024;

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

        }

        private void openControlBtn()
        {
            btn_Update.Enabled = true;
            btn_Upload.Enabled = true;
            btn_Reset.Enabled = true;
            btn_Erase.Enabled = true;
        }

        private void closeControlBtn()
        {
            btn_Update.Enabled = false;
            btn_Upload.Enabled = false;
            btn_Reset.Enabled = false;
            btn_Erase.Enabled = false;
        }

        private void btn_SelectHex_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            filePath = openFileDialog1.FileName;
            //Get the extension of the file
            string extName = System.IO.Path.GetExtension(filePath);
            if (extName == ".hex")
            {
                convertHexToBin(filePath);
            }
            else
            {
                FileStream fileStream = new FileStream(@filePath, FileMode.Open, FileAccess.Read);
                fsLen = (int)fileStream.Length / 1000;
            }
            txb_FilePath.Text = filePath;
            HasSelectBin = true;
        }

        private void cbx_Port_DropDown(object sender, EventArgs e)
        {
            if (!HasOpenPort)
            {
                EnumComportfromReg(cbx_Port);
            }
        }

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
            if (regSubKey.GetValueNames() == null) return;
            string[] strCommList = regSubKey.GetValueNames();
            foreach (string VName in strCommList)
            {
                //向listbox1中添加字符串的名称和数据，数据是从rk对象中的GetValue(it)方法中得来的
                Combobox.Items.Add(regSubKey.GetValue(VName));
            }
            if (Combobox.Items.Count > 0)
            {
                Combobox.SelectedIndex = 0;
                //btn_Uart.Enabled = true;
            }
            else
            {
                Combobox.Text = "";
                //MessageBox.Show("无法获取有效端口号!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            regSubKey.Close();
            regRootKey.Close();
        }

        public void convertHexToBin(string szHexPath)
        {
            Int32 i = 0;
            Int32 j = 0;
            Int32 maxAddr = 0;        //HEX文件的最大地址
            Int32 segAddr = 0;        //段地址
            Int32 first = 0;
            Int32 row = 0;
            Int32 sum = 0;
            try
            {
                String szLine = "";
                String szHex = "";
                if (szHexPath == "")
                {
                    MessageBox.Show("请选择需要转换的目标文件!         ", "错误");
                    return;
                }

                StreamReader HexReader = new StreamReader(szHexPath);

                //先找出HEX文件的最大地址
                while (true)
                {
                    szLine = HexReader.ReadLine(); //读取一行数据
                    i++;
                    if (szLine == null) //读完所有行
                    {
                        break;
                    }
                    if (szLine.Substring(0, 1) == ":") //判断第1字符是否是:
                    {

                        if (szLine.Substring(1, 8) == "00000001")//数据结束
                        {
                            break;
                        }
                        if (szLine.Substring(7, 2) == "04")
                        {
                            segAddr = Int32.Parse(szLine.Substring(9, 4), NumberStyles.HexNumber);      //800
                            segAddr *= 16;
                        }
                        else if (szLine.Substring(7, 2) == "00")
                        {
                            int len_row = szLine.Length;
                            Int32 tmpAddr = Int32.Parse(szLine.Substring(3, 4), NumberStyles.HexNumber);
                            tmpAddr += UInt16.Parse(szLine.Substring(1, 2), NumberStyles.HexNumber);
                            tmpAddr += segAddr;
                            row++;
                            sum = row * 16;
                            if (len_row < 42)
                                sum -= Int32.Parse(szLine.Substring(1, 2), NumberStyles.HexNumber);

                            if (tmpAddr > maxAddr)
                                maxAddr = tmpAddr;
                        }

                    }
                    else
                    {
                        MessageBox.Show("错误:不是标准的hex文件!");
                        return;
                    }

                }

                //新建一个二进制文件,填充为0XFF
                byte[] szBin = new byte[sum];


                ////for (i = 0; i < maxAddr; i++)
                ////    szBin[i] = 0XFF;
                ////返回文件开头
                HexReader.BaseStream.Seek(0, SeekOrigin.Begin);
                HexReader.DiscardBufferedData();//不加这句不能正确返回开头 
                segAddr = 0;

                //根据hex文件地址,填充bin文件
                while (true)
                {

                    szLine = HexReader.ReadLine(); //读取一行数据

                    if (szLine == null) //读完所有行
                    {
                        break;
                    }
                    if (szLine.Substring(0, 1) == ":") //判断第1字符是否是:
                    {

                        if (szLine.Substring(1, 8) == "00000001")//数据结束
                        {
                            break;
                        }
                        if (szLine.Substring(7, 2) == "04")
                        {
                            segAddr = Int32.Parse(szLine.Substring(9, 4), NumberStyles.HexNumber);
                            segAddr *= 16;
                        }
                        if (szLine.Substring(7, 2) == "00")
                        {
                            int tmpAddr = Int32.Parse(szLine.Substring(3, 4), NumberStyles.HexNumber);
                            int num = Int16.Parse(szLine.Substring(1, 2), NumberStyles.HexNumber);
                            tmpAddr += segAddr;
                            j = 0;
                            for (i = 0; i < num; i++)
                            {
                                szBin[first++] = (byte)Int16.Parse(szLine.Substring(j + 9, 2), NumberStyles.HexNumber);
                                j += 2;
                            }

                        }
                    }

                }

                HexReader.Close(); //关闭目标文件


                //  if (path == "")
                {
                    filePath = Path.ChangeExtension(szHexPath, "bin");
                    //tbBinPath.Text = szBinPath;
                }
                FileStream fs = new FileStream(filePath, FileMode.Create);

                //将byte数组写入文件中
                fs.Write(szBin, 0, szBin.Length);
                //所有流类型都要关闭流，否则会出现内存泄露问题                
                fsLen = (int)fs.Length / 1000;                    //计算总段长
                //txb_totalsection.Text = fsLen.ToString();
                fs.Close();

                string tmp = "文件转换完成! 文件大小: ";
                tmp += sum.ToString();
                tmp += "字节";


                //FileStream fBin = new FileStream(szBinPath, FileMode.Create); //创建文件BIN文件
                //BinaryWriter BinWrite = new BinaryWriter(fBin); //二进制方式打开文件
                //BinWrite.Write(szBin, 0,maxAddr); //写入数据
                //BinWrite.Flush();//释放缓存
                //BinWrite.Close();//关闭文件
                //string tmp = "文件转换完成! 文件大小: ";
                //tmp += maxAddr.ToString();
                //tmp += "字节";
                MessageBox.Show(tmp, "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// 串口接收数据显示（用于调试）
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

        private bool YmodemUpLoalFile(string filePath)
        {
            /* control signals */
            const byte STX = 2;  // Start of TeXt 
            const byte EOT = 4;  // End Of Transmission
            const byte ACK = 6;  // Positive ACknowledgement


            /* sizes */
            const int dataSize = 1024;
            const int crcSize = 2;

            /* the packet size: 1029 bytes */
            /* header: 3 bytes */
            // STX
            int packetNumber = 0;
            int invertedPacketNumber = 255;
            /* data: 1024 bytes */
            byte[] data = new byte[dataSize];
            /* footer: 2 bytes */
            byte[] CRC = new byte[crcSize];

            /* get the file */
            FileStream fileStream = new FileStream(@filePath, FileMode.Open, FileAccess.Read);
            try
            {
                /* send the initial packet with filename and filesize */
                if (serialPort1.ReadByte() != C)
                {
                    Console.WriteLine("Can't begin the transfer.");
                    return false;
                }

                sendYmodemInitialPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, filePath, fileStream, CRC, crcSize);

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
                    sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize);

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
                sendYmodemClosingPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize);
                /* get ACK (downloader acknowledge the EOT) */
                if (serialPort1.ReadByte() != ACK)
                {
                    Console.WriteLine("Can't complete the transfer.");
                    return false;
                }
            }
            catch (TimeoutException)
            {
                throw new Exception("Eductor does not answering");
            }
            finally
            {
                fileStream.Close();
            }

            Console.WriteLine("File transfer is succesful");
            return true;
        }

        private void sendYmodemPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, byte[] CRC, int crcSize)
        {
            serialPort1.Write(new byte[] { STX }, 0, 1);
            serialPort1.Write(new byte[] { (byte)packetNumber }, 0, 1);
            serialPort1.Write(new byte[] { (byte)invertedPacketNumber }, 0, 1);
            serialPort1.Write(data, 0, dataSize);
            serialPort1.Write(CRC, 0, crcSize);
        }

        private void sendYmodemInitialPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, string path, FileStream fileStream, byte[] CRC, int crcSize)
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
            sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize);
        }

        private void sendYmodemClosingPacket(byte STX, int packetNumber, int invertedPacketNumber, byte[] data, int dataSize, byte[] CRC, int crcSize)
        {
            /* calculate CRC */
            Crc16Ccitt crc16Ccitt = new Crc16Ccitt(InitialCrcValue.Zeros);
            CRC = crc16Ccitt.ComputeChecksumBytes(data);

            /* send the packet */
            sendYmodemPacket(STX, packetNumber, invertedPacketNumber, data, dataSize, CRC, crcSize);
        }

        private void uploadFileThread()
        {
            YmodemUpLoalFile(txb_FilePath.Text);
        }
        /// <summary>
        /// bootloader运行时更新固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            Thread UploadThread = new Thread(uploadFileThread);
            UploadThread.Start();
        }

        /// <summary>
        /// app运行时更新固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update1_Click(object sender, EventArgs e)
        {
            serialPort1.Write("download\r\n");
            Thread.Sleep(1000);
            if (serialPort1.ReadByte() != C)
            {
                Console.WriteLine("download cmd error!");
                MessageBox.Show("启动下载命令失败！");
            }
            Thread UploadThread = new Thread(uploadFileThread);
            UploadThread.Start();
        }

        /// <summary>
        /// 擦除固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Erase_Click(object sender, EventArgs e)
        {
            serialPort1.Write("erase\r\n");
        }

        /// <summary>
        /// 上传固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Upload_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 复位固件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            serialPort1.Write("reset\r\n");
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            tbx_show.Text = "";
        }

        private void btn_SendCMD_Click(object sender, EventArgs e)
        {

        }

        string cmd_str = "";
        private void tbx_show_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                serialPort1.Write("\r");
                //MessageBox.Show(cmd_str);
            }
            else
            {
                serialPort1.Write(e.KeyChar.ToString());
            }
        }
    }
}
