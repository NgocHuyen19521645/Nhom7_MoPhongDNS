using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNS_Simulation
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.tbServerName.Text == "" || this.tbPassword.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (this.tbServerName.Text != "Server" || this.tbPassword.Text != "UIT")
                {
                    MessageBox.Show("Sai thông tin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    ServerActionForm serverActionForm = new ServerActionForm();
                    serverActionForm.Show();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
