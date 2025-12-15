using System.Collections.Generic;

namespace FlashcardsApp
{
    public static class FlashcardSet
    {
        public static List<Card> Cards { get; private set; } = new();

        public static void AddCard(Card c)
        {
            Cards.Add(c);
        }
    }
}