using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class ImportExcelForm
    {
        TextBox txtPath;
        Button btnBrowse, btnImport;

        void InitializeComponent()
        {
            txtPath = new() { Location = new(20, 20), Width = 300 };
            btnBrowse = new() { Text = "Пошук", Location = new(330, 20) };
            btnImport = new() { Text = "Імпорт Excel", Location = new(20, 70), Width = 380 };

            btnBrowse.Click += btnBrowse_Click;
            btnImport.Click += btnImport_Click;

            Controls.AddRange(new Control[] { txtPath, btnBrowse, btnImport });
            ClientSize = new(450, 130);
            Text = "Імпорт з Excel (.xlsx)";
        }
    }
}