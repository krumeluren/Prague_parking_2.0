
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
        }
        #endregion

        #region Change lot data
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
            Console.Clear();
            Console.WriteLine("Floors/Locations: {0}", Locations.Count);

            foreach (var location in Locations)
            {
                location.Display();
                foreach (var row in location.Rows)
                {
                    Console.WriteLine(" ");
                    row.DisplayLots();
                }
            }
        }
        #endregion

        #region DisplayLocations()
        /// <summary>
        /// Display() all locations
        /// </summary>
        public void DisplayLocations()
        {
            foreach (var location in Locations)
            {
                location.Display();
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
            FileName = fileName;
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
            Console.Clear();
            bool isDone = false;
            while (!isDone)
            {
                #region Menu
                Console.WriteLine($"Garage {Name} Menu");
                Console.WriteLine("[1] Find a vehicle");
                Console.WriteLine("[2] Add a vehicle");
                Console.WriteLine("[3] Display Garage");
                Console.WriteLine("[4] Set all lot names in the Park");
                Console.WriteLine("[5] Set the heigth of the Park");
                Console.WriteLine("[6] Set charging stations of all lots in the Park");
                Console.WriteLine("[7] Exit Garage Menu");
                Console.Write("Val: ");
                #endregion

                switch (Console.ReadLine())
                {
                    #region Find a vehicle
                    case "1":
                        {
                            Vehicle vehicle = null;
                            if((vehicle = UISelectVehicle()) != null)
                            {
                                vehicle.UIMenu(this);
                            }
                            break;
                        }
                    #endregion
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
        #region UISave() - Serialize the garage to /parks
        /// <summary>
        /// UI for saving the park to a json in /parks. Ask save or save as
        /// </summary>
        /// <returns>The filename chosen by the user</returns>
        public string UISave()
        {
            Console.Clear();
            string fileName = FileName;
            Console.WriteLine($"Exiting: Garage {FileName} ");
            Console.WriteLine($"[1] Save as {FileName}.json");
            Console.WriteLine("[2] Save as... .json");
            Console.WriteLine("[3] Don't save");
            Console.Write("Option: ");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        Save(FileName);
                        break;
                    }
                case "2":
                    {
                        Console.Write("File name: ");
                        fileName = Console.ReadLine();
                        Save(fileName);
                        break;
                    }
                case "3":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid!");
                        break;
                    }
            }
            return fileName;
        }
        #endregion

        #region GetAllLots()
        /// <returns>Returns a list of lots inside the garage</returns>
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

        #region GetAllVehicles()
        /// <returns> A list of all Vehicles in garage</returns>
        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            List<Lot> lots = GetAllLots();
            foreach (var lot in lots)
            {
                foreach (var vehicle in lot.Vehicles)
                {
                    if (!vehicles.Contains(vehicle)) // Add vehicle to list if it isnt in list already. (A truck can be on 4 lots)
                    {
                        vehicles.Add(vehicle);
                    }
                }
            }
            return vehicles;
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

        #region UISelectVehicle()
        /// <summary>
        /// UI for filtering though vehicles and selecting one
        /// </summary>
        /// <returns>Selected vehicle, or null</returns>
        public Vehicle UISelectVehicle()
        {
            List<Vehicle> vehicles = GetAllVehicles();
            Vehicle vehicle = null;

            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Console.WriteLine("Sök efter ett fordon");
                foreach (Vehicle v in vehicles)
                {
                    int i = 0;
                    Console.Write($"{i + 1}: ");
                    v.Display();
                    i++;
                }

                #region Menu
                Console.WriteLine("");
                Console.WriteLine("[x]  Välj ett fordon");
                Console.WriteLine("");
                Console.WriteLine("Sök efter: ");
                Console.WriteLine(" [1]  Regnr");
                Console.WriteLine(" [2]  Typ");
                Console.WriteLine(" [3]  Färg");
                Console.WriteLine(" [4]  Våning");
                Console.WriteLine(" [5]  Parkeringsnummer");
                Console.WriteLine("");
                Console.WriteLine("[6]  Gör om");
                Console.WriteLine("[b]  Backa");
                Console.Write("Val: ");
                #endregion

                switch (Console.ReadLine().ToLower())
                {
                    #region Pick
                    case "x":
                        {
                            if ((vehicle = UIPickVehicle(vehicles)) != null)
                            {
                                return vehicle;
                            }
                            Console.WriteLine("Kunde inte ta bort något fordon");
                            break;
                        }
                    #endregion
                    #region Regnr
                    case "1":
                        {
                            Console.Write("Regnr: ");
                            vehicles = Query.VehicleQ.ById(vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Typ
                    case "2":
                        {
                            Console.WriteLine(" [1]  Bil");
                            Console.WriteLine(" [2]  MC");
                            Console.WriteLine(" [3]  Cykel");
                            Console.WriteLine(" [4]  Lastbil");
                            Console.Write("Val: ");
                            #region Switch
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    {
                                        vehicles = Query.VehicleQ.ByType(vehicles, typeof(Car));break;
                                    }
                                case "2":
                                    {
                                        vehicles = Query.VehicleQ.ByType(vehicles, typeof(MC));break;
                                    }
                                case "3":
                                    {
                                        vehicles = Query.VehicleQ.ByType(vehicles, typeof(Bike)); break;
                                    }
                                case "4":
                                    {
                                        vehicles = Query.VehicleQ.ByType(vehicles, typeof(Truck));break;
                                    }
                            }
                            break;
                            #endregion
                        }
                    #endregion
                    #region Color
                    case "3":
                        {
                            Console.Write("Färg: ");
                            vehicles = Query.VehicleQ.ByColor(vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Floor
                    case "4":
                        {
                            Console.Write("Våning: ");
                            vehicles = Query.VehicleQ.ByFloor(this, vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Lot number
                    case "5":
                        {
                            Console.Write("Parkeringsnummer: ");
                            vehicles = Query.VehicleQ.ByLotIndex(this, vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Clear
                    case "6":
                        {
                            vehicles = GetAllVehicles();
                            break;
                        }
                    #endregion
                    #region Back
                    case "b":
                        {
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Default
                    default:
                        {
                            Console.WriteLine("Fel");
                            break;
                        }
                        #endregion
                }
            }
            Console.Clear();
            return null;
        }
        #endregion


        #region UIPickVehicle()
        /// <summary>
        /// Pick a vehicle from the list
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns> A vehicle or null</returns>
        private Vehicle UIPickVehicle(List<Vehicle> vehicles)
        {
            int tries = 0;
            while (tries < 5) // user has 5 tries before exiting
            {
                Console.Clear();
                foreach (Vehicle v in vehicles)
                {
                    int index = 0;
                    Console.WriteLine($"{index + 1}: ");
                    v.Display();
                    index++;
                }
                Console.Write("Ange ett nummer: ");
                int i = 0;
                if (int.TryParse(Console.ReadLine(), out i)) 
                {
                    i -= 1;
                    if (i < vehicles.Count && i >= 0) // In range
                    {
                        return vehicles[i];
                    }
                }
                tries++;
            }
            return null;
        }
        #endregion


        #region Load(fileName) - Load and return a json MyGarage object
        /// <summary>
        /// Load a MyGarage object with contents from a json file in /parks and required initial methods
        /// </summary>
        /// <returns>The loaded park</returns>
        public static MyGarage Load(string fileName)
        {
            Console.Clear();
            string filePath = $"../../../parks/{fileName}.json";
            GarageSerializer garageSerializer = new GarageSerializer();
            MyGarage garage = garageSerializer.JsonDeserialize(typeof(MyGarage), filePath) as MyGarage;
            garage.SetIndexes();
            garage.SetReferences();
            return garage;
        }
        #endregion
        #region SetIndexes()
        /// <summary>
        /// Sets the indexes of all objects inside the garags. Lot index inside row, row index inside location etc
        /// </summary>
        public void SetIndexes()
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
            // below writes same name
           Console.Write(Name);
           Console.WriteLine(Locations[0].Rows[0].Lots[0].Row.Location.Garage.Name);
        }
        #endregion
    }
}