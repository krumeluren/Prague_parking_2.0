using Prague_Parking;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    class Garage
    {
        #region Properties
        public string Name { get; set; } // Name of garage
        public string FileName { get; set; } // The name of the file loaded
        public List<Location> Locations { get; set; }
        public int Size { get; set; } // lot count in garage
        #endregion
        #region Constructor
        public Garage(string name)
        {
            Name = name;
        }
        #endregion

        //  Functions
        #region Load() - Load json garage
        /// <summary>
        /// Load a Garage object with contents from a json file in /parks and required initial methods
        /// </summary>
        /// <returns>The loaded park</returns>
        public static Garage Load(string fileName)
        {
            Console.Clear();
            string filePath = $"../../../parks/{fileName}.json";
            GarageSerializer garageSerializer = new GarageSerializer();
            Garage garage = garageSerializer.JsonDeserialize(filePath) as Garage;
            garage.SetIndexes();
            garage.SetReferences();
            garage.UpdateSettings();
           

            return garage;
        }
        #endregion
        #region UpdateSettings()
        /// <summary>
        /// Updates vehicle and lot settings from _settings/settings.json
        /// </summary>
        public void UpdateSettings()
        {
            Settings settings = Settings.Load();
            
            List<Vehicle> vehicles = GetAllVehicles();
            foreach (Vehicle vehicle in vehicles)
            {
                if (vehicle.GetType() == typeof(Car))
                {
                    vehicle.Size = settings.Car_Size;
                }
                if (vehicle.GetType() == typeof(MC))
                {
                    vehicle.Size = settings.MC_Size;
                }
                if (vehicle.GetType() == typeof(Truck))
                {
                    vehicle.Size = settings.Car_Size;
                }
                if (vehicle.GetType() == typeof(Bike))
                {
                    vehicle.Size = settings.Bike_Size;
                }
            }
            List<Lot> lots = GetAllLots();
            foreach (Lot lot in lots)
            {
                lot.Space = settings.Size_Per_Lot;
                lot.UpdateAvailableSpace();
            }
        }
        #endregion
        #region SetIndexes()
        /// <summary>
        /// On load or garage change in structure. Sets the indexes of all objects
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
        /// On load or garage change in structure. Set references for all objects in garage. lot.Row = row its inside of etc. Useful for not requiring large loops later.
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
        #region CheckLots()
        /// <summary>
        /// Goes over lot(s) staring at input lot, filling each lot, until remaining vehicle size is 0,
        /// </summary>
        /// <returns>true if vehicle can fit on lot, false if not</returns>
        public bool CheckLots(Vehicle vehicle, Lot lot, List<Lot> filter)
        {
            Row row = lot.Row;
            int i = lot.Index; // Start lot

            int vehicleSizeLeft = vehicle.Size;
            // if vehicle is same size or larger than a lot
            if (vehicle.Size >= lot.Space) 
            {
                // While inside of row, lot is empty, lot height is taller than vehicle, the lot exists in the filter, not end of the row and vehicle has size left to fit..
                // Iterate lots until vehicle size is 'emptied' - return true
                while ((i < row.Lots.Length &&
                        lot.SpaceLeft >= lot.Space &&
                        lot.Heigth >= vehicle.Heigth &&
                        filter.Contains(lot) &&
                        i < row.Lots.Length &&
                        vehicleSizeLeft != 0)
                        )
                {
                    vehicleSizeLeft -= lot.Space;
                    i++; // Move to next lot inside if to not go out of bound on last index
                    if (i < row.Lots.Length)
                    {
                        lot = row.Lots[i];
                    }
                }
                // If while loop emptied vehicle size
                if (vehicleSizeLeft == 0) 
                {
                    return true;
                }
            }
            // If vehicle is smaller than a lot
            else if (vehicle.Size < lot.Space && vehicle.Size > 0) 
            {
                // If lot fits the vehicle, height is less than vehicle, the lot exists in the filter..
                // vehicle fits - return true
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

        //  Displays
        #region Display()
        /// <summary>
        /// Display the entire garage
        /// </summary>
        public void Display()
        {
            Console.Clear();
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
        #region Display2
        /// <summary>
        /// Graphical overview of the garage
        /// </summary>
        public void Display2()
        {
            foreach (Location location in Locations)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(" ");
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.ForegroundColor = ConsoleColor.Black;
                location.Display();
                Console.ForegroundColor = ConsoleColor.White;
                int counter = 0; // If squares on one line exceed 20 when moving to next row, write on  a new line
                foreach (Row row in location.Rows)
                {
                    if (counter > 20)
                    {
                        counter = 0;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(" ");

                    }
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");

                    foreach (Lot lot in row.Lots)
                    {
                        if (lot.SpaceLeft == lot.Space)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.Write($"[{lot.Number}]");
                        }

                        else if (lot.SpaceLeft == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write($"[{lot.Number}]");
                        }
                        else if (lot.SpaceLeft == 0 || lot.SpaceLeft != lot.Space)
                        {
                            Console.BackgroundColor = ConsoleColor.Blue;
                            Console.Write($"[{lot.Number}]");
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        counter++;
                    }


                }

                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
        #endregion

        // User interface
        #region UIMenu()
        /// <summary>
        /// User interface for managing this garage.
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Display2();
                #region Menu
                Console.WriteLine(" ");
                Console.WriteLine($"\nGarage {Name} Menu");
                Console.WriteLine(" 1) Hitta fordon");
                Console.WriteLine(" 2) Lägg till fordon");
                Console.WriteLine(" 3) Visa detaljerat garage");
                Console.WriteLine(" 4) Gå in i en våning");
                Console.WriteLine(" 5) Spara och avsluta");
                Console.Write("Val: ");
                #endregion

                switch (Console.ReadLine())
                {
                    #region Find a vehicle
                    case "1":
                        {
                            Vehicle vehicle = null;
                            if((vehicle = UIFilterVehicles()) != null)
                            {
                                vehicle.UIMenu(this);
                                Save(FileName);
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
                                    Save(FileName);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Error while adding vehicle to lot");
                                    Console.ReadKey();

                                }
                            }
                            catch (Exception) { Console.WriteLine("Error while creating vehicle"); Console.ReadKey(); }
                            break;
                        }
                    #endregion
                    #region Display();
                    case "3":
                        {
                            Display();
                            Console.Write("Tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                    #endregion
                    #region Enter a location menu
                    case "4":
                        {
                            foreach (var loc in Locations)
                            {
                                Console.WriteLine($"{loc.Index + 1}: {loc.Name}");
                            }
                            Console.Write("Nummer: ");
                            int i = 0;
                            if (int.TryParse(Console.ReadLine(), out i))
                            {
                                if (i - 1 >= 0 && i - 1 < Locations.Count)
                                    Locations[i-1].UIMenu();
                            }
                            break;
                        }
                    #endregion
                    #region Exit
                    case "5":
                        {
                            Console.WriteLine("Sparar..");
                            UISave();
                            Console.WriteLine("Avslutar..");
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
        #region UIFilterVehicles()
        /// <summary>
        /// UI for filtering vehicles and selecting one
        /// </summary>
        /// <returns>Selected vehicle, or null</returns>
        public Vehicle UIFilterVehicles()
        {
            List<Vehicle> vehicles = GetAllVehicles();
            Vehicle vehicle = null;

            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Console.WriteLine("Lista av fordon");
                int i = 1;
                foreach (Vehicle v in vehicles)
                {
                    Console.BackgroundColor = i % 2 == 0 ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write($"{i}: ");
                    v.Display();
                    i++;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                #region Menu
               
                Console.WriteLine("");
                Console.WriteLine("Sök efter: ");
                Console.WriteLine("  1)  Regnr");
                Console.WriteLine("  2)  Typ");
                Console.WriteLine("  3)  Färg");
                Console.WriteLine("  4)  Våning");
                Console.WriteLine("  5)  Parkeringsnummer");
                Console.WriteLine("  6)  Gör om");
                Console.WriteLine("  b)  Backa");
                Console.WriteLine("  x)  Välj ett fordon från listan");
                Console.WriteLine("");
                Console.Write(" Val: ");
                #endregion

                switch (Console.ReadLine().ToLower())
                {
                    #region Pick
                    case "x":
                        {
                            if ((vehicle = UISelectVehicle(vehicles)) != null)
                            {
                                return vehicle;
                            }
                            else
                            Console.WriteLine("Inget fordon togs bort");
                            break;
                        }
                    #endregion
                    #region Regnr
                    case "1":
                        {
                            Console.Write("Regnr: ");
                            vehicles = Query.VehicleQuery.ById(vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Typ
                    case "2":
                        {
                            Console.WriteLine(" 1)  Bil");
                            Console.WriteLine(" 2)  MC");
                            Console.WriteLine(" 3)  Cykel");
                            Console.WriteLine(" 4)  Lastbil");
                            Console.Write("Val: ");
                            #region Switch
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    {
                                        vehicles = Query.VehicleQuery.ByType(vehicles, typeof(Car));break;
                                    }
                                case "2":
                                    {
                                        vehicles = Query.VehicleQuery.ByType(vehicles, typeof(MC));break;
                                    }
                                case "3":
                                    {
                                        vehicles = Query.VehicleQuery.ByType(vehicles, typeof(Bike)); break;
                                    }
                                case "4":
                                    {
                                        vehicles = Query.VehicleQuery.ByType(vehicles, typeof(Truck));break;
                                    }
                                default: break;
                            }
                            break;
                            #endregion
                        }
                    #endregion
                    #region Color
                    case "3":
                        {
                            Console.Write("Färg: ");
                            vehicles = Query.VehicleQuery.ByColor(vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Floor
                    case "4":
                        {
                            Console.Write("Våning: ");
                            vehicles = Query.VehicleQuery.ByFloor(this, vehicles, Console.ReadLine());
                            break;
                        }
                    #endregion
                    #region Lot number
                    case "5":
                        {
                            Console.Write("Parkeringsnummer: ");
                            vehicles = Query.VehicleQuery.ByLotIndex(this, vehicles, Console.ReadLine());
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
        #region UISelectVehicle()
        /// <summary>
        /// Let user pick a vehicle from a list
        /// </summary>
        /// <param name="vehicles"></param>
        /// <returns> A vehicle or null</returns>
        private Vehicle UISelectVehicle(List<Vehicle> vehicles)
        {
            int tries = 0;
            while (tries < 5) // user has 5 tries before exiting
            {
                Console.Clear();
                int i = 1;
                foreach (Vehicle v in vehicles)
                {
                    Console.BackgroundColor = i % 2 == 0 ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write($"{i}: ");
                    v.Display();
                    i++;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.Write("Ange ett nummer: ");
                i = 0;
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
    }
}