using OfficeOpenXml;
using System;
using System.IO;
using System.Windows.Forms;

namespace FlashcardsApp
{
    public partial class ImportExcelForm : Form
    {
        public ImportExcelForm()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
                txtPath.Text = ofd.FileName;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtPath.Text))
            {
                MessageBox.Show("File not found");
                return;
            }

            int count = 0;

            using var package = new ExcelPackage(new FileInfo(txtPath.Text));
            var ws = package.Workbook.Worksheets[0];

            int rows = ws.Dimension.Rows;

            for (int i = 2; i <= rows; i++) // з 2-го рядка
            {
                string q = ws.Cells[i, 1].Text;
                string a = ws.Cells[i, 2].Text;

                if (string.IsNullOrWhiteSpace(q) ||
                    string.IsNullOrWhiteSpace(a))
                    continue;

                Database.AddCard(q, a);
                count++;
            }

            MessageBox.Show($"Imported {count} cards from Excel ✅");
            Close();
        }
    }
}