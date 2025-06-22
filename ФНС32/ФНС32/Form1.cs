using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ФНС32
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login1 = "123";
            string password1 = "123";

            string login2 = "111";
            string password2 = "111";

            if (textBox1.Text == login1 && textBox2.Text == password1)
            {
                Form4 wf = new Form4();
                wf.Show();
                this.Hide();
            }
            else if (textBox1.Text == login2 && textBox2.Text == password2)
            {
                Form3 wf = new Form3();
                wf.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
