using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    class Row
    {
        #region Properties
        public int Index { get; set; } // Index inside location
        public Location Location { get; set; }
        public Lot[] Lots { get; set; } 
        #endregion

        #region Constructor
        public Row(int size, int number, string name = "Unnamed row", int heigth = 0)
        {
            Index = number;
        }
        #endregion

        #region SetAllLotHeigths(int heigth) - set Heigth for all lots of this row
        public void SetAllLotHeigths(int heigth)
        {
            for (int i = 0; i < Lots.Length; i++)
            {
                Lots[i].SetHeigth(heigth);
            }
        }
        #endregion
        #region SetAllLotChargers(bool hasCharger) - set HasCharger for all lots of this row
        public void SetAllLotChargers(bool hasCharger)
        {
            for (int i = 0; i < Lots.Length; i++)
            {
                Lots[i].SetHasCharger(hasCharger);
            }
        }
        #endregion
        #region UISetHeigth() - Interface for setting Heigth
        /// <summary>
        /// Ask for int, if not empty, set all lot heigths
        /// </summary>
        public void UISetHeight()
        {
            int heigth;
            Console.WriteLine("Enter för att skippa");
            Console.Write("Höjd: ");
            string heigthStr = Console.ReadLine().Trim();
            if (heigthStr != "") // If not empty input
            {
                if (int.TryParse(heigthStr, out heigth)) // While parse fails
                {
                    SetAllLotHeigths(heigth);
                }
            }
        }
        #endregion
        #region UISetHasCharger() Change the HasCharger bool
        /// <summary>
        /// Updates the HasCharger bool of all lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Har denna parkering en laddningsstation?");
            Console.Write("y/n: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y": SetAllLotChargers(true); break;
                case "n": SetAllLotChargers(false); break;
                default:  break;
            }
        }
        #endregion

        #region UIMenu()
        /// <summary>
        /// A user menu for managing this row
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                #region Menu
                Console.WriteLine($"Rad {Index + 1} Menu");
                Console.WriteLine(" 1) Visa alla platser");
                Console.WriteLine(" 2) Gå in i en parkering");
                Console.WriteLine(" 3) Sätt höjden på alla parkeringar");
                Console.WriteLine(" 4) Ändra laddningsstation på alla parkeringar");
                Console.WriteLine(" b) Backa");
                #endregion
                Console.Write("Val: ");
                switch (Console.ReadLine())
                {
                    #region Display lots
                    case "1":
                        {
                            DisplayLots();
                            Console.Write("Tryck för att fortsätta");
                            Console.ReadKey();
                            break;
                        }
                    #endregion
                    #region Enter a lot
                    case "2":
                        {
                            foreach (var lot in Lots)
                            {
                                string c = lot.HasCharger == true ? "Ja" : "Nej";
                                Console.WriteLine($"{lot.Number + 1}: Höjd {lot.Heigth}, Laddningsstation: {c} ");
                            }
                            Console.Write("Nummer: ");
                            int i = 0;
                            if (int.TryParse(Console.ReadLine(), out i))
                            {
                                if (i - 1 >= 0 && i - 1 < Lots.Length)
                                    Lots[i - 1].UIMenu();
                            }
                            break;
                        }
                    #endregion
                    #region Set height of lots
                    case "3":
                        {
                            UISetHeight();
                            break;
                        }
                    #endregion
                    #region Set HasCharger of lots
                    case "4":
                        {
                            UISetHasCharger();
                            break;
                        }
                    #endregion
                    #region Go back
                    case "b":
                        {
                            Console.WriteLine("Backar..");
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Default
                    default:
                        {
                            Console.WriteLine("Invalid!");
                            break;
                        }
                    #endregion
                }
            }

        }
        #endregion

        #region DisplayLots() - Displays properties of every lot on this Row
        /// <summary>
        /// Displays relevant properties of each lot in the Row object
        /// </summary>
        public void DisplayLots()
        {
            for (int i = 0; i < Lots.Length; i++)
            {                
                Lots[i].Display();
            }
        }
        #endregion

        #region GetAllLots()
        /// <returns>A list of all lots inside this row</returns>
        public List<Lot> GetAllLots()
        {
            List<Lot> queriedLots = new List<Lot>();
            foreach (Lot lot in Lots)
            {
                queriedLots.Add(lot);
            }
            return queriedLots;
        }
        #endregion
    }
}