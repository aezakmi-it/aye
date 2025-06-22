using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ФНС32
{
    internal static class Program
    {
        public static ФНСDataSet фНСDataSet = new ФНСDataSet();
        public static ФНСDataSetTableAdapters.EmployeeTableAdapter employeeTableAdapter = new ФНСDataSetTableAdapters.EmployeeTableAdapter();
        public static ФНСDataSetTableAdapters.ReportTableAdapter reportTableAdapter = new ФНСDataSetTableAdapters.ReportTableAdapter();
        public static ФНСDataSetTableAdapters.PaymentTableAdapter paymentTableAdapter = new ФНСDataSetTableAdapters.PaymentTableAdapter();
        public static ФНСDataSetTableAdapters.Tax_documentTableAdapter taxDocumentTableAdapter = new ФНСDataSetTableAdapters.Tax_documentTableAdapter();

        // Добавляем строку подключения к базе данных
        // Замените YOUR_SERVER и YOUR_DATABASE на реальные значения вашей базы данных
        public static string ConnectionString = @"Data Source=localhost;Initial Catalog=ФНС;Integrated Security=True";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            reportTableAdapter.Fill(фНСDataSet.Report);
            paymentTableAdapter.Fill(фНСDataSet.Payment);
            taxDocumentTableAdapter.Fill(фНСDataSet.Tax_document);
            employeeTableAdapter.Fill(фНСDataSet.Employee);

            Application.Run(new Form1());
        }
    }
}
