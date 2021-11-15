
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    public class Garage
    {
        #region Properties
        public string Name { get; set; }
        public int Size { get; set; }
        public List<Location> Locations { get; set; } = new List<Location>();
        #endregion

        #region SetLotNumbers()
        public void SetLotNumbers()
        {
            int lotNumber = 0;

            for (int i = 0; i < Locations.Count; i++)
            {
                Locations[i].Index = i;
                for (int ii = 0; ii < Locations[i].Rows.Count; ii++)
                {
                    Locations[i].Rows[ii].Index = ii;
                    Locations[i].Rows[ii].LocationIndex = i;
                    for (int iii = 0; iii < Locations[i].Rows[ii].Lots.Length; iii++)
                    {
                        Locations[i].Rows[ii].Lots[iii].Index = iii;
                        Locations[i].Rows[ii].Lots[iii].RowIndex = ii;
                        Locations[i].Rows[ii].Lots[iii].LocationIndex = i;
                        Locations[i].Rows[ii].Lots[iii].Number = lotNumber;
                        lotNumber++;
                    }
                }
            }
            Size = lotNumber;
        }
        #endregion

        #region SetReferences()
        public void SetReferences()
        {
            for (int i = 0; i < Locations.Count; i++)
            {
                Locations[i].Garage = this;
                for (int ii = 0; ii < Locations[i].Rows.Count; ii++)
                {
                    Locations[i].Rows[ii].Location = Locations[i];
                    for (int iii = 0; iii < Locations[i].Rows[ii].Lots.Length; iii++)
                    {
                        Locations[i].Rows[ii].Lots[iii].Row = Locations[i].Rows[ii];
                    }
                }
            }
        }
        #endregion

        #region Constructor
        public Garage() { }
        public Garage(string name)
        {
            Name = name;
        }
        #endregion

        #region AddLocation()
        /// <summary>
        /// Adds a location object to the garage, and enters the IAddMultipleRows() of the new location for adding rows.
        /// </summary>
        public void AddLocation(string name = null)
        {
            Location location = new Location(this, name);
            Locations.Add(location);
            location.UIAddMultipleRows();
        }
        #endregion
        #region UIAddLocation()
        /// <summary>
        /// User interface ask for Name of Location, then creates new Location inside this MyGarage
        /// </summary>
        public void UIAddLocation()
        {
            string name = Location.UISetName();
            AddLocation(name);
        }
        #endregion

        #region Display()
        /// <summary>
        /// Display the entire garage
        /// </summary>
        public void Display()
        {
            Console.WriteLine("Floors/Locations: {0}", Locations.Count);

            foreach (var location in Locations)
            {
                location.Display();
                foreach (var row in location.Rows)
                {
                    row.Display();
                    row.DisplayLots();
                }
            }
        }
        #endregion
        #region DisplayLocations()
        /// <summary>
        /// Display number, name and row count of all locations
        /// </summary>
        public void DisplayLocations()
        {
            int count = 1;
            foreach (var location in Locations)
            {
                Console.WriteLine($"Location {count}: Name: {location.Name}, Row Count: {location.Rows.Count}");
                count++;
            }
        }
        #endregion

        #region SetAllLotNames(string name) - set Name prop of all Lots in all Rows in all Locations
        public void SetAllLotNames(string name)
        {

            for (int i = 0; i < Locations.Count; i++)
            {
                Location location = Locations[i];
                location.SetAllLotNames(name);
            }
            
        }
        #endregion
        #region SetAllLotHeigths(int heigth) - set Heigth prop of all Lots in all Rows in all Locations
        public void SetAllLotHeigths(int heigth)
        {
            for (int i = 0; i < Locations.Count; i++)
            {
                Location location = Locations[i];
                location.SetAllLotHeigths(heigth);
            }

        }
        #endregion
        #region SetAllLotChargers(bool hasCharger) - set HasCharger prop of all Lots in all Rows in all Locations
        public void SetAllLotChargers(bool hasCharger)
        {
            for (int i = 0; i < Locations.Count; i++)
            {
                Location location = Locations[i];
                location.SetAllLotChargers(hasCharger);
            }
            Console.WriteLine("Success");
        }
        #endregion

        #region Save(string fileName)
        /// <summary>
        /// Save the garage to a json file in /templates
        /// </summary>
        /// <param name="fileName"> The name of the file. Overrides or creates a new</param>
        public void Save(string fileName)
        {
            SetLotNumbers();
            SetReferences();
            string filePath = $"../../../templates/{fileName}.json";
            GarageSerializer garageSerializer = new GarageSerializer();
            garageSerializer.JsonSerialize(this, filePath);
            Console.WriteLine("Saved..");
        }
        #endregion


        #region UIMenu()
        /// <summary>
        /// Outer user menu for managing this garage. Add locations, step in to locations etc
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.WriteLine($"Garage {Name} Menu");
                Console.WriteLine("[1] Add a new Location");
                Console.WriteLine("[2] Edit existing Location");
                Console.WriteLine("[3] Display Garage");
                Console.WriteLine("[4] Set all lot names in the Park");
                Console.WriteLine("[5] Set the heigth of the Park");
                Console.WriteLine("[6] Set charging stations of all lots in the Park");
                Console.WriteLine("[7] Exit Garage Menu");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    #region IAddLocation();
                    case "1":
                        {
                            UIAddLocation();
                            break;
                        }
                    #endregion
                    #region UILocationMenu(int-1)
                    case "2":
                        {
                            DisplayLocations();
                            Console.Write("Enter a Location number:");
                            int loc;
                            if (int.TryParse(Console.ReadLine(), out loc))
                            {
                                loc -= 1;
                                if (loc < Locations.Count && loc >= 0)
                                {
                                    Location location = Locations[loc];
                                    UILocationMenu(location);
                                }
                                else
                                {
                                    Console.WriteLine("Out of range");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid.");
                            }
                            break;
                        }
                    #endregion
                    #region Display();
                    case "3":
                        {
                            Display();
                            break;
                        }
                    #endregion
                    #region UISetName();
                    case "4":
                        {
                            string name = UISetName();
                            SetAllLotNames(name);
                            break;
                        }
                    #endregion
                    #region UISetHeigth();
                    case "5":
                        {
                            int? heigth = UISetHeigth();
                            if (heigth != null)
                            {
                                SetAllLotHeigths((int)heigth);
                            }
                            break;
                        }
                    #endregion
                    #region UISetHasCharger();
                    case "6":
                        {
                            UISetHasCharger();
                            break;
                        }
                    #endregion
                    #region Exit
                    case "7":
                        {
                            UISave();
                            Console.WriteLine("Exiting..");
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
        #region UISave() - Serialize the garage to /templates
        public void UISave()
        {
            Console.WriteLine($"Exiting: Garage {Name} ");
            Console.WriteLine($"[1] Save as {Name}.json");
            Console.WriteLine("[2] Save as... .json");
            Console.WriteLine("[3] Don't save");
            Console.Write("Option: ");
            switch (Console.ReadLine())
            {
                #region Save
                case "1":
                    {
                        Save(Name);
                        break;
                    }
                #endregion
                #region Save as
                case "2":
                    {
                        Console.Write("File name: ");
                        Save(Console.ReadLine());
                        break;
                    }
                #endregion
                #region Don't save
                case "3":
                    {
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
        #endregion
        #region UILocationMenu
        /// <summary>
        /// User menu targeting a location in the garage. Step into location, delete location etc
        /// </summary>
        void UILocationMenu(Location location)
        {
            bool isDone = false;

            while (!isDone)
            {
                location.Display();
                Console.WriteLine("Location Menu");
                Console.WriteLine("[1] Step into");
                Console.WriteLine("[2] Remove");
                Console.WriteLine("[3] Back");
                Console.Write("Option: ");
                switch (Console.ReadLine())
                {
                    #region Location.UIMenu();
                    case "1":
                        {
                            location.UIMenu();
                            break;
                        }
                    #endregion
                    #region Locations.Remove(location);
                    case "2":
                        {
                            Console.WriteLine("Removing..");
                            Locations.Remove(location);
                            SetLotNumbers();
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Go back to IMenu()
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

        #region UISetName() Get a Name and SetAllLotNames(name);
        /// <summary>
        /// Ask user for name. Return the string on null
        /// </summary>
        public string UISetName()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine().Trim();
            name = name == "" ? null : name;
            return name;
        }
        #endregion
        #region UISetHeigth() Get the Heigth and SetAllLotHeigths(h);
        /// <summary>
        /// Ask user for input heigth. Returns int >=0 or null
        /// </summary>
        public static int? UISetHeigth()
        {
            int? heigth;
            Console.Write("Heigth: ");
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
                heigth = null;
            }
            heigth = heigth < 0 ? 0 : heigth;
            return heigth;
        }
        #endregion
        #region UISetHasCharger() Get the Heigth and SetAllLotChargers(bool);
        /// <summary>
        /// Updates the HasCharger bool of the lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Do all lots have electric chargers?");
            Console.Write("y/n: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y": SetAllLotChargers(true); ; Console.WriteLine("Set to True"); break;
                case "n": SetAllLotChargers(false); Console.WriteLine("Set to False"); break;
                default: Console.WriteLine("Didn't change"); break;
            }
        }
        #endregion

    }
}