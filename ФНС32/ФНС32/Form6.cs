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
using ФНС32.ФНСDataSetTableAdapters;

namespace ФНС32
{
    public partial class Form6 : Form
    {
        private string taxpayerType;
        private string reportType;
        private decimal income;
        private decimal taxAmount;
        private string inn;
        private int idEmployee;

        private TaxpayerTableAdapter taxpayerTableAdapter = new TaxpayerTableAdapter();
        private Tax_documentTableAdapter taxDocumentTableAdapter = new Tax_documentTableAdapter();
        private ReportTableAdapter reportTableAdapter = new ReportTableAdapter();
        private PaymentTableAdapter paymentTableAdapter = new PaymentTableAdapter();
        private Tax_calculationTableAdapter taxCalculationTableAdapter = new Tax_calculationTableAdapter();

        public Form6(string taxpayerType, string reportType, decimal income, decimal taxAmount, string inn, int idEmployee)
        {
            InitializeComponent();
            this.taxpayerType = taxpayerType;
            this.reportType = reportType;
            this.income = income;
            this.taxAmount = taxAmount;
            this.inn = inn;
            this.idEmployee = idEmployee;

            DisplayData();
        }

        private void DisplayData()
        {
            labelTaxpayerTypeValue.Text = taxpayerType;
            labelReportTypeValue.Text = reportType;
            labelIncomeValue.Text = income.ToString("F2");
            labelTaxAmountValue.Text = taxAmount.ToString("F2");
            labelINNValue.Text = inn;
        }

        private bool TaxpayerExists(string inn)
        {
            var table = Program.фНСDataSet.Taxpayer;
            var rows = table.Select($"INN = '{inn}'");
            return rows.Length > 0;
        }

        private string GetEmployeeFioById(int idEmployee)
        {
            var employeeTable = Program.фНСDataSet.Employee;
            var rows = employeeTable.Select($"ID_employee = {idEmployee}");
            if (rows.Length > 0)
                return rows[0]["FIO"].ToString();
            else
                return null;
        }

        private int GetNextId(DataTable table, string idColumnName)
        {
            if (table.Rows.Count == 0)
                return 1;

            object maxIdObj = table.Compute($"MAX({idColumnName})", "");
            return (maxIdObj != DBNull.Value) ? Convert.ToInt32(maxIdObj) + 1 : 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                if (string.IsNullOrWhiteSpace(inn))
                {
                    MessageBox.Show("ИНН пустой, сохранение невозможно.");
                    return;
                }

                taxpayerTableAdapter.Fill(Program.фНСDataSet.Taxpayer);
                if (!TaxpayerExists(inn))
                {
                    MessageBox.Show("Налогоплательщик с таким ИНН не найден. Сначала добавьте его в базу!");
                    return;
                }

                string employeeFio = GetEmployeeFioById(idEmployee);
                if (employeeFio == null)
                {
                    MessageBox.Show("Сотрудник с таким ID не найден.");
                    return;
                }

                var taxDocTable = Program.фНСDataSet.Tax_document;
                var reportTable = Program.фНСDataSet.Report;
                var paymentTable = Program.фНСDataSet.Payment;
                var taxCalcTable = Program.фНСDataSet.Tax_calculation;

                // Обновляем таблицы из базы
                taxDocumentTableAdapter.Fill(taxDocTable);
                reportTableAdapter.Fill(reportTable);
                paymentTableAdapter.Fill(paymentTable);
                taxCalculationTableAdapter.Fill(taxCalcTable);

                // Вычисляем новые ID вручную
                int newDocumentId = GetNextId(taxDocTable, "ID_document");
                int newReportId = GetNextId(reportTable, "ID_report");
                int newAccrualId = GetNextId(taxCalcTable, "ID_accrual");
                int newPaymentId = GetNextId(paymentTable, "ID_payment");

                // Создаём новую запись в Tax_document
                var newDocRow = taxDocTable.NewRow();
                newDocRow["ID_document"] = newDocumentId;
                newDocRow["Data_podachi"] = DateTime.Now;
                newDocRow["Tip_documenta"] = "Отчет";
                newDocRow["Status"] = "Обрабатывается";
                newDocRow["INN"] = inn;
                taxDocTable.Rows.Add(newDocRow);
                taxDocumentTableAdapter.Update(taxDocTable);

                // Создаём новую запись в Report
                var newReportRow = reportTable.NewRow();
                newReportRow["ID_report"] = newReportId;
                newReportRow["Formation_date"] = DateTime.Now;
                newReportRow["Tip_report"] = reportType;
                newReportRow["ID_document"] = newDocumentId;
                newReportRow["INN"] = inn;
                newReportRow["ID_employee"] = idEmployee;
                reportTable.Rows.Add(newReportRow);
                reportTableAdapter.Update(reportTable);

                // Создаём новую запись в Tax_calculation
                var newTaxCalcRow = taxCalcTable.NewRow();
                newTaxCalcRow["ID_accrual"] = newAccrualId; // обязательно задаём ID_accrual
                newTaxCalcRow["Tip_naloga"] = taxpayerType;
                newTaxCalcRow["Accrual_amount"] = taxAmount;
                newTaxCalcRow["Accrual_period"] = DateTime.Now;
                newTaxCalcRow["ID_document"] = newDocumentId;
                taxCalcTable.Rows.Add(newTaxCalcRow);
                taxCalculationTableAdapter.Update(taxCalcTable);

                // Создаём новую запись в Payment
                var newPaymentRow = paymentTable.NewRow();
                newPaymentRow["ID_payment"] = newPaymentId;
                newPaymentRow["Data_payment"] = DateTime.Now;
                newPaymentRow["Amount"] = taxAmount;
                newPaymentRow["ID_accrual"] = newAccrualId; // ссылка на Tax_calculation
                paymentTable.Rows.Add(newPaymentRow);
                paymentTableAdapter.Update(paymentTable);

                MessageBox.Show("Данные успешно сохранены.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            // Код инициализации, если нужен
        }
    }
}
