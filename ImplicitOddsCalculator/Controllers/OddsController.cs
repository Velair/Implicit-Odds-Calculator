using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImplicitOddsCalculator.ViewModels;
using ImplicitOddsCalculator.Models;
using Pulzonic.Multipartial;

namespace ImplicitOddsCalculator.Controllers
{
    public class OddsController : Controller
    {
        // GET: Odds
        public ActionResult Index()
        {
            var mainTable = new TableLayout();
            return View("Odds", mainTable);
        }

        [HttpPost]
        public ActionResult MultipleCommand(string Command, string street, TableLayout mainTable)
        {
            switch (Command)
            {
                case "Select":
                    return SelectCards(street, mainTable);
                case "Random":
                    return RandomCards(street);
                case "Clear":
                    return ClearCards(street);
                default:
                    return PartialView("DeckSection", TableLayout.BuildViewModel());
            }
        }

        private ActionResult SelectCards(string street, TableLayout mainTable)
        {
            switch (street)
            {
                case "H":
                    Operations.SelectNewCards(mainTable.txtHand, MySession.Current.Hand);
                    break;

                case "F":
                    Operations.SelectNewCards(mainTable.txtFlop, MySession.Current.Flop);
                    break;

                case "T":
                    Operations.SelectNewCards(mainTable.txtTurn, MySession.Current.Turn);
                    break;

                case "R":
                    Operations.SelectNewCards(mainTable.txtRiver, MySession.Current.River);
                    break;

                default:
                    break;
            }

            return ReturnMultipartial(new MultipartialResult(this));
        }

        private ActionResult RandomCards(string street)
        {
            switch (street)
            {
                case "H":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Hand);
                    MySession.Current.MainDeck.TakeRandomCardsFromDeck(2, MySession.Current.Hand);
                    break;
                case "F":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Flop);
                    MySession.Current.MainDeck.TakeRandomCardsFromDeck(3, MySession.Current.Flop);
                    break;
                case "T":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Turn);
                    MySession.Current.MainDeck.TakeRandomCardsFromDeck(1, MySession.Current.Turn);
                    break;
                case "R":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.River);
                    MySession.Current.MainDeck.TakeRandomCardsFromDeck(1, MySession.Current.River);
                    break;
                default:
                    break;
            }

            return ReturnMultipartial(new MultipartialResult(this));
        }

        private ActionResult ClearCards(string street)
        {
            switch (street)
            {
                case "H":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Hand);
                    MySession.Current.Hand = new List<Card>() { new Card(), new Card() };
                    break;
                case "F":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Flop);
                    MySession.Current.Flop = new List<Card>() { new Card(), new Card(), new Card() };
                    break;
                case "T":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.Turn);
                    MySession.Current.Turn = new List<Card>() { new Card() };
                    break;
                case "R":
                    MySession.Current.MainDeck.ReturnCardsToDeck(MySession.Current.River);
                    MySession.Current.River = new List<Card>() { new Card() };
                    break;
                default:
                    break;
            }

            return ReturnMultipartial(new MultipartialResult(this));
        }

        private static ActionResult ReturnMultipartial(MultipartialResult multiPartial)
        {
            TableLayout newTableLayout = TableLayout.BuildViewModel();
            multiPartial.AddView("OutsSection", "OutsSection", newTableLayout);
            multiPartial.AddView("DeckSection", "DeckSection", newTableLayout);
            return multiPartial;
        }
    }
}