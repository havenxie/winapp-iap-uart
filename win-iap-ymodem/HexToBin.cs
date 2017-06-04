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
    public class HexToBin
    {
        public static bool convertHexToBin(string szHexPath)
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
                    MessageBox.Show("请选择需要转换的目标文件! ", "提示");
                    return false;
                }
                StreamReader HexReader = new StreamReader(szHexPath);
                //先找出HEX文件的最大地址
                while (true)
                {
                    szLine = HexReader.ReadLine(); //读取一行数据
                    i++;
                    if (szLine == null) break;
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
                        return false;
                    }
                }
                //新建一个二进制文件
                byte[] szBin = new byte[sum];

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
                Form1.filePath = Path.ChangeExtension(szHexPath, "bin");
                //tbBinPath.Text = szBinPath;
                FileStream fs = new FileStream(Form1.filePath, FileMode.Create);

                //将byte数组写入文件中
                fs.Write(szBin, 0, szBin.Length);
                fs.Close();

                return true;
                //Form1.tbx_show.AppendText("> 文件转换完成!\r\n");
                //tbx_show.AppendText("> 文件大小: " + sum.ToString() + " 字节\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

    }
}
