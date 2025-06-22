using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ФНС32.ФНСDataSetTableAdapters;
using Excel = Microsoft.Office.Interop.Excel;

namespace ФНС32
{
    public partial class Form3 : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private BindingSource bindingSourceReport = new BindingSource(); // для Report
        private ФНС32.ФНСDataSet.Tax_calculationDataTable taxCalculationTable = new ФНС32.ФНСDataSet.Tax_calculationDataTable();
        private Tax_calculationTableAdapter taxCalculationTableAdapter = new Tax_calculationTableAdapter();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Все");
            comboBox1.Items.Add("ИП");
            comboBox1.Items.Add("Юридическое лицо");
            comboBox1.Items.Add("Физическое лицо");
            comboBox1.SelectedIndex = 0; // По умолчанию выбрать "Все"
            taxpayerDataGridView.ReadOnly = true;
            paymentDataGridView.ReadOnly = true;
            reportDataGridView.ReadOnly = true; // по умолчанию отчёты тоже только для чтения
            // Загружаем данные в таблицу Taxpayer
            this.taxpayerTableAdapter.Fill(this.фНСDataSet.Taxpayer);

            // Инициализируем BindingSource и привязываем к DataGridView
            bindingSource1.DataSource = this.фНСDataSet.Taxpayer;
            taxpayerDataGridView.DataSource = bindingSource1;

            // Запрет редактирования по умолчанию
            taxpayerDataGridView.ReadOnly = true;



            // Загрузка других данных, если нужно
            this.employeeTableAdapter.Fill(this.фНСDataSet.Employee);
            this.tax_documentTableAdapter.Fill(this.фНСDataSet.Tax_document);
            this.tax_calculationTableAdapter.Fill(this.фНСDataSet.Tax_calculation);
            this.reportTableAdapter.Fill(this.фНСDataSet.Report);
            this.paymentTableAdapter.Fill(this.фНСDataSet.Payment);
            reportDataGridView.DataSource = bindingSourceReport;

