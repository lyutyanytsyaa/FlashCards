using System;
using System.Windows.Forms;

namespace FlashcardsApp
{
    public partial class EditCardForm : Form
    {
        Card c;
        public EditCardForm(Card card)
        {
            InitializeComponent();
            c = card;
            txtQ.Text = c.Question;
            txtA.Text = c.Answer;
        }

        private void save_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQ.Text) || string.IsNullOrWhiteSpace(txtA.Text))
            {
                MessageBox.Show("Fill both fields");
                return;
            }
            c.Question = txtQ.Text.Trim();
            c.Answer = txtA.Text.Trim();
            Database.Update(c);
            Close();
        }
    }
}