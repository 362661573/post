using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public delegate void ChangeFormColor1(int topmost ,int a);
    class Class1
    {
        //基本设置
        static private string path = @"ftp://" + "188a004f22.51mypc.cn" + "/GRMCULXFRER";    //目标路径
        static private string ftpip = "188a004f22.51mypc.cn";    //ftp IP地址
        static private string username = "admin";   //ftp用户名
        static private string password = "admin";   //ftp密码

        public event ChangeFormColor1 ChangeColor1;
        //获取ftp上面的文件和文件夹
        public static string[] GetFileList(string dir)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(username, password);//设置用户名和密码
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    Console.WriteLine(line);
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取ftp上面的文件和文件夹：" + ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }

        }
        //从ftp服务器上下载文件的功能  
        public void Download(string fileName)
        {

            FtpWebRequest reqFTP;
            try
            {

                string filePath = Application.StartupPath;
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    ChangeColor1(readCount,(int)cl);
                    // float b =2048/(float)cl*100;   
                    System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
                    System.Threading.Thread.Sleep(10);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}