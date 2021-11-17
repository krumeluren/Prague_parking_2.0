using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0.Garage
{
    [Serializable]
    class Location
    {
        #region Properties
        public string Name { get; set; } // Optional name
        public int Index { get; set; } // Index inside garage
        public Garage Garage { get; set; }
        public List<Row> Rows { get; set; }
        #endregion
        #region Constructor
        public Location(int number, string name = "Unnamed location")
        {
            Name = name;
        }
        #endregion

        // Functions
        #region SetAllLotHeigths(int heigth) - set Heigth prop of all Lots in all Rows of this Location
        public void SetAllLotHeigths(int heigth)
        {
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                row.SetAllLotHeigths(heigth);
            }
        }
        #endregion
        #region SetAllLotChargers(bool hasCharger) - set HasCharger prop of all Lots in all Rows of this Location
        public void SetAllLotChargers(bool hasCharger)
        {
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                row.SetAllLotChargers(hasCharger);
            }
        }
        #endregion
        #region GetAllLots()
        public List<Lot> GetAllLots()
        {
            List<Lot> lots = new List<Lot>();

            foreach (Row row in Rows)
            {
                foreach (Lot lot in row.Lots)
                {
                    lots.Add(lot);
                }
            }
            return lots;
        }
        #endregion
        #region LotCount()
        /// <returns>Lot count of this Location</returns>
        public int LotCount()
        {
            int c = 0;
            for (int i = 0; i < Rows.Count; i++)
            {
                c += Rows[i].Lots.Length;
            }
            return c;
        }
        #endregion

        //  Displays
        #region GetName()
        /// <returns>Name, or index if null</returns>
        public string GetName()
        {
            return Name == null ? $"Område: {Index}, " : $"{Name}";
        }
        #endregion
        #region Display()
        /// <summary>
        ///  Displays location status
        /// </summary>
        public void Display()
        {
            string locName = GetName();
            int freeLot = 0;
            int occupiedLot = 0;
            int partOccupiedLot = 0;
            foreach (var row in Rows)
            {
                foreach (var lot in row.Lots)
                {
                    if (lot.SpaceLeft == lot.Space)
                        freeLot++;
                    else if (lot.SpaceLeft == 0)
                        occupiedLot++;
                    else if (lot.SpaceLeft != 0 && lot.SpaceLeft != lot.Space)
                        partOccupiedLot++;
                }
            }
            Console.WriteLine($"{locName}, Lediga platser: {freeLot}, Upptagna platser: {occupiedLot}, Delvis upptagna platser {partOccupiedLot}.");
        }
        #endregion
        #region DisplayLots()
        /// <summary>
        /// Display all lots
        /// </summary>
        public void DisplayLots()
        {
            foreach (Row row in Rows)
            {
                row.DisplayLots();
            }
        }
        #endregion

        //  User interfaces
        #region UIMenu()
        /// <summary>
        /// Menu for this managing rows inside this location
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                DisplayLots();
                Console.WriteLine(" ");
                Display();
                Console.WriteLine(" ");
                #region Menu
                Console.WriteLine($"{GetName()} Meny");
                Console.WriteLine(" 1) Visa alla platser");
                Console.WriteLine(" 2) Gå in i en rad");
                Console.WriteLine(" 3) Byt namn");
                Console.WriteLine(" 4) Sätt höjden på alla parkeringar");
                Console.WriteLine(" 5) Ändra laddningsstation på alla parkeringar");
                Console.WriteLine(" b) Backa");
                #endregion
                Console.Write("Val: ");
                switch (Console.ReadLine())
                {
                    #region Display lots
                    case "1":
                        {
                            Display();
                            break;
                        }
                    #endregion
                    #region Enter a row
                    case "2":
                        {
                            Console.Clear();
                            foreach (var row in Rows)
                            {
                                Console.WriteLine($"{row.Index + 1}: Antal platser: {row.Lots.Length}");
                            }
                            Console.Write("Nummer: ");
                            int i = 0;
                            if (int.TryParse(Console.ReadLine(), out i))
                            {
                                if (i - 1 >= 0 && i - 1 < Rows.Count)
                                    Rows[i - 1].UIMenu();
                            }
                            break;
                        }
                    #endregion
                    #region Set the heigth of the location
                    case "3":
                        {
                            UISetName();
                            break;
                        }
                    #endregion
                    #region Set Height of all lots
                    case "4":
                        {
                            UISetHeigth();
                            break;
                        }
                    #endregion
                    #region Set charging stations of all lots in the Park
                    case "5":
                        {
                            UISetHasCharger();
                            break;
                        }
                    #endregion
                    #region Back
                    case "b":
                        {
                            Console.WriteLine("Backar");
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Default, error
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
        #region UISetName()
        /// <summary>
        /// Ask user for a name of this location
        /// </summary>
        public void UISetName()
        {
            Console.WriteLine("Enter för att skippa");
            Console.Write("Namn: ");
            string name = Console.ReadLine().Trim();
            Name = name == null ? Name : name;
        }
        #endregion
        #region UISetHeigth()
        /// <summary>
        /// Ask for user input, if not empty, set height of all lots
        /// </summary>
        public void UISetHeigth()
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
        /// Updates the HasCharger bool of all lots
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Sätt laddningsstation för alla parkeringar. Enter för att backa.");
            Console.Write("y/n: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y": SetAllLotChargers(true); Console.WriteLine("Set to True"); break;
                case "n": SetAllLotChargers(false); Console.WriteLine("Set to False"); break;
                default: Console.WriteLine("Didn't change"); break;
            }
        }
        #endregion
    }
}
