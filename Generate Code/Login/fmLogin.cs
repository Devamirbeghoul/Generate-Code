using Generate_Code.Generate;
using Generate_Code.Global_Classes;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generate_Code
{
    public partial class fmLogin : Form
    {
        public fmLogin()
        {
            InitializeComponent();
        }

        private void Validate(object sender, CancelEventArgs e)
        {
            Guna2TextBox tb = sender as Guna2TextBox;

            if (string.IsNullOrEmpty(tb.Text))
            {
                e.Cancel = true;
                epEmpty.SetError(tb, "This field is required!");
            }
            else
                epEmpty.SetError(tb , string.Empty);
        }

        private void fmLogin_Activated(object sender, EventArgs e)
        {
            tbServer.Focus();
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields are Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool IsConnected = await clsConnection.Connect(tbServer.Text.Trim(), tbUserId.Text.Trim(), tbPassword.Text.Trim());

            if (IsConnected)
            {
                if (cbRememberMe.Checked)
                    clsSystem.SaveLoginInformationInRegistry(tbServer.Text.Trim(), tbUserId.Text.Trim(), tbPassword.Text.Trim());
                else
                    clsSystem.SaveLoginInformationInRegistry("", "", "");

                fmSetInfo NewSet = new fmSetInfo();

                NewSet.ShowDialog();
            }
            else
                MessageBox.Show("Error: Cannot connect to server. Please check your credentials and try again.",
                                "Connection Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }

        private void fmLogin_Load(object sender, EventArgs e)
        {
            string Server = string.Empty, UserId = string.Empty, Password = string.Empty;

            if (clsSystem.RestoreLoginInformationFromRegistry(ref Server, ref UserId, ref Password))
            {
                tbServer.Text = Server;
                tbUserId.Text = UserId;
                tbPassword.Text = Password;
                cbRememberMe.Checked = true;
            }
        }
    }
}
