using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
 using System.Drawing.Imaging;
 using System.IO;

namespace QrCodeTest
{
    class Program
    {
        //将文本文件每行的字符串信息生成QrCode
        static void fileToQrCode(string file)
        {
            string[] text = file.Split('\n');
            for (int i = 0; i < text.Length; i++)
            {
                QrEncoder qrEncoder = new QrEncoder();
                QrCode qrCode = qrEncoder.Encode(text[i]);
                string filename;
                if(text[i].Length >= 4)
                {   //生成的QrCode以信息所在行号三位数+信息的前四个字符构成
                    filename = (i + 1).ToString("X3") + text[i].Substring(0, 4) + ".png";
                }
                else
                {
                    filename = (i + 1).ToString("X3") + text[i] + ".png";
                }
                GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                using (FileStream stream = new FileStream(filename, FileMode.Create))
                {
                    render.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);
                }
            }
        }
        //直接在控制台上用黑白方块打印出QrCode码
        static void printQrCode(string str)
        {
            QrEncoder qrEncoder = new QrEncoder();
            QrCode qrCode = qrEncoder.Encode(str);
            for (int i = 0; i < qrCode.Matrix.Width + 2; i++)
            {
                Console.Write("█");
            }
            Console.WriteLine();
            for (int j = 0; j < qrCode.Matrix.Width; j++)
            {
                Console.Write("█");
                for (int i = 0; i < qrCode.Matrix.Width; i++)
                {
                    char charToPrint = qrCode.Matrix[j, i] ? '　' : '█';
                    Console.Write(charToPrint);
                }
                Console.Write("█");
                Console.WriteLine();
            }
            for (int i = 0; i < qrCode.Matrix.Width + 2; i++)
            {
                Console.Write("█");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                if(args[0].Length > 100)
                {   //如果命令参数长度超过100，输出提示
                    Console.WriteLine("{0} You can't input more than 100 characters!", args[0].Length);
                }
                else
                {
                    if (args[0].Substring(0, 2) == "-f" && args[0].Length > 2)
                    {
                        if (File.Exists(args[0].Substring(2)))
                        {
                            string file = File.ReadAllText(args[0].Substring(2));
                            fileToQrCode(file);
                        }
                        else
                        {//如果参数文件名错误，输出提示
                            Console.WriteLine("The file does not exist,please write a correct file name!");
                        }
                    }
                    else
                    {
                        printQrCode(args[0]);
                    }
                }
                
            }
            else
            {   //如果没有参数，输出提示
                Console.WriteLine("You need to input something.");
            }
        }
    }
}
