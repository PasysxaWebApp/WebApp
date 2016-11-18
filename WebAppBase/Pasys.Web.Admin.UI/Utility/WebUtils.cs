using Pasys.Web.Core;
using SharedUtilitys.Helper;
using System;
using System.IO;
using System.Web;

namespace Pasys.Web.Admin.UI.Utility
{
    public partial class WebUtils
    {
        private static object _locker = new object();//锁对象
        private static GlobalConfigInfo globalConfig = null;

        static WebUtils()
        {
            globalConfig = Pasys.Web.Core.ConfigManager.GetGlobalConfig();
        }

        #region  加密/解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">加密字符串</param>
        public static string AESEncrypt(string encryptStr)
        {
            return SecureHelper.AESEncrypt(encryptStr, globalConfig.SecretKey);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">解密字符串</param>
        public static string AESDecrypt(string decryptStr)
        {
            return SecureHelper.AESDecrypt(decryptStr, globalConfig.SecretKey);
        }

        #endregion

        #region Cookies
        /// <summary>
        /// 获得用户sid
        /// </summary>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            return AESDecrypt(WebHelper.GetCookie(key));
        }

        /// <summary>
        /// 设置用户sid
        /// </summary>
        public static void SetCookie(string key, string val)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
                cookie = new HttpCookie(key);

            cookie.Value = AESEncrypt(val);
            cookie.Expires = DateTime.Now.AddDays(15);
            string cookieDomain = globalConfig.CookieDomain;
            if (cookieDomain.Length != 0)
                cookie.Domain = cookieDomain;

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        #endregion

        #region  日志

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input">输入内容</param>
        public static void WriteLogFile(Exception ex)
        {
            WriteLogFile(string.Format("方法:{0},异常信息:{1}", ex.TargetSite, ex.Message));
        }

        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input">输入内容</param>
        public static void WriteLogFile(string input)
        {
            lock (_locker)
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    string fileName = IOHelper.GetMapPath("/App_Data/exlogs/") + DateTime.Now.ToString("yyyyMMdd") + ".log";

                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    if (!fileInfo.Exists)
                    {
                        fileInfo.Create().Close();
                    }
                    else if (fileInfo.Length > 2048 * 1000)
                    {
                        fileInfo.Delete();
                    }

                    fs = fileInfo.OpenWrite();
                    sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.Write("Log Entry : ");
                    sw.Write("{0}", DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss"));
                    sw.Write(Environment.NewLine);
                    sw.Write(input);
                    sw.Write(Environment.NewLine);
                    sw.Write("------------------------------------");
                    sw.Write(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        #endregion
    }
}
