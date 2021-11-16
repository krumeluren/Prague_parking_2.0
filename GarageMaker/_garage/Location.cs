using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    public class Location
    {
        #region Properties
        public string Name { get; set; }
        public int Index { get; set; }
        public Garage Garage { get; set; }
        public List<Row> Rows { get; set; }

        #endregion

        #region Constructor
        public Location() { }
        public Location(Garage garage, string name = "Unnamed location")
        {
            Garage = garage;
            Name = name;
            Rows = new List<Row>();
        }
        #endregion

        #region AddRow() - Add a new Row() object to List<Row> Structure property
        public void AddRow(int size, int heigth)
        {
            Rows.Add(new Row(size, heigth ));
            Garage.SetLotNumbers();
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

        #region UIAddRow() - Interface get user input and run AddRow()
        /// <summary>
        /// Add a single row to location
        /// </summary>
        public void UIAddRow()
        {
            Console.WriteLine("How many lots are there on this row?");
            int size = Row.UISetSize();
            Console.WriteLine("Enter the max heigth of this row, or skip");
            int heigth = Row.UISetHeight();
            AddRow(size, heigth);
        }
        #endregion

        #region UIAddMultipleRows() - Interface get user input and run AddRow() for each
        /// <summary>
        /// UI for adding rows to location.
        /// Either run AddRow() x times with the user input data, or run IAddRow() asking for new user input data each time
        /// </summary>
        public void UIAddMultipleRows()
        {
            //  User select row count
            Console.WriteLine("How many rows do you want to add?");
            Console.Write("Amount: ");
            int amount;
            while (!int.TryParse(Console.ReadLine(), out amount))
            {
                Console.Write("Invalid. Try again: ");
            }
            //  Interface
            Console.Clear();
            Console.WriteLine("[1] Enter parameters for all");
            Console.WriteLine("[2] Enter individually");
            Console.WriteLine("[x] Go back");
            Console.Write("Option: ");
            switch (Console.ReadLine())
            {
                #region Enter for all one time
                case "1":
                    {
                        Console.Clear();
                        Console.WriteLine("How many lots are there on the rows?");
                        int size = Row.UISetSize();
                        Console.Clear();
                        Console.WriteLine("Enter the max heigth of the rows, or skip");
                        int heigth = Row.UISetHeight();
                        for (int i = 0; i < amount; i++)
                        {
                            AddRow(size, heigth);
                        }
                        break;
                    }
                #endregion
                #region Enter for each
                case "2":
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            Console.Clear();
                            Console.WriteLine($"Entering for Row {i+1}");
                            UIAddRow();
                        }
                        break;
                    }
                #endregion
                default: Console.WriteLine("Backing.."); return;
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
        public static int UISetHeigth()
        {
            int heigth;
            Console.Write("Location Max Heigth: ");
            string heigthStr = Console.ReadLine().Trim();
            if (heigthStr != "") // If not empty input
            {
                while (!(int.TryParse(heigthStr, out heigth))) // While parse fails
                {
                    Console.Write("Invalid. Try again: ");
                    heigthStr = Console.ReadLine().Replace(" ", "");
                }
                return heigth;
            }
            else //if heigth not set
            {
                return -1;
            }
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

        #region Display()
        /// <summary>
        ///  Displays relevant properties of the Location.
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"Location {Index+1}: Name: {Name}, Row Count: {Rows.Count}");
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
                Display();
                Console.WriteLine("Location Menu");
                Console.WriteLine("[1] Add a new Row");
                Console.WriteLine("[2] Add multiple Rows");
                Console.WriteLine("[3] Enter row");
                Console.WriteLine("[4] Display Rows");
                Console.WriteLine("[5] Set Name");
                Console.WriteLine("[6] Set the heigth of all lots");
                Console.WriteLine("[7] Set charging stations of all lots");
                Console.WriteLine("[8] Exit to Garage Menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    #region Add one Row. Enter IAddRow()
                    case "1":
                        {
                            Console.Clear();
                            UIAddRow();
                            break;
                        }
                    #endregion
                    #region Add multiple Rows. Enter IAddMultipleRows()
                    case "2":
                        {
                            Console.Clear();
                            UIAddMultipleRows();
                            break;
                        }
                    #endregion
                    #region Enter Row
                    case "3":
                        {
                            Console.Clear();
                            int? row = UITargetRow();
                            if(row != null)
                            {
                                UIRowMenu(Rows[(int)row]);
                            }
                            break;
                        }
                    #endregion
                    #region Display rows of location
                    case "4":
                        {
                            Console.Clear();
                            Display();
                            DisplayRows();
                            Console.WriteLine("Continue");
                            Console.ReadKey();
                            break;
                        }
                    #endregion
                    #region Set Name
                    case "5":
                        {
                            Console.Clear();
                            string name = UISetName();
                            if (name != null)
                            Name = name;
                            break;
                        }
                    #endregion
                    #region Set the heigth of the location
                    case "6":
                        {
                            int heigth = UISetHeigth();
                            if(heigth != -1)
                            {
                                SetAllLotHeigths(heigth);
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

        #region UIRowMenu()
        /// <summary>
        /// Menu targeting a row inside this location. Step Into, Remove etc
        /// </summary>
        private void UIRowMenu(Row row)
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Console.WriteLine("Row Menu");
                Console.WriteLine("[1] Step into");
                Console.WriteLine("[2] Remove");
                Console.WriteLine("[3] Back");
                switch (Console.ReadLine())
                {
                    #region Step into
                    case "1":
                        {
                            row.UIMenu();
                            break;
                        }
                    #endregion
                    #region Remove
                    case "2":
                        {
                            Rows.Remove(row);
                            Garage.SetLotNumbers();
                            break;
                        }
                    #endregion
                    #region Go back
                    case "3":
                        {
                            Console.WriteLine("Backing..");
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
    }
}
