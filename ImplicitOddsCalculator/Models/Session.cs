using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImplicitOddsCalculator.Models
{
    public class MySession
    {
        public Deck MainDeck { get; set; }
        public List<Card> Hand { get; set; }
        public List<Card> Flop { get; set; }
        public List<Card> Turn { get; set; }
        public List<Card> River { get; set; }
        public List<Card> FaceUpCards { get; set; }
        public int Outs { get; set; }
        public string BestHand { get; set; }
        public List<KeyValuePair<string, int>> listOuts { get; set; }

        private MySession()
        {
            MainDeck = new Deck();
            Hand = new List<Card>() { new Card(), new Card() };
            Flop = new List<Card>(){new Card(),new Card(),new Card()};
            Turn = new List<Card>(){new Card()};
            River = new List<Card>(){new Card()};
            FaceUpCards = new List<Card>();
            BestHand = "";
            Outs = 0;
            listOuts = new List<KeyValuePair<string, int>>();
        }

        public static MySession Current
        {
            get
            {
                MySession session = (MySession)HttpContext.Current.Session["Session"];
                if (session == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["Session"] = session;
                }
                return session;
            }
        }
    }
}