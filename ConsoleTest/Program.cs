using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using HuaweiAPIDLL;
using SkoLifeSMSWinSrv.BO;
using Task = SkoLifeSMSWinSrv.BO.Task;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {


            //SkoLifeSMSWinSrv.BO.HuaweiAPI.UserLogin("192.168.8.1", "admin", "admin", out string log);
            //SkoLifeSMSWinSrv.BO.HuaweiAPI.SendSMS("192.168.8.1", "01004558638", "VERY GOOD 2");
            
            
            //using (MD5 md5Hash = MD5.Create())

            //{
            //    string hash = GetMd5Hash(md5Hash, source);

            //    Console.WriteLine("The MD5 hash of " + source + " is: " + hash + ".");

            //    Console.WriteLine("Verifying the hash...");

            //    if (VerifyMd5Hash(md5Hash, source, hash))
            //    {
            //        Console.WriteLine("The hashes are the same.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("The hashes are not same.");
            //    }
            //}


            TaskManager.DefaultInstance = new TaskManager();
            TaskManager.DefaultInstance.GetTasks();
            TaskManager.DefaultInstance.Start();
            Console.ReadLine();
            return;

            //var x = FindPorts();

            //string result = SendMessage("192.168.8.1", 80, "admin", "admin", "+201004558638", "My SMS from C#");

            //List<string> str = new List<string>();
            //string[] ports = SerialPort.GetPortNames();
            //foreach (string port in ports)
            //{
            //    str.Add(port);
            //}

            //test();
            //return;

            //SMS sms = new SMS();
            //sms.Direction = SMSDirection.Submited;
            //sms.PhoneNumber = "+201004558638";
            //sms.ValidityPeriod = new TimeSpan(4, 0, 0, 0);
            //sms.Message = "Hello World";
            //string pduSource = sms.Compose(SMS.SMSEncoding.UCS2);
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void test()
        {
            //TaskManager.DefaultInstance = new TaskManager();
            //Task tsk = new  Task(1, "hello world", "01004558638", "+20105996500", "COM1");
            //TaskManager.DefaultInstance.Tasks.Enqueue(tsk);
            //tsk.Execute();
        }

        static ManagementObject[] FindPorts()
        {
            
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");
                List<ManagementObject> objects = new List<ManagementObject>();
                foreach (ManagementObject obj in searcher.Get())
                {
                    if (obj["Caption"] != null && obj["Caption"].ToString().Contains("COM"))
                    {
                        objects.Add(obj);
                    }
                }

                return objects.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        static void getPorts()
        {
            List<string> str = new List<string>();
            var searcher = new ManagementObjectSearcher(@"Select * From Win32_USBHub");
            ManagementObjectCollection collection = searcher.Get();
            foreach (var device in collection)
            {
                string deviceId = device["DeviceID"].ToString();
                string port = device["Caption"].ToString();
                str.Add(port);
                //if (deviceId == usbDeviceName)
                //    str.Add("Port for " + usbDeviceName + " is " + port);


                //MessageBox.Show(deviceId + "\n" + port + "\n" );
            }
        }


        private static string SendMessage(string host, int port, string userName, string password, string number, string message)
        {
            string result = "ERROR";

            // Create the base URI to send a message with the Diafaan SMS Server HTTP API
            string uri = "http://" + host + ":" + port.ToString() + "/html/smsinbox.html";
            // Add the HTTP query to the base URI
            uri += "?username=" + HttpUtility.UrlEncode(userName);
            uri += "&password=" + HttpUtility.UrlEncode(password);
            uri += "&to=" + HttpUtility.UrlEncode(number);
            uri += "&message=" + HttpUtility.UrlEncode(message);

            SendSMS(uri);
            return "";
            // Send the HTTP request to Diafaan SMS Server
            WebRequest request = WebRequest.Create(uri);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Get the HTTP response from Diafaan SMS Server
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        private static string SendSMS(string url)
        {
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(url);
            while (wb.ReadyState != WebBrowserReadyState.Complete) { /*MediaTypeNames.Application.DoEvents();*/ }

            return wb.Document.DomDocument.ToString();

        }

        private static void send3()
        {
            string result = string.Empty;
            WebRequest request = WebRequest.Create("http://192.168.8.1/api/webserver/SesTokInfo/");
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Get the HTTP response from Diafaan SMS Server
                Stream dataStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = reader.ReadToEnd();
                }
            }

            string SesInfo = string.Empty;
            string TokInfo = string.Empty;

            if (result != string.Empty)
            {
                int SesInfoStart = result.IndexOf("SessionID=");// + 10;
                int SesInfoEnd = result.IndexOf("</SesInfo>") - 1;

                int TokInfoStart = result.IndexOf("<TokInfo>") + 9;
                int TokInfoEnd = result.IndexOf("</TokInfo>") - 1;
                SesInfo = result.Substring(SesInfoStart, SesInfoEnd + 1 - SesInfoStart);
                TokInfo = result.Substring(TokInfoStart, TokInfoEnd + 1 - TokInfoStart);
            }

            request = WebRequest.Create("http://192.168.8.1/api/user/login/");
            request.Method = "POST";
            request.Headers.Add("__RequestVerificationToken: " + TokInfo);
            //request.Headers.Add("__RequestVerificationToken: S6VUw0nfJGoDXs80vF3MgASOuumW81hz");
            request.Headers.Add("Cookie: " + SesInfo);

            //var passwordHash = Sha256("admin");
            var password = "";//Sha256("admin" + passwordHash + TokInfo);
            

            string postString = $@"<?xml version:'1.0' encoding='UTF-8'?>
                 <request>
                <Username>admin</Username>
                <Password>{password}</Password>
                <password_type>4</password_type>
                </request>";

            byte[] bytes = Encoding.UTF8.GetBytes(postString);

            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            
            Encoding encode;
            StreamReader sr;
            HttpWebResponse myresponse = (HttpWebResponse)request.GetResponse();
            if (myresponse.StatusCode != HttpStatusCode.OK)
            {
                string message = string.Format("POST failed. Received HTTP {0}", myresponse.StatusCode);
                Console.WriteLine(message);
                Console.ReadLine();
            }
            else
            {
                Stream ReceiveStream = myresponse.GetResponseStream();
                encode = Encoding.GetEncoding("utf-8");
                sr = new StreamReader(ReceiveStream);
                result = sr.ReadToEnd();
                Console.WriteLine(result);
                Console.ReadLine();
            }


            //responseReader.Close();
            request.GetResponse().Close();

        }

        static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            string returnStr = "";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                returnStr = System.Convert.ToBase64String(bytes);

            }
            return returnStr;
        }

        static string ComputeSha256Hash2(string Phrase)
        {
            SHA256 HashTool = SHA256.Create();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Phrase));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        static string Hex(byte[] bb)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bb.Length; i++)
            {
                sb.Append(bb[i].ToString("X2"));
            }
            return sb.ToString();
        }

      



    }
}
