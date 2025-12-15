using System;
using System.Windows.Forms;

namespace FlashcardsApp
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void btnAdd_Click(object s, EventArgs e)
            => new AddCardForm().ShowDialog();

        private void btnStudy_Click(object s, EventArgs e)
            => new StudyForm().ShowDialog();

        private void btnCards_Click(object s, EventArgs e)
            => new CardsForm().ShowDialog();
        
        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            new ImportExcelForm().ShowDialog();
        }

    }
}