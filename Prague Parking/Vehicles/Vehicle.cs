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

        // Functions
        #region Park(garage, lot, filter) - Parks the vehicle on lot (or lots if larger vehicle)
        /// <summary>
        /// Try add the vehicle to the specified lot start point + more if larger vehicle 
        /// </summary>
        /// <returns>Parking success</returns>
        public bool Park(Garage.Garage garage, Lot lot, List<Lot> filter)
        {
            
            Row row = lot.Row;
            int i = lot.Index;
            int vehicleSizeLeft = this.Size;

            bool canFit = garage.CheckLots(this, lot, filter);
            if (!canFit) return false;
            
            if (this.Size >= lot.Space) // If same size or bigger than lot
            {
                while (vehicleSizeLeft != 0 ) // While there is size left
                {
                    lot.Vehicles.Add(this); // Add vehicle to lot
                    lot.SpaceLeft -= lot.Space; // Remove all available space from lot
                    vehicleSizeLeft -= lot.Space; // Drain vehicle size

                    i++; // Move to next lot inside if to not go out of bound on last one
                    if (i < row.Lots.Length) 
                    {
                        lot = row.Lots[i];
                    }
                }
            }
            else if (this.Size < lot.Space) // If vehicle is smaller than lot
            {
                lot.Vehicles.Add(this);
                lot.SpaceLeft -= vehicleSizeLeft;
            }
            return true;
            
           
        }
        #endregion
        #region Unpark(garage)
        /// <summary>
        /// For every lot: if it contains the vehicle, remove it and update SpaceLeft of lot
        /// </summary>
        /// <param name="garage"></param>
        /// <returns>true if successfully unparked</returns>
        public bool Unpark(Garage.Garage garage)
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
        #region AvailableLots(filter, garage)
        /// <summary>
        /// Runs CheckLots() for every lot in garage using a filter.
        /// </summary>
        /// <param name="filter">A list of lots used as a filter. If a lot doesnt exist inside this filter it will be skipped inside CheckLots</param>
        /// <param name="garage">The garage to check</param>
        /// <returns>A list of available lots to park at</returns>
        private List<Lot> AvailableLots(List<Lot> filter, Garage.Garage garage)
        {
            List<Lot> allLots = garage.GetAllLots();
            List<Lot> availableLots = new List<Lot>();

            foreach (Lot lot in allLots)
            {
                if (garage.CheckLots(this, lot, filter))
                {
                    availableLots.Add(lot);
                }
            }
            return availableLots;
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

            TimeSpan freeTime = new TimeSpan(0, settings.Free_Time, 0);

            if (this.GetType() == typeof(Car))
            {
                pricePerHour = settings.Car_Price;
            }
            else if (this.GetType() == typeof(MC))
            {
                pricePerHour = settings.MC_Price;
            }
            else if (this.GetType() == typeof(Bike))
            {
                pricePerHour = settings.Bike_Price;
            }
            else if (this.GetType() == typeof(Truck))
            {
                pricePerHour = settings.Truck_Price;
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
        #region SetHeight()
        static public int SetHeight()
        {
            Console.Clear();
            Console.WriteLine("Ange fordonets höjd");
            Console.Write("Höjd: ");
            int height = int.TryParse(Console.ReadLine(), out height) ? height : int.MaxValue;
            return height;
        }
        #endregion
        #region SetId()
        static public string SetId()
        {
            Console.Clear();
            Console.WriteLine("Ange ett regnr");
            Console.Write("Regnr: ");
            while (true)
            {
                string id = Console.ReadLine().ToUpper().Replace(" ", "");
                if (id.Length != 0)
                {
                    return id;
                }
                Console.WriteLine("Ogiltigt.");
                Console.Write("Regnr: ");
            }
        }
        #endregion
        #region SetColor()
        static public string SetColor()
        {
            Console.Clear();
            Console.WriteLine("Ange en färg");
            Console.Write("Färg: ");
            string id = Console.ReadLine().ToLower().Replace(" ", "");
            id = id == "" ? null : id;
            return id;
        }
        #endregion
        #region SetHasCharger()
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

        //  Displays
        #region Display()
        public void Display()
        {
            string type = $"Typ: {Type}, ";
            string id = Id == null ? null : $"Regnr: {Id}, ";
            string color = Color == null ? null : $"Färg: {Color}, ";
            string electric = Electric == false ? $"Eldriven: Nej, " : $"Eldriven: Ja, ";
            string height = $"Height: {Heigth}, ";
            string arrival = $"Ankom: {Arrival}";
            Console.WriteLine($"{type}{id}{color}{height}{electric}{arrival}");
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

        // User interfaces
        #region UICreator() - Returns a Vehicle object
        /// <summary>
        /// UI for creating a vehicle
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
        #region UIMenu
        /// <summary>
        /// Menu of the vehicle.
        /// </summary>
        /// <param name="garage"></param>
        public void UIMenu(Garage.Garage garage)
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
                            UIMove(garage);
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
        #region UIMove()
        /// <summary>
        /// Remove the vehicle, then let user park it again
        /// </summary>
        /// <param name="garage"></param>
        /// <returns>true if moved, else false</returns>
        public bool UIMove(Garage.Garage garage)
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
        #region UIPark - Main UI for parking the vehicle
        /// <summary>
        /// Main UI for parking the vehicle
        /// </summary>
        /// <param name="garage"></param>
        /// <returns> True if vehicle parked, false if not</returns>
        public bool UIPark(Garage.Garage garage)
        {
            bool isParked = false;
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
                            isParked = UIParkAuto(garage);
                            isDone = true;
                            break;
                        }
                    case "2":
                        {
                            isParked = UIParkAt(garage);
                            isDone = true;
                            break;
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
        /// <returns>True if parked, false if not</returns>
        public bool UIParkAuto(Garage.Garage garage)
        {
            List<Lot> filter = Query.ByMinHeigth(garage.GetAllLots(), Heigth); // Returns all lots taller than the vehicle

            // Foreach lot in garage
            for (int i = 0; i < garage.Locations.Count; i++)
            {
                Location location = garage.Locations[i];
                for (int ii = 0; ii < location.Rows.Count; ii++)
                {
                    Row row = location.Rows[ii];
                    for (int iii = 0; iii < row.Lots.Length; iii++)
                    {
                        Lot lot = row.Lots[iii];
                        // Do
                        if (garage.CheckLots(this, lot, filter))
                        {
                            UIParkCommand(lot);
                            if (Park(garage, lot, filter))
                            {
                                return true;
                            }
                            else Console.WriteLine("Fel.");
                        } 
                    }
                }
            }
            return false;
        }
        #endregion
        #region UIParkAt(garage) - Let user filter the garage and pick a lot to park at
        /// <summary>
        /// Let user filter the garage and pick a lot to park at
        /// </summary>
        /// <returns>True if parked, false if not</returns>
        public bool UIParkAt(Garage.Garage garage)
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
                Console.WriteLine(" 1) Parkera fordonet");
                Console.WriteLine("");
                Console.WriteLine(" 2) Hitta platser på våning: ");
                Console.WriteLine(" 3) Filtrera Med/utan laddningsstation");
                Console.WriteLine(" 4) Filtrera min höjd");
                Console.WriteLine(" 5) Filtrera max höjd");
                Console.WriteLine(" 6) Rensa filter");
                Console.WriteLine(" 7) Backa");
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
                                Lot lot = queriedLots[i-1]; //  Select lot by index in available lots
                                Console.Clear();
                                // Try parking vehicle. if success exit method
                                if (garage.CheckLots(this, lot, filter))
                                {
                                    UIParkCommand(lot);
                                    if (Park(garage, lot, filter))
                                    {
                                        return true;
                                    }
                                    else Console.WriteLine("Fel.");
                                }
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
                                filter = garage.Locations[i - 1].GetAllLots();
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
        #region UIParkCommand
        /// <summary>
        /// Tell user to park the vehicle at a lot. Run after garage.CheckLots() but before this.Park()
        /// </summary>
        /// <param name="lot"> The lot to park at</param>
        public void UIParkCommand(Lot lot)
        {
            Console.Clear();
            Console.WriteLine($"Parkera fordonet på plats {lot.Number+1} vid {lot.Row.Location.GetName()}");
            Console.WriteLine("Tryck för att fortsätta");
            Console.ReadKey();
        }
        #endregion
    }
}