using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class EditCardForm
    {
        TextBox txtQ, txtA;
        Button save;
        void InitializeComponent()
        {
            txtQ = new() { Location = new(20, 20), Width = 360 };
            txtA = new() { Location = new(20, 60), Width = 360 };
            save = new() { Text = "Зберегти", Location = new(20, 100), Width = 360 };

            save.Click += save_Click;

            Controls.AddRange(new Control[] { txtQ, txtA, save });
            ClientSize = new(410, 160);
            Text = "Редагувати";
            StartPosition = FormStartPosition.CenterParent;
        }
    }
}