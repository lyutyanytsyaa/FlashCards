using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace FlashcardsApp
{
    static class Database
    {
        static string cs = "Data Source=flashcards.db";

        public static void Init()
        {
            using var con = new SqliteConnection(cs);
            con.Open();

            var cmd = con.CreateCommand();
            cmd.CommandText =
            @"
            CREATE TABLE IF NOT EXISTS Cards(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Question TEXT NOT NULL,
                Answer TEXT NOT NULL,
                Rating INTEGER NOT NULL DEFAULT 0
            );
            CREATE TABLE IF NOT EXISTS Reviews(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CardId INTEGER NOT NULL,
                ReviewedAt TEXT NOT NULL,
                Rating INTEGER NOT NULL,
                FOREIGN KEY(CardId) REFERENCES Cards(Id)
            );
            ";
            cmd.ExecuteNonQuery();
        }

        public static void AddCard(string q, string a)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO Cards (Question,Answer,Rating) VALUES (@q,@a,0)";
            cmd.Parameters.AddWithValue("@q", q);
            cmd.Parameters.AddWithValue("@a", a);
            cmd.ExecuteNonQuery();
        }

        // order e.g. "Rating DESC" ; search = text filter on Question or Answer; ratingFilter nullable 0/1/2
        public static List<Card> GetCards(string order, string search = null, int? ratingFilter = null)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            string where = "";
            if (!string.IsNullOrWhiteSpace(search))
            {
                where += " (Question LIKE @s OR Answer LIKE @s) ";
                cmd.Parameters.AddWithValue("@s", $"%{search}%");
            }
            if (ratingFilter.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(where)) where += " AND ";
                where += " Rating = @rf ";
                cmd.Parameters.AddWithValue("@rf", ratingFilter.Value);
            }

            cmd.CommandText = $"SELECT Id,Question,Answer,Rating FROM Cards" + (string.IsNullOrWhiteSpace(where) ? "" : " WHERE " + where) + $" ORDER BY {order}";
            var list = new List<Card>();
            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Card
                {
                    Id = r.GetInt32(0),
                    Question = r.GetString(1),
                    Answer = r.GetString(2),
                    Rating = r.GetInt32(3)
                });
            }
            return list;
        }

        public static void Update(Card c)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "UPDATE Cards SET Question=@q,Answer=@a,Rating=@rt WHERE Id=@i";
            cmd.Parameters.AddWithValue("@q", c.Question);
            cmd.Parameters.AddWithValue("@a", c.Answer);
            cmd.Parameters.AddWithValue("@rt", c.Rating);
            cmd.Parameters.AddWithValue("@i", c.Id);
            cmd.ExecuteNonQuery();
        }

        public static void UpdateRating(int id, int newRating)
        {
            using var con = new SqliteConnection(cs);
            con.Open();

            var cmd = con.CreateCommand();
            // update rating on card (simple approach: set to newRating)
            cmd.CommandText = "UPDATE Cards SET Rating=@r WHERE Id=@i";
            cmd.Parameters.AddWithValue("@r", newRating);
            cmd.Parameters.AddWithValue("@i", id);
            cmd.ExecuteNonQuery();

            // insert review record for today (UTC stored)
            var cmd2 = con.CreateCommand();
            cmd2.CommandText = "INSERT INTO Reviews (CardId, ReviewedAt, Rating) VALUES (@id, @dt, @r)";
            cmd2.Parameters.AddWithValue("@id", id);
            cmd2.Parameters.AddWithValue("@dt", DateTime.UtcNow.ToString("o"));
            cmd2.Parameters.AddWithValue("@r", newRating);
            cmd2.ExecuteNonQuery();
        }

        public static void Delete(int id)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "DELETE FROM Cards WHERE Id=@i";
            cmd.Parameters.AddWithValue("@i", id);
            cmd.ExecuteNonQuery();
        }

        // statistics helpers
        public static int CountAll()
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Cards";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int CountByRating(int rating)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM Cards WHERE Rating = @r";
            cmd.Parameters.AddWithValue("@r", rating);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static int CountReviewedToday()
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            // compare date part in UTC
            var today = DateTime.UtcNow.Date;
            var from = today.ToString("o");
            var to = today.AddDays(1).ToString("o");
            cmd.CommandText = "SELECT COUNT(DISTINCT CardId) FROM Reviews WHERE ReviewedAt >= @from AND ReviewedAt < @to";
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static double AverageRating()
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT AVG(Rating) FROM Cards";
            var r = cmd.ExecuteScalar();
            if (r == DBNull.Value) return 0;
            return Convert.ToDouble(r);
        }

        // helper to get random cards by logic: prefer low rating but keep randomness
        public static List<Card> GetCardsForStudy(int count = 50)
        {
            using var con = new SqliteConnection(cs);
            con.Open();
            // We will sample by weighing rating 0 heavier: weight = 3 - rating (so 0->3,1->2,2->1)
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT Id,Question,Answer,Rating FROM Cards";
            var list = new List<Card>();
            using var r = cmd.ExecuteReader();
            var rnd = new Random();
            while (r.Read())
            {
                var c = new Card
                {
                    Id = r.GetInt32(0),
                    Question = r.GetString(1),
                    Answer = r.GetString(2),
                    Rating = r.GetInt32(3)
                };
                // add multiple copies depending on weight to make easier cards appear more often
                int weight = Math.Max(1, 3 - c.Rating); // 0->3,1->2,2->1
                for (int i = 0; i < weight; i++) list.Add(c);
            }
            // shuffle and limit
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                var tmp = list[i]; list[i] = list[j]; list[j] = tmp;
            }
            if (list.Count > count) list = list.GetRange(0, count);
            return list;
        }
    }
}
