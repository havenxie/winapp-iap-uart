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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            closeControlBtn();
            EnumComportfromReg(cbx_Port);
            cbx_Baud.SelectedIndex = 13;
        }

        private void openControlBtn()
        {
            btn_download.Enabled = true;
            btn_Upload.Enabled = true;
            btn_Reset.Enabled = true;
            btn_Erase.Enabled = true;
        }

        private void closeControlBtn()
        {
            btn_download.Enabled = false;
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


    }
}