            reportDataGridView.ReadOnly = true;
            {
                // Загрузка данных в taxCalculationTable через TableAdapter
                taxCalculationTableAdapter.Fill(taxCalculationTable);
                tax_calculationDataGridView.DataSource = taxCalculationTable;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();
            if (selected == "Все")
            {
                bindingSource1.Filter = null;
            }
            else
            {
                bindingSource1.Filter = $"Tip = '{selected}'";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application excelApp = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel._Worksheet workSheet = null;

            try
            {
                excelApp = new Excel.Application();
                workbooks = excelApp.Workbooks;
                workbook = workbooks.Add();
                workSheet = (Excel._Worksheet)workbook.ActiveSheet;

                workSheet.Columns.ColumnWidth = 15;

                // Заголовки столбцов
                for (int i = 0; i < taxpayerDataGridView.ColumnCount; i++)
                {
                    workSheet.Cells[1, i + 1] = taxpayerDataGridView.Columns[i].HeaderText;
                }

                // Данные с учётом фильтрации
                for (int i = 0; i < taxpayerDataGridView.RowCount; i++)
                {
                    for (int j = 0; j < taxpayerDataGridView.ColumnCount; j++)
                    {
                        object value = taxpayerDataGridView[j, i].Value;
                        workSheet.Cells[i + 2, j + 1] = value != null ? value.ToString() : "";
                    }
                }

                excelApp.Visible = true;
            }
            finally
            {
                if (workSheet != null) Marshal.ReleaseComObject(workSheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (workbooks != null) Marshal.ReleaseComObject(workbooks);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form3 = new Form1();
            form3.Show();
            this.Hide();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Включаем редактирование таблицы по нажатию кнопки
            taxpayerDataGridView.ReadOnly = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Также включаем редактирование (если нужно)
            paymentDataGridView.ReadOnly = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form3 = new Form1();
            form3.Show();
            this.Hide();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.EndEdit();

                var changes = фНСDataSet.Taxpayer.GetChanges();
                if (changes == null)
                {
                    MessageBox.Show("Нет изменений для сохранения.");
                    return;
                }

                int updatedRows = taxpayerTableAdapter.Update(фНСDataSet.Taxpayer);
                if (updatedRows > 0)
                {
                    MessageBox.Show("Данные успешно сохранены.");
                    фНСDataSet.Taxpayer.AcceptChanges();
                    taxpayerDataGridView.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Сохранение не выполнено. Проверьте настройки TableAdapter.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }
        

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.EndEdit();
                taxpayerTableAdapter.Update(this.фНСDataSet.Taxpayer);
                MessageBox.Show("Данные успешно сохранены.");
                taxpayerDataGridView.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (paymentDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку для удаления.");
                    return;
                }

                // Подтверждение удаления
                if (MessageBox.Show("Вы действительно хотите удалить выбранную запись?", "Подтверждение удаления", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                // Удаляем выбранную строку из BindingSource и DataTable
                foreach (DataGridViewRow row in paymentDataGridView.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        bindingSource1.RemoveAt(row.Index);
                    }
                }

                // Сохраняем изменения в базу
                bindingSource1.EndEdit();
                paymentTableAdapter.Update(фНСDataSet.Payment);

                MessageBox.Show("Запись успешно удалена и сохранена.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, есть ли выбранные строки
                if (taxpayerDataGridView.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строки для удаления.");
                    return;
                }

                // Подтверждение удаления
                DialogResult dr = MessageBox.Show("Вы действительно хотите удалить выбранные записи?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr != DialogResult.Yes)
                    return;

                // Удаляем выбранные строки из BindingSource (и DataTable)
                foreach (DataGridViewRow row in taxpayerDataGridView.SelectedRows)
                {
                    if (!row.IsNewRow)
                    {
                        taxpayerDataGridView.Rows.Remove(row);
                    }
                }

                // Завершаем редактирование и сохраняем изменения в базу
                bindingSource1.EndEdit();
                taxpayerTableAdapter.Update(фНСDataSet.Taxpayer);

                MessageBox.Show("Выбранные записи успешно удалены и сохранены.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            reportDataGridView.ReadOnly = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                bindingSourceReport.EndEdit();
                reportTableAdapter.Update(this.фНСDataSet.Report);
                MessageBox.Show("Данные отчётов сохранены.");
                reportDataGridView.ReadOnly = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных отчётов: " + ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Form1 form3 = new Form1();
            form3.Show();
            this.Hide();
            this.Close();
        }


        private void ApplyFilters()
        {
            DataView dv = taxCalculationTable.DefaultView;

            List<string> filters = new List<string>();

            string searchText = textBox1.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                string escapedText = searchText.Replace("'", "''");
                filters.Add($"Tip_naloga LIKE '%{escapedText}%'");
            }

            DateTime fromDate = dateTimePicker1.Value.Date;
            DateTime toDate = dateTimePicker2.Value.Date;
            filters.Add($"Accrual_period >= #{fromDate:MM/dd/yyyy}# AND Accrual_period <= #{toDate:MM/dd/yyyy}#");

            dv.RowFilter = string.Join(" AND ", filters);
        }

        private DataView GetDataViewFromDataGridView(DataGridView dgv)
        {
            if (dgv.DataSource is BindingSource bs)
            {
                if (bs.DataSource is DataTable dt)
                    return dt.DefaultView;
                if (bs.DataSource is DataView dv)
                    return dv;
            }
            else if (dgv.DataSource is DataTable dtDirect)
            {
                return dtDirect.DefaultView;
            }
            else if (dgv.DataSource is DataView dvDirect)
            {
                return dvDirect;
            }

            return null;
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Form1 form3 = new Form1();
            form3.Show();
            this.Hide();
            this.Close();
        }
    }
}