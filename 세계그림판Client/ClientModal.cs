using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 세계그림판Client
{
    public partial class ClientModal : Form
    {
        public ClientModal()
        {
            InitializeComponent();

            txtIP.Text = "";
            txtPort.Text = "";
            txtUser.Text = "";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // IP, port, user name check
            if (txtIP.Text.Equals("") || txtPort.Text.Equals("") || txtUser.Text.Equals(""))
            {
                MessageBox.Show("IP나 Port나 ID를 입력했는지 확인해주세요~", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int num = 0;
            if(Int32.TryParse(txtPort.Text, out num) == false)
            {
                MessageBox.Show("Port에 숫자를 입력해주세요~", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form1 form = new Form1(txtIP.Text, num, txtUser.Text);
            form.ShowDialog();
        }
    }
}
