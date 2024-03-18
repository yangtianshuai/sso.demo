using System.Threading;

namespace sso.test
{
    public class SsoHost
    {
        //全局变量
        public static string access_token = "";
        public static sso.OAuth2 sso = null;

        public static sso.SsoUser user = null;

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
                        //登录退出
                        access_token = sso.Token();
                    }
                    else
                    {
                        access_token = null;
                    }
                    if (string.IsNullOrEmpty(access_token))
                    {
                        access_token = sso.GetToken("", "", "");
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

    }
}
