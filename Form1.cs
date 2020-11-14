using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //data
                string cookieStr = "lastUser=in89;lastAutoLoginCheck=true;JSESSIONID=D502FA3C02AD8A9708EB004FEAF37BE1;_ati=2091820415051;Hm_lvt_bf3296661a119dc2e4c3427b339b6d9e=1587388838;qs_sys=is;qs_cid=753983;qs_uid=1166219;MMSSessionSID=17B9E50C5CDBCBB93A47B07A50BF2909;JXSessionSID=<SNAID>644748F04EED8ED51C57E962A31256D3</SNAID>;aLogin=65bad84d-9a87-4703-999d-5b7f61ac3ebd;isLoginId=cab4eacd-3847-4a7b-bbbb-e86e25a2afb3";
                string postData = "page=1&beginTime=2020-09-16+00%3A00%3A00&endTime=2020-09-17+00%3A00%3A00&orderStatus=2&businessType=-1&_search=false&nd=1600242405575&rows=18&sidx=&sord=asc";
                byte[] data = Encoding.UTF8.GetBytes(postData);
                    // X - Requested - With: XMLHttpRequest
                    //X - HttpWatch - RID: 5066 - 10821
                    //Content - Type: application / x - www - form - urlencoded; charset = UTF - 8
                    //Accept: application / json, text / javascript, */*; q=0.01
                    //Referer: https://web.qinsilk.com/is/admin/inner/report/sale/saleReport.ac?mid=83&
                    //Accept-Language: zh-CN
                    //Accept-Encoding: gzip, deflate
                    //User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko
                    //Host: web.qinsilk.com
                    //Content-Length: 156
                    //DNT: 1
                    //Connection: Keep-Alive
                    //Cache-Control: no-cache
                // Prepare web request...
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://web.qinsilk.com/is/admin/inner/report/sale/saleGroupByClientListJSON.ac");
                request.Method = "POST";
               // request.Referer = "http://web.qinsilk.com/is/admin/inner/report/sale/saleReport.ac?mid=83&";
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                request.UserAgent = "Mozilla/5.0(Windows NT 6.1;WOW64)AppleWebKit/537.36(KHTML,like Gecko)Chrome/78.0.3904.108 Safari/537.36";
                request.Accept = "application/json, text/plain, */*";
                request.Headers.Add("Accept-Language", "zh-CN");
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.Headers.Add("qs-pcversion", "3.4.12");
                request.Headers.Add("Cookie", cookieStr);
                request.ContentLength = data.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // Get response
                HttpWebResponse myResponse = (HttpWebResponse)request.GetResponse();
                var cookiestr_1 = myResponse.Headers.Get("Set-Cookie");
                var wer = UpdateCookie(cookieStr, cookiestr_1);
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                Console.WriteLine(cookiestr_1);

                Console.WriteLine("合并的cookie:", wer);

                Console.WriteLine(content);
                
                Console.ReadLine();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
          MySqlConnection m_conn = new MySqlConnection();
            m_conn.ConnectionString =
            string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", "120.76.129.195",
            "zhanghao", "root", "c274184309"
            );
            m_conn.Open();

            string a = @"SELECT
                        caigou.sj,
                        caigou.`款号`,
                        caigou.`类型`,
                        caigou.je,
                        caigou.id
                        FROM
                        caigou
                        WHERE
                        caigou.shijiancuo > (SELECT UNIX_TIMESTAMP('2020-08-01 00:00:49')*1000)
                        ";

            MySqlCommand mscommand = new MySqlCommand(a, m_conn);
            using (MySqlDataReader reader = mscommand.ExecuteReader(CommandBehavior.CloseConnection))//ExecuteReader
            {
                while (reader.Read())
                {
                   // Console.WriteLine(reader.GetValue(4));
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader.GetName(i) + reader.GetValue(i));
                    }
                    Console.WriteLine();
                }                
            }
            Console.WriteLine("连接成功");
            m_conn.Close();
        }
        /// <summary>
        /// 合并Cookie，将cookie2与cookie1合并更新 返回字符串类型Cookie
        /// </summary>
        /// <param name="cookie1">旧cookie</param>
        /// <param name="cookie2">新cookie</param>
        /// <returns></returns>
        public string UpdateCookie(string cookie1, string cookie2)
        {
            StringBuilder sb = new StringBuilder();

            Dictionary<string, string> dicCookie = new Dictionary<string, string>();
            //遍历cookie1
            if (!string.IsNullOrEmpty(cookie1))
            {
                foreach(string cookie in cookie1.Replace(",", ";").Split(';'))
                {
                    if (!string.IsNullOrEmpty(cookie) && cookie.IndexOf("=") > 0)
                    {
                        string key = cookie.Split('=')[0].Trim();
                        string value = cookie.Substring(key.Length + 1).Trim();
                        if (dicCookie.ContainsKey(key))
                        {
                            dicCookie[key] = cookie;
                        }
                        else
                        {
                            dicCookie.Add(key, cookie);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(cookie2))
            {
                //遍历cookie2
                foreach (string cookie in cookie2.Replace(',', ';').Split(';'))
                {
                    if (!string.IsNullOrEmpty(cookie) && cookie.IndexOf('=') > 0)
                    {
                        string key = cookie.Split('=')[0].Trim();
                        string value = cookie.Substring(key.Length + 1).Trim();
                        if (dicCookie.ContainsKey(key))
                        {
                            dicCookie[key] = cookie;
                        }
                        else
                        {
                            dicCookie.Add(key, cookie);
                        }
                    }
                }
            }
            int i = 0;
            foreach (var item in dicCookie)
            {
                i++;
                if (i < dicCookie.Count)
                {
                    sb.Append(item.Value + ";");
                }
                else
                {
                    sb.Append(item.Value);
                }
            }
            return sb.ToString();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ChangeColor += new ChangeFormColor(f_ChangeColor);
            f.Show();
        }
        void f_ChangeColor(bool topmost)
        {
            this.BackColor = Color.LightBlue;
            this.Text = "改变成功";
            string s = (string)Form2.table["xiaomao"];
            this.Text = s;
        }
        void f_ChangeColor1(int topmost,int a)
        {
            this.progressBar1.Maximum = a;
            this.progressBar1.Value +=topmost;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Class1.GetFileList(null);
            Class1 wer = new Class1();
            wer.ChangeColor1 += new ChangeFormColor1(f_ChangeColor1);
            wer.Download("/8uftp.exe");

            System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label1将因为循环执行太快而来不及显示信息
            System.Threading.Thread.Sleep(1000);

            string filePath = Application.StartupPath;
            if (File.Exists(filePath + "\\8uftp.exe"))
            {
                string a = filePath + "\\8uftp.exe";
                //    Process pr = new Process();//声明一个进程类对象
                //    pr.StartInfo.FileName =a;
                //    pr.Start();

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = a;
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Minimized;
                Process pro = Process.Start(info);
                pro.WaitForExit();
            }

        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            string filePath = Application.StartupPath;
            if (File.Exists("D:\\FTP\\8uftp.exe"))
            {
               string a = "D:\\FTP\\8uftp.exe";
            //    Process pr = new Process();//声明一个进程类对象
            //    pr.StartInfo.FileName =a;
            //    pr.Start();

                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = a;
                info.Arguments = "";
                info.WindowStyle = ProcessWindowStyle.Minimized;
                Process pro = Process.Start(info);
                pro.WaitForExit();
            }
        }
        //基本设置
        static private string path = @"ftp://" + "188a004f22.51mypc.cn" + "/GRMCULXFRER";    //目标路径
        static private string ftpip = "188a004f22.51mypc.cn";    //ftp IP地址
        static private string username = "admin";   //ftp用户名
        static private string password = "admin";   //ftp密码
        //从ftp服务器上下载文件的功能  
        public  void Download(string fileName)
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
                progressBar1.Maximum = (int)cl;
                while (readCount > 0)
                {
                    if ((progressBar1.Value + 2048) < (int)cl)
                    {
                        
                    }                    
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    progressBar1.Value += readCount;
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
