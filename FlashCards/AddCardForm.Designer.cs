using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class AddCardForm
    {
        TextBox txtQ, txtA;
        Button btnSave;
        void InitializeComponent()
        {
            txtQ = new() { Location = new(20, 20), Width = 360 };
            txtA = new() { Location = new(20, 60), Width = 360 };
            btnSave = new() { Text = "Зберегти", Location = new(20, 100), Width = 360 };

            btnSave.Click += btnSave_Click;

            Controls.AddRange(new Control[] { txtQ, txtA, btnSave });
            ClientSize = new(410, 160);
            Text = "Добавити картку";
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}