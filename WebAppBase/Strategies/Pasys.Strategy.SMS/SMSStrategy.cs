using Pasys.Web.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Pasys.Strategy.SMS
{
    /// <summary>
    /// 简单短信策略
    /// </summary>
    public partial class SMSStrategy : ISMSStrategy
    {
        private string _url;
        private string _username;
        private string _password;

        /// <summary>
        /// 短信服务器地址
        /// </summary>
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// 短信账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">接收人号码</param>
        /// <param name="body">短信内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string body)
        {
            if (string.IsNullOrWhiteSpace(_url))
            {
                return false;
            }
            //var url = _url + "?method=Submit";
            byte[] result = Encoding.UTF8.GetBytes(_password.Trim());    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            var md5Pwd = Encoding.UTF8.GetString(output);  //tbMd5pass为输出加密文本的文本框

            string postStrTpl = "account={0}&password={1}&mobile={2}&content={3}";
            var postData = Encoding.UTF8.GetBytes(string.Format(postStrTpl, _username, _password, to, body));

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(_url);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = postData.Length;

            var newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(postData, 0, postData.Length);
            newStream.Flush();
            newStream.Close();

            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            if (myResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();

            //string postData = string.Format("OperID={2}&OperPass={3}&DesMobile={0}&Content={1}&ContentType=15", to, body, _username, _password);
            //string content = WebHelper.GetRequestData(_url, postData);
            if (content.Equals("error"))
            {
                return false;
            }

            int len1 = content.IndexOf("</code>");
            int len2 = content.IndexOf("<code>");
            string code = content.Substring((len2 + 6), (len1 - len2 - 6));

            //int len3 = content.IndexOf("</msg>");
            //int len4 = content.IndexOf("<msg>");
            //string msg = content.Substring((len4 + 5), (len3 - len4 - 5));

            //以下各种情况的判断要根据不同平台具体调整
            if (Convert.ToInt32(code) == 2)
            {
                return true;
            }
            else
            {
                if (content.Substring(0, 1) == "2") //余额不足
                {
                    //"手机短信余额不足";
                    //TODO
                }
                else
                {
                    //短信发送失败的其他原因
                    //TODO
                }
                return false;
            }
        }
    }
}
