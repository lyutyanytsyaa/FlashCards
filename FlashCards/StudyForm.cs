using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FlashcardsApp;

    public partial class StudyForm : Form
    {
        List<Card> cards;
        Card current;
        Random rnd = new Random();
        bool answerVisible = false;

        public StudyForm()
        {
            InitializeComponent();
            
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(400, 150);
            
            // Беремо всі картки
            var all = Database.GetCards("Rating ASC");

            // Якщо карток нема
            if (all.Count == 0)
            {
                MessageBox.Show("Немає карток!");
                Close();
                return;
            }

            // Створюємо список з вагами
            cards = CreateWeightedList(all);

            Next();
        }

        //-------------------------------------------------------------
        // Логіка вагового списку (більше шансів для поганого рейтингу)
        //-------------------------------------------------------------
        List<Card> CreateWeightedList(List<Card> src)
        {
            List<Card> w = new List<Card>();

            foreach (var c in src)
            {
                int weight = 1;

                if (c.Rating == 0) weight = 6;  // дуже часто
                if (c.Rating == 1) weight = 3;  // середньо
                if (c.Rating == 2) weight = 1;  // рідко

                for (int i = 0; i < weight; i++)
                    w.Add(c);
            }
            return w;
        }

        //-------------------------------------------------------------
        // Вибір наступної картки
        //-------------------------------------------------------------
        void Next()
        {
            answerVisible = false;

            // Вибрати випадкову картку по її вазі
            current = cards[rnd.Next(cards.Count)];

            lblQ.Text = current.Question;
            lblA.Text = "*****";
        }

        //-------------------------------------------------------------
        // Оцінити та оновити ваги
        //-------------------------------------------------------------
        void Rate(int r)
        {
            Database.UpdateRating(current.Id, r);

            // Перегенерувати список ваг
            var all = Database.GetCards("Rating ASC");
            cards = CreateWeightedList(all);

            Next();
        }

        //-------------------------------------------------------------
        // Кнопки
        //-------------------------------------------------------------
        private void show_Click(object sender, EventArgs e)
        {
            if (!answerVisible)
            {
                lblA.Text = current.Answer;
                answerVisible = true;
            }
            else
            {
                lblA.Text = "*****";
                answerVisible = false;
            }
        }

        private void know_Click(object sender, EventArgs e) => Rate(2);
        private void so_Click(object sender, EventArgs e) => Rate(1);
        private void dont_Click(object sender, EventArgs e) => Rate(0);

        //-------------------------------------------------------------
        // Гарячі клавіші 0–3
        //-------------------------------------------------------------
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    show.PerformClick();
                    return true;

                case Keys.D1:
                case Keys.NumPad1:
                    dont.PerformClick();
                    return true;

                case Keys.D2:
                case Keys.NumPad2:
                    so.PerformClick();
                    return true;

                case Keys.D3:
                case Keys.NumPad3:
                    know.PerformClick();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
