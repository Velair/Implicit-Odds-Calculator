using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImplicitOddsCalculator.Models
{
    public class Deck
    {
        public List<Card> deck = new List<Card>();
        public static List<string> ranks = new List<string>() { "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K", "A" };
        public static List<string> suits = new List<string>() { "c", "h", "d", "s" };

        public Deck()
        {
            suits.ForEach(suit => ranks.ForEach(rank => deck.Add(new Card(rank, suit))));
        }

        public void ReturnCardsToDeck(List<Card> cardsToReturn)
        {
            if (!cardsToReturn.Any() || cardsToReturn.Any(lam => lam.cardValue.Length < 2))
            {
                cardsToReturn.Clear();
                return;
            }

            this.deck.AddRange(cardsToReturn);
            MySession.Current.FaceUpCards.RemoveAll(card => cardsToReturn.Contains(card));
            cardsToReturn.Clear();
        }

        public void TakeInputCardsFromDeck(List<Card> newCards, List<Card> listStreet)
        {
            listStreet.AddRange(newCards);
            MySession.Current.FaceUpCards.AddRange(newCards);
            MySession.Current.MainDeck.deck.RemoveAll(lam => newCards.Any(lam2 => lam2.cardValue == lam.cardValue));
            newCards.Clear();
        }

        public bool ValidateNewCards(List<Card> listNewCards, List<Card> listStreet)
        {
            return listNewCards.Any(newCard => MySession.Current.FaceUpCards.Except(listStreet).Any(dealtCard => dealtCard.cardValue==newCard.cardValue));
        }

        public void TakeRandomCardsFromDeck(int numberCards, List<Card> listStreet)
        {
            listStreet.AddRange(MySession.Current.MainDeck.deck.OrderBy(lam => Guid.NewGuid()).Take(numberCards));
            MySession.Current.FaceUpCards.AddRange(listStreet);
            MySession.Current.MainDeck.deck.RemoveAll(lam => listStreet.Contains(lam));
        }
    }
}