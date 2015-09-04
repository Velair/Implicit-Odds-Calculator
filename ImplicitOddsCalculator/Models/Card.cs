using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImplicitOddsCalculator.Models
{
    public class Card
    {
        public static Dictionary<string, string> broadways = new Dictionary<string, string> { { "T", "10" }, { "J", "11" }, { "Q", "12" }, { "K", "13" }, { "A", "14" } };
        public string cardValue { get; set; }
        public string cardRank { get; set; }
        public string cardSuit { get; set; }
        public string Image { get; set; }

        public Card(string rank, string suit)
        {
            cardRank = rank;
            cardSuit = suit;
            cardValue = rank + suit;
            Image = "/Images/Deck/" + cardValue + ".png";
            if (cardValue == "Ad") { Image = "/Images/Deck/dA.png"; }
        }

        public Card()
        {
            cardRank = cardSuit = cardValue = String.Empty;
            Image = "/Images/Deck/cardRed.png";
        }

        public static List<Card> CreateCardsFromString(string txtCards)
        {
            List<string> listCardsValues = new List<string>(); 
            for (int i = 0; i < txtCards.Length; i+=2)
            {
                string newCard = Char.ToUpper(txtCards[i]).ToString() + Char.ToLower(txtCards[i+1]).ToString();
                listCardsValues.Add(newCard);
            }
            return new List<Card>(listCardsValues.Distinct().Select(lam => new Card(lam[0].ToString(), lam[1].ToString())).ToList());
        }

        public static List<string> GetRanks(List<Card> listCards)
        {
            var ranks = listCards.Select(lam => lam.cardRank).ToList();
            return ranks;
        }

        public static List<string> GetSuits(List<Card> listCards)
        {
            var suits = listCards.Select(lam => lam.cardSuit).ToList();
            return suits;
        }

        public static List<string> GetBroadways(List<Card> listCards)
        {
            var ranks = GetRanks(listCards);

            foreach (var item in broadways)
            {
                while (ranks.Contains(item.Key))
                {
                    ranks[ranks.FindIndex(lam => lam == item.Key)] = item.Value;
                }
            }
            return ranks;
        }
    }
}