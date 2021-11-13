using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    class Row
    {
        #region Properties
        public string Name { get; set; } // Optional name
        public int Index { get; set; } // Index inside location
        public Location Location { get; set; }
        public Lot[] Lots { get; set; } 
        #endregion

        #region Constructor
        public Row(int size, int number, string name = "Unnamed row", int heigth = 0)
        {
            Name = name;
            Index = number;
            size = size < 1 ? 1 : size; //  If size is less than 1, set to 1.
        }
        #endregion

        #region Change lot data
        #region SetAllLotNames(string name) - set Name for all lots of this row
        public void SetAllLotNames(string name)
        {
            Name = name == null ? Name : name;
            for (int i = 0; i < Lots.Length; i++)
            {
                Lots[i].SetName(name);
            }
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
        #region UISetName() - Interface asks for string. Returns string or null.
        /// <returns>Interface asks for string. Returns the string, or null if user-input is blank or spaces only.</returns>
        public static string UISetName()
        {
            Console.Write("Row Name: ");
            string name = Console.ReadLine();
            name = name.Trim() == "" ? null : name;
            return name;
        }
        #endregion
        #region UISetHeigth() - Interface for setting Heigth
        /// <returns>Returns an int. Or Null if user-input is blank.</returns>
        public static int? UISetHeight()
        {
            int? heigth;
            Console.Write("Row max heigth: ");
            string heigthStr = Console.ReadLine().Trim();
            int h;
            if (heigthStr != "") // If not empty input
            {
                while (!(int.TryParse(heigthStr, out h))) // While parse fails
                {
                    Console.Write("Invalid. Try again: ");
                    heigthStr = Console.ReadLine().Trim();
                }
                heigth = h; //  On success
            }
            else //if heigth not set
            {
                heigth = null;
            }
            return heigth >= 0 ? heigth : null;
        }
        #endregion
        #region UISetHasCharger() Change the HasCharger bool
        /// <summary>
        /// Updates the HasCharger bool of the lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Does this row have a charging station?");
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
                Console.WriteLine($"Row {Index} Menu");
                Console.WriteLine("[2] Set all lot names in the row");
                Console.WriteLine("[3] Set the heigth of the row");
                Console.WriteLine("[4] Set charging stations of all lots in the row");
                Console.WriteLine("[5] Display Lots");
                Console.WriteLine("[6] Exit Row Menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    case "2":
                        {
                            string name = UISetName();
                            SetAllLotNames(name);
                            break;
                        }
                    case "3":
                        {
                            int? heigth = UISetHeight();
                            if (heigth != null)
                            {
                                SetAllLotHeigths((int)heigth);
                            }
                            break;
                        }
                    case "4":
                        {
                            UISetHasCharger();
                            break;
                        }
                    case "5":
                        {
                            Display();
                            DisplayLots();
                            break;
                        }
                    case "6":
                        {
                            Console.WriteLine("Exiting Row menu..");
                            isDone = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid!");
                            break;
                        }
                }
            }

        }
        #endregion

        #region Display() - Displays properties of row
        /// <summary>
        /// Displays relevant properties of the Row object
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"Row {Index+1}: Name: {Name}, Lots: {Lots.Length} ");
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