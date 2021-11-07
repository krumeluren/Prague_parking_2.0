using System;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    class Row
    {
        #region Properties
        public Location Location { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public Lot[] Lots { get; set; }
        #endregion

        #region Constructor(size, name = "Unnamed row", heigth = null)
        public Row(int size, int number, string name = "Unnamed row", int? heigth = null)
        {
            Name = name;
            Number = number;
            size = size < 1 ? 1 : size; //  If size is less than 1, set to 1.
            Lots = new Lot[size];
            for (int i = 0; i < size; i++)
            {
                Lots[i] = new Lot(name, heigth);
            }
        }
        #endregion

        #region SetAllLotNames(string name) - set Name for all lots of this row
        public void SetAllLotNames(string name)
        {
            Name = name == null ? Name = Name : Name = name;
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

        //  Edit lot data individually
        #region UIEditLots() - User interface for editing data of lots
        /// <summary>
        /// User menu for editing lots inside of row
        /// </summary>
        public void UIEditLots()
        {
            bool isDone = false;
            while (!isDone)
            {
                Display();
                DisplayLots();

                Console.WriteLine("[1] Edit specific Lot");
                Console.WriteLine("[2] Edit all Lots");
                Console.WriteLine("[3] Exit menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    #region EditSpecificLot()
                    case "1":
                        {
                            DisplayLots();
                            EditSpecificLot();
                            break;
                        }
                    #endregion
                    #region EditAllLots()
                    case "2":
                        {
                            EditAllLots();
                            break;
                        }
                    #endregion
                    #region Exit to IMenu()
                    case "3":
                        {
                            isDone = true; Console.WriteLine("Backing..");
                            break;
                        }
                    #endregion
                    #region Default
                    default:
                       
                        {
                            Console.WriteLine("Invalid");
                            break;
                        }
                    #endregion
                }
            }
        }
        #endregion
        #region EditSpecificLot()
        /// <summary>
        /// Runs the UI's for a specific lot in this row
        /// </summary>
        public void EditSpecificLot()
        {
            Console.Write("Enter a target: ");
            int i;
            if (int.TryParse(Console.ReadLine(), out i))
            {
                if (i > 0 && i <= (Lots.Length))
                {
                    Lot lot = Lots[i - 1];
                    lot.UISetName();
                    lot.UISetHeigth();
                    lot.UISetHasCharger();
                }
            }
            else
            {
                Console.WriteLine("Invalid");
            }
        }
        #endregion
        #region EditAllLots()
        /// <summary>
        /// Runs the UI's for updating props on all lots on this row
        /// </summary>
        public void EditAllLots()
        {
            for (int i = 0; i < this.Lots.Length; i++)
            {
                Lot lot = Lots[i];
                Console.Write((i + 1) + ": ");
                lot.Display();
                lot.UISetName();
                lot.UISetHeigth();
                lot.UISetHasCharger();
            }
        }
        #endregion

        #region Display() - Displays properties of row for user
        /// <summary>
        /// Displays relevant properties of the Row object
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"Row {Number}: Name: {Name}, Lots: {Lots.Length} ");
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
                Console.Write( "Lot: " + (i + 1) + ": ");
                Lots[i].Display();
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
                Console.WriteLine($"Row {Number} Menu");
                Console.WriteLine("[1] Edit Lots");
                Console.WriteLine("[2] Set all lot names in the row");
                Console.WriteLine("[3] Set the heigth of the row");
                Console.WriteLine("[4] Set charging stations of all lots in the row");
                Console.WriteLine("[5] Display Lots");
                Console.WriteLine("[6] Exit Row Menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            UIEditLots();
                            break;
                        }
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

        //  User Interface statics
        #region UISetName() - Interface asks for string. Returns string or null.
        /// <returns>Interface asks for string. Returns the string, or null if user-input is blank or spaces only.</returns>
        public static string UISetName()
        {
            Console.Write("Row Name: ");
            string name = Console.ReadLine();
            name = name.Trim() == "" ? name = null : name = name;
            return name;
        }
        #endregion

        #region UISetSize() - Interface asks for int of >= 1. Loops
        /// <returns>An int. Has to be than atleast 1 or it loops</returns>
        public static int UISetSize()
        {
            Console.Write("Lot count: ");
            int size;
            while (!(int.TryParse(Console.ReadLine(), out size)) || size < 1) // While parse fails or size is smaller than minumum
            {
                Console.Write(size + " is invalid. Try again: ");
            }
            return size;
        }
        #endregion

        #region UISetHeigth() - Interface for setting Heigth
        /// <returns>Returns an int?. Null if user-input is blank, only spaces or less than 0</returns>
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
            heigth = heigth >= 0 ? heigth = heigth : heigth = null;
            return heigth;
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
    }
}