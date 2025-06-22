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
    public partial class Form2 : Form
    {
        private SqlDataAdapter adapter;
        private DataSet ds;
        public Form2()
        {
            InitializeComponent();
        }

        private void taxpayerBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.taxpayerBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.фНСDataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Employee". При необходимости она может быть перемещена или удалена.
            this.employeeTableAdapter.Fill(this.фНСDataSet.Employee);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Report". При необходимости она может быть перемещена или удалена.
            this.reportTableAdapter.Fill(this.фНСDataSet.Report);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Payment". При необходимости она может быть перемещена или удалена.
            this.paymentTableAdapter.Fill(this.фНСDataSet.Payment);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Tax_calculation". При необходимости она может быть перемещена или удалена.
            this.tax_calculationTableAdapter.Fill(this.фНСDataSet.Tax_calculation);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Tax_document". При необходимости она может быть перемещена или удалена.
            this.tax_documentTableAdapter.Fill(this.фНСDataSet.Tax_document);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "фНСDataSet.Taxpayer". При необходимости она может быть перемещена или удалена.
            this.taxpayerTableAdapter.Fill(this.фНСDataSet.Taxpayer);

        }
        // Универсальный метод добавления записи
        private void AddNewRecord(BindingSource source)
        {
            var newRow = source.AddNew();

            // Здесь можно добавить логику заполнения обязательных полей, если нужно
            // Например, для автоинкрементных полей это не требуется
        }

        // Универсальный метод удаления записи с подтверждением
        private void DeleteCurrentRecord(BindingSource source)
        {
            if (source.Current != null)
            {
                var result = MessageBox.Show(
                    "Вы уверены, что хотите удалить выбранную запись?",
                    "Подтверждение удаления",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    source.RemoveCurrent();
                    SaveAllChanges();
                }
            }
        }

        // Метод сохранения изменений
        private void SaveAllChanges()
        {
            try
            {
                this.Validate();

                taxpayerBindingSource.EndEdit();
                employeeBindingSource.EndEdit();
                reportBindingSource.EndEdit();
                paymentBindingSource.EndEdit();
                tax_calculationBindingSource.EndEdit();
                tax_documentBindingSource.EndEdit();

                this.tableAdapterManager.UpdateAll(this.фНСDataSet);

                MessageBox.Show("Данные сохранены успешно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        // Пример обработчиков кнопок для каждой таблицы с твоими названиями

        // Taxpayer
        private void button1_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(taxpayerBindingSource);
        }

        private void button3_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(taxpayerBindingSource);
        }

        // Employee
        private void button4_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(employeeBindingSource);
        }

        private void button6_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(employeeBindingSource);
        }

        // Report
        private void button7_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(reportBindingSource);
        }

        private void button9_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(reportBindingSource);
        }

        // Payment
        private void button10_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(paymentBindingSource);
        }

        private void button12_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(paymentBindingSource);
        }

        // Tax_calculation
        private void button13_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(tax_calculationBindingSource);
        }

        private void button15_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(tax_calculationBindingSource);
        }

        // Tax_document
        private void button16_Click(object sender, EventArgs e) // Добавить
        {
            AddNewRecord(tax_documentBindingSource);
        }

        private void button18_Click(object sender, EventArgs e) // Удалить
        {
            DeleteCurrentRecord(tax_documentBindingSource);
        }

        // Кнопка сохранения для всех изменений
        private void button2_Click(object sender, EventArgs e)
        {
            SaveAllChanges();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form4 form2 = new Form4();
            form2.Show();
            this.Hide();
            this.Close();
        
    }

        private void button8_Click(object sender, EventArgs e)
        {
            taxpayerDataGridView.ReadOnly = false; // разрешаем редактирование
            button8.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tax_documentDataGridView.ReadOnly = false; // разрешаем редактирование
            button11.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }

        private void button14_Click(object sender, EventArgs e)
        {
            tax_calculationDataGridView.ReadOnly = false; // разрешаем редактирование
            button14.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }

        private void button17_Click(object sender, EventArgs e)
        {
            paymentDataGridView.ReadOnly = false; // разрешаем редактирование
            button17.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }

        private void button19_Click(object sender, EventArgs e)
        {
            reportDataGridView.ReadOnly = false; // разрешаем редактирование
            button19.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }

        private void button20_Click(object sender, EventArgs e)
        {
            employeeDataGridView.ReadOnly = false; // разрешаем редактирование
            button20.Enabled = false;    // можно отключить кнопку редактирования после нажатия
        }
    }
}


