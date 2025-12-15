using System.Drawing;
using System.Windows.Forms;

namespace FlashcardsApp
{
    partial class CardsForm
    {
        DataGridView dgv;
        TextBox txtSearch;
        Button btnAll, btnHard, btnMedium, btnEasy;
        ComboBox cmbSort;
        Button btnAdd, btnEdit, btnDel;

        // stats panel controls
        Panel pnlStats;
        Label lblTotal, lblR0, lblR1, lblR2, lblToday, lblAvg;

        void InitializeComponent()
        {
            dgv = new() { Location = new(20, 70), Size = new(560, 340) };
            txtSearch = new() { Location = new(20, 20), Width = 200 };
            btnAll = new() { Text = "Всі", Location = new(230, 18), Width = 80 };
            btnHard = new() { Text = "Не знаю", Location = new(320, 18), Width = 80 };
            btnMedium = new() { Text = "Так собі", Location = new(410, 18), Width = 80 };
            btnEasy = new() { Text = "Знаю", Location = new(500, 18), Width = 80 };

            cmbSort = new() { Location = new(590, 18), Width = 100 };
            cmbSort.Items.AddRange(new[] { "За рейтингом", "За словом", "За алфавітом" });
            cmbSort.SelectedIndex = 0;

            btnAdd = new() { Text = "Добавити", Location = new(700, 18), Width = 80 };
            btnEdit = new() { Text = "Редагувати", Location = new(700, 58), Width = 80 };
            btnDel = new() { Text = "Видалити", Location = new(700, 98), Width = 80 };

            // stats panel on right
            pnlStats = new() { Location = new(600, 200), Size = new(300, 210), BorderStyle = BorderStyle.FixedSingle };
            lblTotal = new() { Location = new(10, 10), Width = 200 };
            lblR0 = new() { Location = new(10, 40), Width = 200 };
            lblR1 = new() { Location = new(10, 70), Width = 200 };
            lblR2 = new() { Location = new(10, 100), Width = 200 };
            lblToday = new() { Location = new(10, 130), Width = 200 };
            lblAvg = new() { Location = new(10, 160), Width = 200 };

            pnlStats.Controls.AddRange(new Control[] { lblTotal, lblR0, lblR1, lblR2, lblToday, lblAvg });

            Controls.AddRange(new Control[] { dgv, txtSearch, btnAll, btnHard, btnMedium, btnEasy, cmbSort, btnAdd, btnEdit, btnDel, pnlStats });

            ClientSize = new(840, 440);
            Text = "Картки";
            StartPosition = FormStartPosition.CenterScreen;
        }
    }
}