using System;
using System.Windows.Forms;

namespace sso.test
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (SsoHost.sso.Logout())
            {
                MessageBox.Show("退出成功");
                this.Close();
            }
            else
            {
                MessageBox.Show("退出失败：" + SsoHost.sso.Error());
            }
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(SsoHost.access_token))
            {
                SsoHost.LogoutCallBack = () =>
                {
                    var res = MessageBox.Show("登录票据已过期，是否重新登录", "安全提示", MessageBoxButtons.YesNo);
                    if(res == DialogResult.Yes)
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            this.Close();
                        });                        
                    }
                };
                //获取SSO用户信息
                SsoHost.user = SsoHost.sso.GetUser();                
            }
           
            if (SsoHost.user != null)
            {
                this.log.Text += "用户ID：" + SsoHost.user.id + "\r\n";
                this.log.Text += "用户姓名：" + SsoHost.user.name + "\r\n";
                this.log.Text += "登录账号：" + SsoHost.user.login_name + "\r\n";
                this.log.Text += "员工ID：" + SsoHost.user.employee_id + "\r\n";
                this.log.Text += "工号：" + SsoHost.user.ext.user_name + "\r\n";
                this.log.Text += "Job：" + SsoHost.user.ext.job + "\r\n";
                this.log.Text += "Title：" + SsoHost.user.ext.title + "\r\n";
            }
        }
    }
}