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

            var sso = new sso.OAuth2(app_id, secret);  

            if (string.IsNullOrEmpty(access_token))
            {
                //没有access_token，登录
                if (sso.Connect())
                {
                    access_token = sso.GetToken(user_type, user_name, password);
                }
            }

            //获取SSO用户信息
            var user = sso.GetUser(access_token);

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
                }
            });
            thread.IsBackground = true;
            thread.Start();

            Console.ReadKey();

        }
    }
}
