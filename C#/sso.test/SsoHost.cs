using System;
using System.Threading;
using System.Windows.Forms;

namespace sso.test
{
    public class SsoHost
    {
        //全局变量
        public static string access_token = "";
        public static sso.OAuth2 sso = null;

        public static sso.SsoUser user = null;
      
        public delegate void Logout();
        public static Logout LogoutCallBack;

        private static int _seconds = 60;

        ~SsoHost()
        {
            access_token = null;
        }

        public static void Run(int seconds = 60)
        {
            _seconds = seconds;
            //定时检测登录是否失效
            var thread = new Thread(() =>
            {
                while (!string.IsNullOrEmpty(access_token))
                {
                    Thread.Sleep(1000 * _seconds);
                    if (sso.HeatBeat(access_token))
                    {                        
                        access_token = sso.Token();
                    }
                    else
                    {
                        access_token = null;
                    }
                    if (string.IsNullOrEmpty(access_token))
                    {
                        access_token = sso.GetToken("", "", "");
                        //心跳结束，系统票据已经耗尽，需要提示用户
                        if (string.IsNullOrEmpty(access_token))
                        {
                            LogoutCallBack?.Invoke();
                        }
                    }
                }                

            });
            thread.IsBackground = true;
            thread.Start();
        }

    }
}
