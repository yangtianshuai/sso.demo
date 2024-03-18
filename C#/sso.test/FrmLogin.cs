using System.Windows.Forms;

namespace sso.test
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string user_type = "GH";  //账户类别：OA（内网OA）、GH（工号）、DSF（第三方）
            string user_name = "0745";  //登录用户名
            string password = "0745";  //登录密码    

            user_name = this.tb_user_name.Text;
            password = this.tb_password.Text;

            SsoHost.access_token = SsoHost.sso.GetToken(user_type, user_name, password);

            if (!string.IsNullOrEmpty(SsoHost.access_token))
            {
                Program.IsLogin = true;                
            }
            else
            {
                //原系统登录逻辑
            }

            this.Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}