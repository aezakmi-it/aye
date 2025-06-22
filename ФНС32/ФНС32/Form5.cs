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
    public partial class Form5 : Form
    {
        // Правильно: используем тип TextBox, а не TextBox3
        private TextBox textBoxEmployeeId;

        public Form5()
        {
            InitializeComponent();
            InitializeComboBoxes();
            InitializeEmployeeIdTextBox();
        }

        private void InitializeComboBoxes()
        {
            comboBox1.Items.AddRange(new string[] { "Физическое лицо", "ИП", "Юридическое лицо" });
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.AddRange(new string[] { "Годовой", "Квартальный", "Месячный" });
            comboBox2.SelectedIndex = 0;
        }
        private void InitializeEmployeeIdTextBox()
        {
            textBoxEmployeeId = new TextBox();
            textBoxEmployeeId.Location = new Point(10, 150);
            textBoxEmployeeId.Width = 200;
            textBoxEmployeeId.Name = "textBoxEmployeeId";
            this.Controls.Add(textBoxEmployeeId);
        }

        private decimal GetTaxRate(string taxpayerType)
        {
            switch (taxpayerType)
            {
                case "Физическое лицо": return 13m;
                case "ИП": return 15m;
                case "Юридическое лицо": return 20m;
                default: return 0m;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(textBox1.Text, out decimal income) || income < 0)
            {
                MessageBox.Show("Введите корректную сумму дохода.");
                return;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out int idEmployee))
            {
                MessageBox.Show("Введите корректный номер сотрудника.");
                return;
            }

            string taxpayerType = comboBox1.SelectedItem.ToString();
            string reportType = comboBox2.SelectedItem.ToString();
            string inn = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(inn))
            {
                MessageBox.Show("Введите корректный ИНН.");
                return;
            }

            decimal taxRate = GetTaxRate(taxpayerType);
            decimal taxAmount = income * taxRate / 100m;

            Form6 form6 = new Form6(taxpayerType, reportType, income, taxAmount, inn,  idEmployee);
            form6.ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            Form4 form2 = new Form4();
            form2.Show();
            this.Hide();
            this.Close();

        }
    }
}
