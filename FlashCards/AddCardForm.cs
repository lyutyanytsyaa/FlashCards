using System;
using System.Windows.Forms;

namespace FlashcardsApp
{
    public partial class AddCardForm : Form
    {
        public AddCardForm() => InitializeComponent();

        private void btnSave_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQ.Text) || string.IsNullOrWhiteSpace(txtA.Text))
            {
                MessageBox.Show("Fill both fields");
                return;
            }
            Database.AddCard(txtQ.Text.Trim(), txtA.Text.Trim());
            MessageBox.Show("Saved");
            txtQ.Clear(); txtA.Clear();
        }
    }
}