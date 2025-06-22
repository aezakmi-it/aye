using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ФНС32
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            this.Load += Form7_Load;
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Ежемесячный");
            comboBox1.Items.Add("Квартальный");
            comboBox1.Items.Add("Годовой");
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Здесь можно добавить логику при изменении выбора, если нужно
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Возврат на предыдущую форму (Form3)
            Form3 form3 = new Form3();
            form3.Show();
            this.Close(); // Закрываем текущую форму
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Считываем данные из полей
            string tipReport = comboBox1.SelectedItem?.ToString();
            string reportNumberText = textBox1.Text.Trim();    // Номер отчёта (если нужен)
            string formationDateText = textBox2.Text.Trim();   // Дата формирования (если нужна, сейчас не используется)
            string documentNumberText = textBox3.Text.Trim();  // Номер документа
            string innText = textBox4.Text.Trim();              // ИНН
            string employeeNumberText = textBox5.Text.Trim();  // Номер сотрудника

            // Проверка обязательных полей
            if (string.IsNullOrEmpty(reportNumberText) ||
                string.IsNullOrEmpty(documentNumberText) ||
                string.IsNullOrEmpty(innText) ||
                string.IsNullOrEmpty(employeeNumberText))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            // Парсим дату формирования, если нужно использовать из textBox2, иначе используем DateTime.Now
            DateTime formationDate = DateTime.Now;
            if (!string.IsNullOrEmpty(formationDateText))
            {
                if (!DateTime.TryParse(formationDateText, out formationDate))
                {
                    MessageBox.Show("Введите корректную дату формирования отчёта.");
                    return;
                }
            }

            // Здесь предполагаем, что номера документов, ИНН и номера сотрудников — строки, если они числовые, парсите аналогично
            // Если нужно, проверяйте формат и длину ИНН и других полей

            // Строка подключения — замените на вашу реальную строку подключения
            string connectionString = "ваша_строка_подключения";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Report 
                        (ReportNumber, Formation_date, Tip_report, DocumentNumber, INN, EmployeeNumber)
                        VALUES 
                        (@ReportNumber, @Formation_date, @Tip_report, @DocumentNumber, @INN, @EmployeeNumber)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReportNumber", reportNumberText);
                        command.Parameters.AddWithValue("@Formation_date", formationDate);
                        command.Parameters.AddWithValue("@Tip_report", tipReport);
                        command.Parameters.AddWithValue("@DocumentNumber", documentNumberText);
                        command.Parameters.AddWithValue("@INN", innText);
                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumberText);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Данные успешно сохранены.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }
    }
}