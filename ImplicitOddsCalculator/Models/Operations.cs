using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using ImplicitOddsCalculator.ViewModels;

namespace ImplicitOddsCalculator.Models
{
    public class Operations
    {
        public static bool ValidateInput(string input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
            return Regex.IsMatch(input, @"^(([atjqk]|[2-9])[sdch])+$", RegexOptions.IgnoreCase);    
            }
            return false;
        }

        public static void SelectNewCards(string txtNewCards, List<Card> listCards)
        {
            // Todo Validate Syntax
            if (Operations.ValidateInput(txtNewCards))
            {
                // Turn string to Cards
                List<Card> listCardsToTake = Card.CreateCardsFromString(txtNewCards);

                // Validate Cards are not repeated
                if (listCardsToTake.Count == (txtNewCards.Length / 2) && !MySession.Current.MainDeck.ValidateNewCards(listCardsToTake, listCards))
                {
                    // Return actual cards to deck
                    MySession.Current.MainDeck.ReturnCardsToDeck(listCards);
                    // Select input cards from deck
                    MySession.Current.MainDeck.TakeInputCardsFromDeck(listCardsToTake, listCards);
                }
            }
        }
    }
}