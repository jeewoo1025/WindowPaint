using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 세계그림판
{
    public partial class ServerModal : Form
    {
        public ServerModal()
        {
            InitializeComponent();

            txtIP.Text = "";
            txtPort.Text = "";
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Console.WriteLine(txtIP +", " + txtPort);

            // IP, port 체크
            if(txtIP.Text.Equals("") || txtPort.Text.Equals(""))
            {
                MessageBox.Show("IP나 Port를 입력했는지 확인해주세요~", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int num = 0;
            if(Int32.TryParse(txtPort.Text, out num) == false)
            {
                MessageBox.Show("Port에 숫자를 입력해주세요~", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form1 form = new Form1(txtIP.Text, num);
            form.ShowDialog();
        }
    }
}
