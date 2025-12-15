using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class Form1
    {
        Button btnAdd, btnStudy, btnCards, btnImportExcel;
        void InitializeComponent()
        {
            btnAdd = new() { Text = "Добавити картку", Location = new(30, 20), Size = new(200, 40) };
            btnStudy = new() { Text = "Вчити", Location = new(30, 70), Size = new(200, 40) };
            btnCards = new() { Text = "Всі картки", Location = new(30, 120), Size = new(200, 40) };

            btnAdd.Click += btnAdd_Click;
            btnStudy.Click += btnStudy_Click;
            btnCards.Click += btnCards_Click;

            Controls.AddRange(new Control[] { btnAdd, btnStudy, btnCards });
            ClientSize = new(260, 190);
            Text = "Flashcards";
            StartPosition = FormStartPosition.CenterScreen;
            
            btnImportExcel = new()
            {
                Text = "Імпортувати",
                Location = new(30, 170),
                Size = new(200, 40)
            };

            btnImportExcel.Click += btnImportExcel_Click;
            Controls.Add(btnImportExcel);
            ClientSize = new(260, 240);

        }
    }
}