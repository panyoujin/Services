using System;
using System.IO;
using System.Net;
using System.Text;

namespace Services
{
    public class RequestHelper
    {

        public static string Request(string _address, string method = "GET", string jsonData = null, int timeOut = 5)
        {
            try
            {
                string resultJson = string.Empty;
                if (string.IsNullOrEmpty(_address))
                    return null;
                Uri address = new Uri(_address);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

                // 构建Head
                request.Method = method;
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                    Stream reqstream = request.GetRequestStream();
                    reqstream.Write(bytes, 0, bytes.Length);
                }
                request.Timeout = timeOut * 1000;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();
                    if (responseStr != null && responseStr.Length > 0)
                    {
                        resultJson = responseStr;
                    }
                }
                return resultJson;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async static void Request(string _address, Action<WebResponse> func, string method = "GET", string jsonData = null, int timeOut = 5)
        {
            string resultJson = string.Empty;
            if (string.IsNullOrEmpty(_address))
                return;
            try
            {
                Uri address = new Uri(_address);

                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                //System.Net.ServicePointManager.DefaultConnectionLimit = 50; 
                // 构建Head
                request.Method = method;
                request.KeepAlive = false;
                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                if (!string.IsNullOrWhiteSpace(jsonData))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bytes, 0, bytes.Length);
                        reqStream.Close();
                    }
                }
                request.Timeout = timeOut * 1000;
                request.ContentType = "application/x-www-form-urlencoded";
                var ww = await request.GetResponseAsync();
                func(ww);
                //func(w as HttpWebResponse);
                //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                //{
                //    func(response);
                //    StreamReader reader = new StreamReader(response.GetResponseStream());
                //    string responseStr = reader.ReadToEnd();
                //    if (responseStr != null && responseStr.Length > 0)
                //    {
                //        resultJson = responseStr;
                //    }
                //}
            }
            catch (Exception ex)
            {
                resultJson = ex.Message;
            }
            return;
        }

    }
}
