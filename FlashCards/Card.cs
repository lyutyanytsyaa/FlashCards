namespace FlashcardsApp;

public class Card
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int Rating { get; set; } = 0; 

    public Card(string q, string a)
    {
        Question = q;
        Answer = a;
    }

    public Card() { }
}
