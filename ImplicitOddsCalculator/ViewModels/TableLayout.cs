using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImplicitOddsCalculator.Models;

namespace ImplicitOddsCalculator.ViewModels
{
    public class TableLayout
    {
        public string imgFlop1 { get; set; }
        public string imgFlop2 { get; set; }
        public string imgFlop3 { get; set; }
        public string imgTurn { get; set; }
        public string imgRiver { get; set; }
        public string imgHand1 { get; set; }
        public string imgHand2 { get; set; }
        public string txtFlop { get; set; }
        public string txtTurn { get; set; }
        public string txtRiver { get; set; }
        public string txtHand { get; set; }
        public string disableClearHand { get; set; }
        public string disableClearFlop { get; set; }
        public string disableClearTurn { get; set; }
        public string disableClearRiver { get; set; }
        public string disableInputTurn { get; set; }
        public string disableInputRiver { get; set; }
        public string bestHand { get; set; }
        public List<KeyValuePair<string, int>> listOuts { get; set; } 
        
        

        public TableLayout()
        {
            this.imgFlop1 = this.imgFlop2 = this.imgFlop3 = this.imgTurn = this.imgRiver = this.imgHand1 = this.imgHand2 = "/Images/Deck/cardRed.png";
            this.txtFlop = this.txtHand = this.txtRiver = this.txtTurn = this.bestHand = String.Empty;
            this.disableClearFlop=this.disableClearHand=this.disableClearRiver=this.disableClearTurn=this.disableInputRiver=this.disableInputTurn= "disabled";
            this.listOuts = new List<KeyValuePair<string, int>>();
        }

        public static TableLayout BuildViewModel()
        {
            var newTable = new TableLayout();
            newTable.imgFlop1 = MySession.Current.Flop[0].Image;
            newTable.imgFlop2 = MySession.Current.Flop[1].Image;
            newTable.imgFlop3 = MySession.Current.Flop[2].Image;
            newTable.imgTurn = MySession.Current.Turn[0].Image;
            newTable.imgRiver = MySession.Current.River[0].Image;
            newTable.imgHand1 = MySession.Current.Hand[0].Image;
            newTable.imgHand2 = MySession.Current.Hand[1].Image;

            newTable.txtFlop = MySession.Current.Flop[0].cardValue + MySession.Current.Flop[1].cardValue + MySession.Current.Flop[2].cardValue;
            newTable.txtTurn = MySession.Current.Turn[0].cardValue;
            newTable.txtRiver = MySession.Current.River[0].cardValue;
            newTable.txtHand = MySession.Current.Hand[0].cardValue + MySession.Current.Hand[1].cardValue;

            Hand.SetBestHandAndOuts();
            newTable.bestHand = MySession.Current.BestHand;
            newTable.listOuts = MySession.Current.listOuts;

            EnableDisableButtons(newTable);
            
            return newTable;
        }

        private static void EnableDisableButtons(TableLayout table)
        {
            bool hasFlop = HasFlop(table);
            bool hasHand = HasHand(table);

            if (!hasFlop || !hasHand)
            {
                DisableTurn(table);
                DisableRiver(table);
            }
            else
            {
                if (!HasTurn(table))
                {
                    DisableRiver(table);
                }
                else
                {
                    HasRiver(table);
                }
            }
        }

        private static void DisableTurn(TableLayout table)
        {
            table.disableClearTurn = table.disableInputTurn = "disabled";
        }

        private static void DisableRiver(TableLayout table)
        {
            table.disableClearRiver = table.disableInputRiver = "disabled";
        }

        private static bool HasFlop(TableLayout table)
        {
            if (table.txtFlop.Length == 6)
            {
                table.disableClearFlop = String.Empty;
                return true;
            }
            table.disableClearFlop = "disabled";
            return false;
        }

        private static bool HasHand(TableLayout table)
        {
            if (table.txtHand.Length == 4)
            {
                table.disableClearHand = String.Empty;
                return true;
            }
            table.disableClearHand = "disabled";
            return false;
        }

        private static bool HasTurn(TableLayout table)
        {
            table.disableInputTurn = String.Empty;
            if (table.txtTurn.Length == 2)
            {
                table.disableClearTurn = table.disableInputRiver = String.Empty;
                table.disableClearFlop = table.disableClearHand = "disabled";
                return true;
            }
            table.disableClearTurn = "disabled";
            return false;
        }
        
        private static bool HasRiver(TableLayout table)
        {
            table.disableInputRiver = String.Empty;
            if (table.txtRiver.Length == 2)
            {
                table.disableClearRiver = String.Empty;
                table.disableClearTurn = "disabled";
                return true;
            }
            table.disableClearRiver = "disabled";
            return false;
        }
    }
}