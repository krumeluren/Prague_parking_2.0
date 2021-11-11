using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    class Location
    {
        #region Properties
        public string Name { get; set; }
        public int Number { get; set; }
        public List<Row> Rows { get; set; }
        #endregion

        #region Constructor
        public Location(int number, string name = "Unnamed location")
        {
            Number = number;
            Name = name;
            Rows = new List<Row>();
        }
        #endregion

        #region Unused methods
        #region SetAllLotNames(string name) - set Name prop of all Lots in all Rows of this Location
        public void SetAllLotNames(string name)
        {
            Name = name == null ? Name : name;
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                row.SetAllLotNames(name);
            }
        }
        #endregion
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
        #region UITargetRow() - User input. Return a number or null
        public int? UITargetRow()
        {
            Display();
            DisplayRows();
            Console.Write("Enter a Row number:");
            int i;
            if (int.TryParse(Console.ReadLine(), out i))
            {
                i -= 1;
                if (i < Rows.Count && i >= 0)
                {
                    return i;
                }
                else
                {
                    Console.WriteLine("Out of range");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Invalid.");
                return null;
            }
        }
        #endregion
        #region UISetName()
        /// <summary>
        /// Ask user for a name
        /// </summary>
        /// <returns>A name or null</returns>
        public static string UISetName()
        {
            Console.Write("Location Name: ");
            string name = Console.ReadLine().Trim();
            return name == "" ? null : name;
        }
        #endregion
        #region UISetHeigth()
        public static int? UISetHeigth()
        {
            int? heigth;
            Console.Write("Location Max Heigth: ");
            string heigthStr = Console.ReadLine().Trim();
            int h;
            if (heigthStr != "") // If not empty input
            {
                while (!(int.TryParse(heigthStr, out h))) // While parse fails
                {
                    Console.Write("Invalid. Try again: ");
                    heigthStr = Console.ReadLine().Trim();
                }
                Console.WriteLine("");
                heigth = h; //  On success
            }
            else //if heigth not set
            {
                heigth = 0;
            }
            heigth = heigth >= 0 ? heigth : null;
            return heigth;
        }
        #endregion
        #region UISetHasCharger() Change the HasCharger bool
        /// <summary>
        /// Updates the HasCharger bool of the lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Does this location have a charging station?");
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
        #region UIMenu()
        /// <summary>
        /// Menu for this managing rows inside this location
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Display();
                Console.WriteLine("Location Menu");
                
                Console.WriteLine("[4] Display Rows");
                Console.WriteLine("[5] Set all lot names in the Location");
                Console.WriteLine("[6] Set the heigth of the location");
                Console.WriteLine("[7] Set charging stations of all lots in the location");
                Console.WriteLine("[8] Exit to Garage Menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    #region Display rows of location
                    case "4":
                        {
                            Display();
                            DisplayRows();
                            break;
                        }
                    #endregion
                    #region Set name of all lots in this location
                    case "5":
                        {
                            string name = UISetName();
                            SetAllLotNames(name);
                            break;
                        }
                    #endregion
                    #region Set the heigth of the location
                    case "6":
                        {
                            int? heigth = UISetHeigth();
                            if (heigth != null)
                            {
                                SetAllLotHeigths((int)heigth);
                            }

                            break;
                        }
                    #endregion
                    #region Set charging stations of all lots in the Park
                    case "7":
                        {
                            UISetHasCharger();
                            break;
                        }
                    #endregion
                    #region Go back to Location.ILocationMenu()
                    case "8":
                        {
                            Console.WriteLine("Exiting menu..");
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
        #endregion

        #region Display()
        /// <summary>
        ///  Displays relevant properties of the Location.
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"Location {Number}: Name: {Name}, Row Count: {Rows.Count}");
        }
        #endregion
        #region DisplayRows()
        /// <summary>
        /// Run Display() for each row in this Location object
        /// </summary>
        public void DisplayRows()
        {
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                row.Display();
            }
        }
        #endregion

        #region GetAllLots()
        public List<Lot> GetAllLots()
        {
            List<Lot> lots = new List<Lot>();

            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                foreach (Lot lot in row.GetAllLots())
                {
                    lots.Add(lot);
                }
            }
            return lots;
        }
        #endregion
    }
}
