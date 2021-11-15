using Prague_Parking;
using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Vehicle
    {
        #region Properties
        public string Type { get; set; }
        public string Id { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int Heigth { get; set; }
        public bool Electric { get; set; }
        public DateTime Arrival { get; set; }
        public int Row { get; set; } = -1;
        public List<int> LotsOccupied { get; set; } = new List<int>();
        #endregion

        #region Constructor
        public Vehicle
           (
            DateTime arrival,
            int heigth,
            string id = null,
            string color = null,
            bool electric = false
            )
        {
            Id = id;
            Color = color;
            Heigth = heigth;
            Electric = electric;
            Arrival = arrival;
        }
        #endregion

        #region Display()
        public void Display()
        {
            string type = $"Type: {Type}, ";
            string id = Id == null ? null : $"ID: {Id}, ";
            string color = Color == null ? null : $"Color: {Color}, ";
            string electric = Electric == false ? $"Electric: No, " : $"Electric: Yes, ";
            string height = $"Height: {Heigth}, ";
            string arrival = $"Arrived: {Arrival}, ";
            Console.WriteLine($"{type}{id}{color}{height}{electric}{arrival}");
        }
        #endregion

        #region UICreator() - Returns a Vehicle object
        /// <summary>
        /// UI for creating a custom vehicle
        /// </summary>
        /// <returns>The vehicle created by user</returns>
        public static Vehicle UICreator()
        {
            Console.Clear();
            Console.WriteLine("Ange ett fordonstyp: ");
            Console.WriteLine(" 1) MC ");
            Console.WriteLine(" 2) Personbil ");
            Console.WriteLine(" 3) Cykel ");
            Console.WriteLine(" 4) Lastbil ");
            Console.Write("Val: ");
            switch (Console.ReadLine())
            {
                case "1": return MC.UICreate();
                case "2": return Car.UICreate();
                case "3": return Bike.UICreate();
                case "4": return Truck.UICreate();
                default: Console.WriteLine("Ogiltigt\n"); break;
            }
            return null;
        }
        #endregion

        #region Park(garage, lot, filter) - Parks the vehicle on lot (or lots if larger vehicle)
        /// <summary>
        /// Try add the vehicle to the specified lot start point + more if larger vehicle 
        /// </summary>
        /// <returns>Parking success</returns>
        public bool Park(MyGarage garage, Lot lot, List<Lot> filter)
        {
            bool canFit = garage.CheckLots(this, lot, filter);
            Row row = lot.Row;
            int i = lot.Index;

            int vehicleSizeLeft = this.Size;

            if (canFit)
            {
                if (this.Size >= 4) // If car or bigger
                {
                    while (i < row.Lots.Length && lot.SpaceLeft == 4 && vehicleSizeLeft != 0 && filter.Contains(lot)) // While the lot is empty and there is size left
                    {
                       
                        lot.Vehicles.Add(this); // Add vehicle to lot
                        lot.SpaceLeft -= 4; // Remove available space from lot
                        vehicleSizeLeft -= 4; // Drain vehicle size

                        i++; // Move to next lot
                        if (i < row.Lots.Length) // Inside if to not go out of bound on last one
                        {
                            lot = row.Lots[i];
                        }
                    }
                }
                if (this.Size < 4) // If smaller than car
                {
                    lot.Vehicles.Add(this);
                    lot.SpaceLeft -= vehicleSizeLeft;
                }
                return true;
            }
            else return false;
        }
        #endregion

        #region Unpark(garage)
        /// <summary>
        /// For every lot: if it contains the vehicle, remove it and update SpaceLeft of lot
        /// </summary>
        /// <param name="garage"></param>
        /// <returns>true if successfully unparked</returns>
        public bool Unpark(MyGarage garage)
        {
            bool removed = false;

            for (int i = 0; i < garage.Locations.Count; i++){
                for (int ii = 0; ii < garage.Locations[i].Rows.Count; ii++){
                    for (int iii = 0; iii < garage.Locations[i].Rows[ii].Lots.Length; iii++)
                    {
                        Lot lot = garage.Locations[i].Rows[ii].Lots[iii];
                        if (lot.Vehicles.Contains(this)){
                            if (lot.Unpark(this))
                            {
                                removed = true;
                            }}}}}
            return removed;
        }
        #endregion

        #region UIMenu
        /// <summary>
        /// Menu of the vehicle.
        /// </summary>
        /// <param name="garage"></param>
        public void UIMenu(MyGarage garage)
        {
            while (true)
            {
                Console.Clear();
                Display();
                Console.WriteLine(" ");
                Console.WriteLine(" 1)   Hämta ut fordon");
                Console.WriteLine(" 2)   Flytta fordon");
                Console.WriteLine(" b)   Backa");
                Console.Write("Val: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            Console.Clear();
                            Unpark(garage);
                            DisplayTimeSinceArrival();
                            DisplayPrice(CalcPrice());
                            Console.WriteLine("Tryck för att fortsätta");
                            Console.ReadKey();
                            Console.Clear();
                            return;
                        }
                    case "2":
                        {
                            Move(garage);
                            return;
                        }
                    case "b":
                        {
                            return;
                        }
                    default:
                        {
                            Console.WriteLine("Fel.");
                            break;
                        }
                }
            }
        }
        #endregion

        #region Move()
        /// <summary>
        /// Remove the vehicle, then let user park it again
        /// </summary>
        /// <param name="garage"></param>
        /// <returns>true if moved, else false</returns>
        public bool Move(MyGarage garage)
        {
            if (Unpark(garage))
            {
                return UIPark(garage);
            }
            else
            {
                Console.WriteLine("Fel inträffade vid försök att flytta fordonet");
            }
            return false;
        }
        #endregion

        #region DisplayTimeSinceArrival()
        /// <summary>
        /// Display the time since the vehicle arrived
        /// </summary>
        public void DisplayTimeSinceArrival()
        {
            TimeSpan since = DateTime.Now - Arrival;
            Console.WriteLine("Fordonet har varit på platsen i..");
            Console.WriteLine($"{since.Days} dagar, {since.Hours} timmar, {since.Minutes} minuter");
            Console.WriteLine($"Eller {Math.Floor(since.TotalHours)} timmar");
            Console.WriteLine($"Eller {Math.Floor(since.TotalMinutes)} minuter");
        }
        #endregion

        #region CalcPrice()
        /// <summary>
        /// Calculate the current price of the parked vehicles by loading a settings.json object and using the Arrival of the vehicle
        /// </summary>
        /// <returns>int price</returns>
        public double CalcPrice()
        {
            TimeSpan since = DateTime.Now - Arrival;
            double price = 0;
            double pricePerHour = 0;
            
            Settings settings = Settings.Load();

            TimeSpan freeTime = new TimeSpan(0, settings.FreeTime, 0);

            if (this.GetType() == typeof(Car))
            {
                pricePerHour = settings.CarPrice;
            }
            else if (this.GetType() == typeof(MC))
            {
                pricePerHour = settings.MCPrice;
            }
            else if (this.GetType() == typeof(Bike))
            {
                pricePerHour = settings.BikePrice;
            }
            else if (this.GetType() == typeof(Truck))
            {
                pricePerHour = settings.TruckPrice;
            }

            if (since < freeTime)
            {
                price = 0;
            }
            else
            {
                int hours = (int)Math.Floor(since.TotalHours);
                price = hours * pricePerHour;
            }
            return price;
        }
        #endregion

        #region DisplayPrice()
        /// <summary>
        /// Display the current price for the vehicle
        /// </summary>
        /// <param name="price"></param>
        public void DisplayPrice(double price)
        {
            Settings settings = Settings.Load();

            string currency = settings.Currency;
            Console.WriteLine($"Pris: {price} {currency}\n");
        }
        #endregion

        #region UIPark - Main UI for parking the vehicle
        /// <summary>
        /// Main UI for parking the vehicle
        /// </summary>
        /// <param name="garage"></param>
        /// <returns> True if vehicle parked, false if not</returns>
        public bool UIPark(MyGarage garage)
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Display();
                Console.WriteLine("Parkera fordon");
                Console.WriteLine(" 1)  Hitta första lediga plats");
                Console.WriteLine(" 2)  Låt mig bestämma");
                Console.WriteLine(" 3)  Ångra och backa");

                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            return UIParkAuto(garage);
                        }
                    case "2":
                        {
                            return UIParkAt(garage);
                        }
                    case "3":
                        {
                            isDone = true;
                            return false;
                        }
                    default:
                        {
                            Console.WriteLine("Fel!");
                            break;
                        }
                }
            }
            Console.Clear();
            return false;
        }
        #endregion

        #region UIParkAuto(garage) - Add the vehicle to the first available lot/lots
        /// <summary>
        /// Add vehicle to first available lot/lots
        /// </summary>
        public bool UIParkAuto(MyGarage garage)
        {
            bool success = false;
            List<Lot> filter = Query.ByMinHeigth(garage.GetAllLots(), Heigth); // Returns all lots taller than the vehicle
            for (int i = 0; i < garage.Locations.Count; i++)
            {
                Location location = garage.Locations[i];
                for (int ii = 0; ii < location.Rows.Count; ii++)
                {
                    Row row = location.Rows[ii];
                    for (int iii = 0; iii < row.Lots.Length; iii++)
                    {
                        Lot lot = row.Lots[iii];
                        success = Park(garage, lot, filter);
                        if (success)
                        {
                            return success;
                        }
                    }
                }
            }
            return success;
        }
        #endregion

        #region UIParkAt(garage) - Let user filter the garage and pick a lot to park at
        /// <summary>
        /// Let user filter the garage and pick a lot to park at
        /// </summary>
        /// <returns>True if parked, false if not</returns>
        public bool UIParkAt(MyGarage garage)
        {
            
            List<Lot> queriedLots = new List<Lot>();
            List<Lot> filter = Query.ByMinHeigth(garage.GetAllLots(), Heigth); // Return all lots taller than the vehicle

            bool isDone = false;
            while (!isDone) // While not exiting
            #region Switch Menu
            {
                Console.Clear();
                queriedLots = AvailableLots(filter, garage); // Available lots in garage using a filter
                int nr = 1;
                foreach (var lot in queriedLots) { Console.Write($"{nr}: "); lot.Display(); nr++; };

                #region Menu
                Console.WriteLine("");
                Console.WriteLine("[1] Parkera fordonet");
                Console.WriteLine("");
                Console.WriteLine("[2] Hitta platser på våning: ");
                Console.WriteLine("[3] Filtrera Med/utan laddningsstation");
                Console.WriteLine("[4] Filtrera min höjd");
                Console.WriteLine("[5] Filtrera max höjd");
                Console.WriteLine("[x] Rensa filter");
                Console.WriteLine("[b] Backa");
                Console.Write("Val: ");
                #endregion
                switch (Console.ReadLine())
                {
                    #region Parkera fordonet
                    case "1":
                        {
                            Console.Clear();
                            nr = 1;
                            foreach (var lot in queriedLots) { Console.Write($"{nr}: "); lot.Display(); nr++; };
                            Console.Write("Ange nr från listan: ");
                            int i;
                            bool success = int.TryParse(Console.ReadLine(), out i);
                            if (success && i >= 1 && i <= garage.Size)
                            {
                                i -= 1;
                                Lot lot = queriedLots[i]; //  Select lot by index in available lots
                                Console.Clear();
                                return Park(garage, lot, filter); // Try parking vehicle. if success exit method
                            }
                            break;
                        }
                    #endregion
                    #region Hitta platser på våning:
                    case "2":
                        {
                            int i;
                            Console.Write("Våning: ");
                            bool success = int.TryParse(Console.ReadLine(), out i);
                            if (success && i > 0 && i <= garage.Locations.Count)
                            {
                                filter = garage.Locations[i-1].GetAllLots();
                            }
                            break;
                        }
                    #endregion
                    #region Filtrera Med/utan laddningsstation
                    case "3":
                        {
                            Console.WriteLine(" 1) Med");
                            Console.WriteLine(" 2) Utan");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    {
                                        filter = Query.ByCharger(filter, true);
                                        break;
                                    }
                                case "2":
                                    {
                                        filter = Query.ByCharger(filter, false);
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Fel");
                                        break;
                                    }
                            }
                            Console.ReadLine();
                            break;
                        }
                    #endregion
                    #region Filtrera min höjd
                    case "4":
                        {
                            Console.Write("Minsta höjd till tak: ");
                            int i;
                            bool success = int.TryParse(Console.ReadLine(), out i);
                            if (success && i >= 0)
                            {
                                filter = Query.ByMinHeigth(filter, i);
                            }
                            break;
                        }
                    #endregion
                    #region Filtrera max höjd
                    case "5":
                        {
                            Console.Write("Max höjd till tak: ");
                            int i;
                            bool success = int.TryParse(Console.ReadLine(), out i);
                            if (success && i >= 0)
                            {
                                filter = Query.ByMaxHeigth(filter, i);
                            }
                            break;
                        }
                    #endregion
                    #region Rensa filter
                    case "x":
                        {
                            filter = garage.GetAllLots();
                            break;
                        }
                    #endregion
                    #region Backa
                    case "b":
                        {
                            Console.WriteLine("Backar..");
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Error
                    default:
                        {
                            Console.WriteLine("Fel!");
                            break;
                        }
                        #endregion
                }
            }

            #endregion End User interface
            Console.Clear();
            Console.WriteLine("Fordon ej parkerat");
            return false;
        }
        #endregion

        #region AvailableLots(filter, garage)
        /// <summary>
        /// Runs CheckLots() for every lot in garage using a filter.
        /// </summary>
        /// <param name="filter">A list of lots used as filter. Lot has to be inside. Do filter = garage.GetAllLots() if you want to check all lots</param>
        /// <param name="garage">The garage to check</param>
        /// <returns>A list of available lots to park at</returns>
        private List<Lot> AvailableLots(List<Lot> filter, MyGarage garage)
        {
            List<Lot> availableLots = new List<Lot>();
            for (int i = 0; i < garage.Locations.Count; i++)
            {
                Location location = garage.Locations[i];
                for (int ii = 0; ii < location.Rows.Count; ii++)
                {
                    Row row = location.Rows[ii];
                    for (int iii = 0; iii < row.Lots.Length; iii++)
                    {
                        Lot lot = row.Lots[iii];
                        if (garage.CheckLots(this, lot, filter))
                        {
                            availableLots.Add(lot);
                        }
                    }
                }
            }
            return availableLots;
        }
        #endregion

        #region Set Vehicle data
        static public int SetHeight()
        {
            Console.Clear();
            Console.WriteLine("Ange fordonets höjd");
            Console.Write("Höjd: ");
            int height = int.TryParse(Console.ReadLine(), out height) ? height : int.MaxValue;
            return height;
        }

        static public string SetId()
        {
            Console.Clear();
            Console.WriteLine("Ange ett regnr");
            Console.Write("Regnr: ");
            while (true)
            {
                string id = Console.ReadLine().Trim().ToUpper();
                if (id.Length != 0)
                {
                    return id;
                }
                Console.WriteLine("Ogiltigt.");
                Console.Write("Regnr: ");
            }
        }

        static public string SetColor()
        {
            Console.Clear();
            Console.WriteLine("Ange en färg");
            Console.Write("Färg: ");
            string id = Console.ReadLine().Trim().ToUpper();
            id = id == "" ? null : id;
            return id;
        }

        static public bool SetHasCharger()
        {
            Console.Clear();
            Console.WriteLine("Eldriven: y/n");
            Console.Write("Val: ");
            string answer = Console.ReadLine().Trim();
            if (answer == "y")
                return true;
            else
                return false;
        }
        #endregion

    }
}