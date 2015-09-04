using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Combinatorics.Collections;

namespace ImplicitOddsCalculator.Models
{
    public class Hand
    {
        public static void SetBestHandAndOuts()
        {
            MySession.Current.BestHand = "";
            MySession.Current.listOuts.Clear();

            if (MySession.Current.FaceUpCards.Count >= 5)
            {
                MySession.Current.BestHand = GetBestHand(MySession.Current.FaceUpCards);

                if (MySession.Current.FaceUpCards.Count < 7)
                {
                    MySession.Current.listOuts = CalculateNextHands(MySession.Current.MainDeck.deck, MySession.Current.FaceUpCards);
                    MySession.Current.listOuts.RemoveAll(lam => lam.Key == MySession.Current.BestHand);
                    MySession.Current.Outs = MySession.Current.listOuts.Sum(lam => lam.Value);
                }
            }
        }

        public static string GetBestHand(List<Card> listCards)
        {
            string bestHand="Nothing";
            int bestHandRank = 0;
            var allCombinations = new Combinations<Card>(listCards, 5);

            foreach (var hand in allCombinations)
            {
                if (IsStraightFlush(hand).Item2 > bestHandRank) { bestHand = IsStraightFlush(hand).Item1; bestHandRank = IsStraightFlush(hand).Item2; }
            }
            return bestHand;
        }

        public static List<KeyValuePair<string, int>> CalculateNextHands(List<Card> deck, List<Card> hand)
        {
            var allResults = new SortedList<string, int>();

            foreach (var item in deck)
            {
                hand.Add(item);
                string strength = GetBestHand(hand);
                if (!allResults.ContainsKey(strength)) { allResults.Add(strength, 1); }
                else { allResults[strength] += 1; }
                hand.Remove(item);
            }

            var sortedResults = allResults.OrderBy(lam => lam.Value).ToList();
            return sortedResults;
        }

        private static Tuple<string, int> IsStraightFlush(IList<Card> hand)
        {
            return IsStraight(hand).Item1 == "Straight" && IsFlush(hand).Item1 == "Flush" ? Tuple.Create("Straight Flush", 8) : IsQuads(hand);
        }

        private static Tuple<string, int> IsQuads(IList<Card> hand)
        {
            var ranks = Card.GetRanks((List<Card>)hand);
            bool isQuads = false;

            foreach (var item in ranks)
            {
                if (ranks.Count(lam => lam == item) == 4) { isQuads = true; break; };
                if (ranks.IndexOf(item) == 1) { break; }
            }
            return isQuads ? Tuple.Create("Quads", 7) : IsFullHouse(hand);
        }

        private static Tuple<string, int> IsFullHouse(IList<Card> hand)
        {
            return Card.GetRanks((List<Card>)hand).Distinct().Count() == 2 ? Tuple.Create("Full House", 6) : IsFlush(hand);
        }

        private static Tuple<string, int> IsFlush(IList<Card> hand)
        {
            return Card.GetSuits((List<Card>)hand).Distinct().Count() == 1 ? Tuple.Create("Flush", 5) : IsStraight(hand);
        }

        private static Tuple<string, int> IsStraight(IList<Card> hand)
        {
            var ranksInt = Card.GetBroadways((List<Card>)hand).Select(int.Parse).ToList();
            var wheel = new List<int> { 14, 2, 3, 4, 5 };

            return (ranksInt.Max() - ranksInt.Min() == 4 && ranksInt.Distinct().Count() == 5) || !wheel.Except(ranksInt).Any() ? Tuple.Create("Straight", 4) : IsSet(hand);
        }

        private static Tuple<string, int> IsSet(IList<Card> hand)
        {
            var ranks = Card.GetRanks((List<Card>)hand);
            bool isSet = false;

            foreach (var item in ranks)
            {
                if (ranks.Count(lam => lam == item) == 3) { isSet = true; break; };
                if (ranks.IndexOf(item) == 2) { break; }
            }
            return isSet ? Tuple.Create("Set", 3) : IsDoublePair(hand);
        }

        private static Tuple<string, int> IsDoublePair(IList<Card> hand)
        {
            return Card.GetRanks((List<Card>)hand).Distinct().Count() == 3 ? Tuple.Create("Double Pair", 2) : IsPair(hand);
        }

        private static Tuple<string, int> IsPair(IList<Card> hand)
        {
            return Card.GetRanks((List<Card>)hand).Distinct().Count() == 4 ? Tuple.Create("Pair", 1) : Tuple.Create("Nothing", 0);
        }
    }
}