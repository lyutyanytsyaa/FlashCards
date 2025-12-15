using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FlashcardsApp
{
    public partial class CardsForm : Form
    {
        int? currentFilterRating = null;
        string currentSearch = "";
        string currentOrder = "Rating DESC";

        public CardsForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            Location = new Point(200, 150); // фіксоване місце при відкритті (можна переміщати вручну)

            dgv.AutoGenerateColumns = false;
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Слово",
                DataPropertyName = "Question",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Переклад",
                DataPropertyName = "Answer",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            // кнопки вгорі
            btnAll.Click += (s, e) => { currentFilterRating = null; RefreshList(); };
            btnHard.Click += (s, e) => { currentFilterRating = 0; RefreshList(); };
            btnMedium.Click += (s, e) => { currentFilterRating = 1; RefreshList(); };
            btnEasy.Click += (s, e) => { currentFilterRating = 2; RefreshList(); };

            txtSearch.TextChanged += (s, e) => { currentSearch = txtSearch.Text.Trim(); RefreshList(); };

            cmbSort.SelectedIndexChanged += (s, e) =>
            {
                if (cmbSort.Text == "Rating") currentOrder = "Rating DESC";
                else if (cmbSort.Text == "Word") currentOrder = "Question ASC";
                else currentOrder = "Answer ASC";
                RefreshList();
            };

            btnAdd.Click += add_Click;
            btnDel.Click += del_Click;
            btnEdit.Click += edit_Click;

            RefreshList();
        }

        void RefreshList()
        {
            dgv.DataSource = Database.GetCards(currentOrder, currentSearch, currentFilterRating);
            UpdateRowColors();
            UpdateStatsPanel();
        }

        private void UpdateRowColors()
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.DataBoundItem is Card c)
                {
                    switch (c.Rating)
                    {
                        case 0: row.DefaultCellStyle.BackColor = Color.LightCoral; break;
                        case 1: row.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow; break;
                        case 2: row.DefaultCellStyle.BackColor = Color.LightGreen; break;
                        default: row.DefaultCellStyle.BackColor = Color.White; break;
                    }
                }
            }
        }

        private void UpdateStatsPanel()
        {
            lblTotal.Text = $"Загальна кількість карток: {Database.CountAll()}";
            lblR0.Text = $"К-ть слів не знаю: {Database.CountByRating(0)}";
            lblR1.Text = $"К-ть слів так собі: {Database.CountByRating(1)}";
            lblR2.Text = $"К-ть слів знаю: {Database.CountByRating(2)}";
            lblToday.Text = $"Переглянуто сьогодні: {Database.CountReviewedToday()}";
            lblAvg.Text = $"Середній рейтинг слів: {Database.AverageRating():0.00}";
        }

        void Load(string o) => RefreshList();

        private void add_Click(object s, EventArgs e)
        {
            new AddCardForm().ShowDialog();
            RefreshList();
        }

        private void del_Click(object s, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Card c)
            {
                if (MessageBox.Show("Delete this card?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Database.Delete(c.Id);
                    RefreshList();
                }
            }
        }

        private void edit_Click(object s, EventArgs e)
        {
            if (dgv.CurrentRow?.DataBoundItem is Card c)
            {
                new EditCardForm(c).ShowDialog();
                RefreshList();
            }
        }
    }
}
