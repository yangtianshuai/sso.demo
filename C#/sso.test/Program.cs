using System;
using System.Threading;

namespace sso.test
{
    class Program
    {
        static void Main(string[] args)
        {

            string app_id = "efa952dbd5c64eceb0e62db923079760";
            string secret = "7a41d87e0828494bbc35abfce088641e";

            string user_type = "GH";  //账户类别：OA（内网OA）、GH（工号）、DSF（第三方）
            string user_name = "0745";  //登录用户名
            string password = "0745";  //登录密码           

            string access_token = ""; //SSO令牌

            foreach(string arg in args)
            {
                if (arg.Contains("access_token"))
                {
                    //获取第三方应用传递过来的access_token
                    access_token = arg.Replace("access_token=", "");
                    break;
                }
            }

            var sso = new sso.OAuth2(app_id, secret);

            if (sso.Connect())
            {
                if (string.IsNullOrEmpty(access_token))
                {
                    //没有access_token，打开登录页面
                    access_token = sso.GetToken(user_type, user_name, password);
                }
                else
                {
                    access_token = sso.GetToken("","","");
                }
            }
            else
            {
                //插件无法连接
                access_token = null;
            }

            if (string.IsNullOrEmpty(access_token))
            {                
                //可能由于本地插件未注册，导致无法获取AccessToken
                //不再使用单点登录

                return;
            }

            //获取SSO用户信息
            var user = sso.GetUser(access_token);

            //sso.Logout();

            int seconds = 60;
            //定时检测登录是否失效
            var thread = new Thread(() =>
            {
                while (!string.IsNullOrEmpty(access_token))
                {
                    Thread.Sleep(1000 * seconds);
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

            Console.ReadKey();

        }
    }
}
