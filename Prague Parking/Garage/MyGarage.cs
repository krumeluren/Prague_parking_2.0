
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    class MyGarage
    {
        #region Properties
        public string Name { get; set; } // Name of garage
        public string FileName { get; set; } // The name of the file loaded (initially same as Name)
        public List<Location> Locations { get; set; }
        public int Size { get; set; } // lot count in garage
        #endregion
        #region Constructor
        public MyGarage(string name)
        {
            Name = name;
            Locations = new List<Location>();
        }
        #endregion

        #region Unused methods
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

        #region Save(string fileName)
        /// <summary>
        /// Save the garage to a json file in /parks
        /// </summary>
        /// <param name="fileName"> The name of the file to save as</param>
        public void Save(string fileName)
        {
            string filePath = $"../../../parks/{fileName}.json";
            GarageSerializer garageSerializer = new GarageSerializer();
            garageSerializer.JsonSerialize(this, filePath) ;
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
                #region Menu
                Console.WriteLine($"Garage {Name} Menu");
                Console.WriteLine("[2] Add a vehicle");
                Console.WriteLine("[3] Display Garage");
                Console.WriteLine("[4] Set all lot names in the Park");
                Console.WriteLine("[5] Set the heigth of the Park");
                Console.WriteLine("[6] Set charging stations of all lots in the Park");
                Console.WriteLine("[7] Exit Garage Menu");
                Console.Write("Option: ");
                #endregion

                switch (Console.ReadLine())
                {
                    #region Add a vehicle
                    case "2":
                        {
                            try
                            {
                                Vehicle vehicle = Vehicle.UICreator();
                                try
                                {
                                    vehicle.UIPark(this);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Error while adding vehicle to lot");

                                }
                            }
                            catch (Exception) { Console.WriteLine("Error while creating vehicle"); }
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
                            UISave(FileName);
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

        #region static Load(fileName) - Load and return a json MyGarage object
        /// <summary>
        /// Load a MyGarage object with contents from a json file in /parks
        /// </summary>
        /// <returns>The loaded park</returns>
        public static MyGarage Load(string fileName)
        {
            string filePath = $"../../../parks/{fileName}.json";
            GarageSerializer garageSerializer = new GarageSerializer();
            return garageSerializer.JsonDeserialize(typeof(MyGarage), filePath) as MyGarage;
        }
        #endregion

        #region UISave() - Serialize the garage to /parks
        /// <summary>
        /// UI for saving the park to a json in /parks. Ask save or save as (user input)
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>The filename chosen by the user</returns>
        public string UISave(string fileName)
        {
            Console.WriteLine($"Exiting: Garage {fileName} ");
            Console.WriteLine($"[1] Save as {fileName}.json");
            Console.WriteLine("[2] Save as... .json");
            Console.WriteLine("[3] Don't save");
            Console.Write("Option: ");
            switch (Console.ReadLine())
            {
                #region Save
                case "1":
                    {
                        Save(fileName);
                        break;
                    }
                #endregion
                #region Save as
                case "2":
                    {
                        Console.Write("File name: ");
                        fileName = Console.ReadLine();
                        Save(fileName);
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
            return fileName;
        }
        #endregion

        #region GetAllLots()
        public List<Lot> GetAllLots()
        {
            List<Lot> lots = new List<Lot>();

            for (int i = 0; i < Locations.Count; i++)
            {
                Location location = Locations[i];

                foreach (Lot lot in location.GetAllLots())
                {
                    lots.Add(lot);
                }
            }
            return lots;
        }
        #endregion
        #region CheckLots(vehicle, lot, filter) - Goes over lot/lots in a Row until remaining vehicle size is 0 
        /// <summary>
        /// Goes over lot(s) staring at input lot, filling each lot, until remaining vehicle size is 0,
        /// </summary>
        /// <returns>true if vehicle can fit here, false if not</returns>
        public bool CheckLots(Vehicle vehicle, Lot lot, List<Lot> filter)
        {
            Row row = lot.Row;
            int i = lot.Index;
            
            int vehicleSizeLeft = vehicle.Size;
            if (vehicle.Size >= 4) // if vehicle is car or bigger
            {
                // As long as lot is empty, height is less than vehicle, the lot exists in the filter, Iterate lots until vehicle size is 'emptied' - return true
                while ((i < row.Lots.Length) &&
                    row.Lots[i].SpaceLeft >= 4 &&
                    lot.Heigth >= vehicle.Heigth &&
                    vehicleSizeLeft != 0 &&
                    filter.Contains(lot) 
                    )
                {
                    lot = row.Lots[i];
                    vehicleSizeLeft -= 4;
                    i++;
                }
                if (vehicleSizeLeft == 0) // If while loop emptied vehicle size
                {
                    return true;
                }
            }
            else if (vehicle.Size < 4 && vehicle.Size > 0) // If vehicle is smaller than car
            {
                // If lot is empty, height is less than vehicle, the lot exists in the filter, vehicle fits - return true
                if (lot.SpaceLeft >= vehicleSizeLeft &&
                    lot.Heigth >= vehicle.Heigth &&
                    filter.Contains(lot))
                {
                    return true;
                }
            }
            return false;
        }
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
                    //Locations[i].Rows[ii].LocationIndex = i;
                    for (int iii = 0; iii < Locations[i].Rows[ii].Lots.Length; iii++)
                    {
                        Locations[i].Rows[ii].Lots[iii].Index = iii;
                        //Locations[i].Rows[ii].Lots[iii].RowIndex = ii;
                        //Locations[i].Rows[ii].Lots[iii].LocationIndex = i;
                        Locations[i].Rows[ii].Lots[iii].Number = lotNumber;
                        lotNumber++;
                    }
                }
            }
            Size = lotNumber;
        }
        #endregion
        #region SetReferences()
        /// <summary>
        /// Set backward references for all objects in garage. lot.Row = row its inside of etc.
        /// </summary>
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
            Console.WriteLine(Name);
            Console.WriteLine(Locations[0].Rows[0].Lots[0].Row.Location.Garage.Name);
        }
        #endregion
    }
}