using System;
using System.Threading;
using System.Windows.Forms;

namespace sso.test
{
    public class Program
    {
        public static bool IsLogin = false;

        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                if (arg.Contains("access_token"))
                {
                    //获取第三方应用传递过来的access_token
                    SsoHost.access_token = arg.Replace("access_token=", "");
                    break;
                }
            }

            //从配置获取
            string app_id = "efa952dbd5c64eceb0e62db923079760";
            string secret = "7a41d87e0828494bbc35abfce088641e";

            SsoHost.sso = new sso.OAuth2(app_id, secret);
            if (SsoHost.sso.Connect())
            {
                SsoHost.access_token = SsoHost.sso.GetToken("", "", "");
            }
            else
            {
                //可能由于本地插件未注册，导致无法连接
                SsoHost.access_token = null;
            }
           
            if (!string.IsNullOrEmpty(SsoHost.access_token))
            {
                Program.IsLogin = true;
            }
            else
            {
                Application.Run(new FrmLogin());
            }

            if (IsLogin)
            {
                SsoHost.Run();
                Application.Run(new FrmMain());
            }           

        }
    }
}
