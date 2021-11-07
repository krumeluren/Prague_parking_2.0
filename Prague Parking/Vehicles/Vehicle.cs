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
            int heigth,
            string id = null,
            string color = null,
            bool electric = false,
            DateTime? arrival = null
            )
        {
            Id = id;
            Color = color;
            Heigth = heigth;
            Electric = electric;
            Arrival = arrival == null ? DateTime.Today : (DateTime)arrival;
        }
        #endregion

        #region UICreator() - Returns a Vehicle object
        public static Vehicle UICreator()
        {

            Console.WriteLine("Ange ett fordonstyp: ");
            Console.WriteLine("[1] MC ");
            Console.WriteLine("[2] Personbil ");
            Console.WriteLine("[3] Cykel ");
            Console.WriteLine("[4] Lastbil ");
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

            #region copypaste later
            //string id = null;
            //DateTime arrival;
            //string color = null;
            //int heigth = 9999;
            //bool electric = false;


            //#region Interface for vehicle data
            //bool isDone = false; // exit while loop
            //while (!isDone)
            //{
            //    Console.WriteLine("Lägg till ett fordon");
            //    Console.Write("Ange ett registreringsnummer (Valfri): ");
            //    id = Console.ReadLine().Trim().ToUpper();
            //    id = id == "" ? id = null : id;



            //    Console.WriteLine("");
            //    Console.Write("Ange en färg (Valfri) : ");
            //    color = Console.ReadLine();
            //    color = color == "" ? color = null : color;

            //    #region Set heigth
            //    bool parseError = true;
            //    while (parseError)
            //    {
            //        Console.WriteLine("Enter för att skippa");
            //        Console.Write("Ange en höjd i cm: ");
            //        string str = Console.ReadLine();

            //        if (str == "")
            //        {
            //            Console.WriteLine("Höjd ej satt");
            //            parseError = false;
            //        }
            //        else
            //        {
            //            int nonNullHeigth;
            //            bool success = int.TryParse(str, out nonNullHeigth);
            //            if (success)
            //            {
            //                heigth = nonNullHeigth;
            //            }
            //            else
            //            {
            //                Console.WriteLine("Inte ett giltigt tal. Försök igen.");
            //            }
            //        }
            //    }
            //    #endregion

            //    Console.WriteLine("Är fordonet el-driven?");
            //    Console.WriteLine("[Y] Ja");
            //    Console.WriteLine("[N] Nej");
            //    switch (Console.ReadLine().ToUpper())
            //    {
            //        case "Y": electric = true; break;
            //        default: electric = false; break;
            //    }

            //    Console.WriteLine("Fordonstyp: " );
            //    Console.WriteLine("Registreringsnummer: " + (id == null ? "Ej satt" : id));
            //    Console.WriteLine("Färg: " + (color == null ? "Ej satt" : color));
            //    Console.WriteLine("Höjd: " + (heigth == null ? "Ej satt" : heigth + " cm"));
            //    Console.WriteLine("Elektrisk: " + (electric ? "Ja" : "Nej"));

            //    Console.WriteLine("Ok?");
            //    Console.WriteLine("[Y] Ja");
            //    Console.WriteLine("[N] Gör om");
            //    Console.WriteLine("[X] Lämna");
            //    switch (Console.ReadLine())
            //    {
            //        case "Y": isDone = true; break;
            //        case "N": break;
            //        default: return null; break;
            //    }
            //} // end of while()
            //#endregion
            #endregion
        }
        #endregion

        #region CheckLots(Row row, int l) - Goes over lot/lots in a Row until remaining vehicle size is 0 
        /// <summary>
        /// Goes over lot(s) staring at "l", filling up each lot, until remaining vehicle size is 0,
        /// </summary>
        /// <returns>true if vehicle size reached 0, false if problem</returns>
        private bool CheckLots(Row row, List<Lot> query, int l)
        {
            Lot lot = row.Lots[l];
            int vehicleSizeLeft = Size;
            if (Size >= 4) // if vehicle is car or bigger
            {
                // As long as lot is empty, height is less than vehicle, the lot exists in the query, Iterate lots until vehicle size is 'emptied' - return true
                while (row.Lots[l].SpaceLeft >= 4 &&
                    lot.Heigth >= Heigth &&
                    vehicleSizeLeft != 0 &&
                    query.Contains(lot)) 
                {
                    vehicleSizeLeft -= 4;
                    l++;
                    lot = row.Lots[l];
                }
                if (vehicleSizeLeft == 0) // If while loop emptied vehicle size
                {
                    return true;
                }
            }
            else if (Size < 4 && Size > 0) // If vehicle is smaller than car
            {
                // If lot is empty, height is less than vehicle, the lot exists in the query, vehicle fits - return true
                if (lot.SpaceLeft >= vehicleSizeLeft &&
                    lot.Heigth >= Heigth &&
                    query.Contains(lot)) 
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Park(Row row, int lot) - Parks the vehicle on lot (lots if larger vehicle)
        /// <summary>
        /// Try add the vehicle to the specified lot start point + more if larger vehicle 
        /// </summary>
        /// <returns>Parking success</returns>
        public bool Park(Row row, List<Lot> query, int l)
        {
            bool canFit = CheckLots(row, query, l);
            Lot lot = row.Lots[l];
            int vehicleSizeLeft = Size;

            if (canFit)
            {
                if (Size >= 4) // If car or bigger
                {
                    while (lot.SpaceLeft == 4 && vehicleSizeLeft != 0 && query.Contains(lot)) // While the lot is empty and there is size left
                    {
                        lot.Vehicles.Add(this); // Add vehicle to lot
                        lot.SpaceLeft -= 4; // Drain available lot of space
                        vehicleSizeLeft -= 4; // Drain vehicle size
                        l++; // Go to next lot
                        lot = row.Lots[l];
                    }
                }
                if (Size < 4) // If smaller than car
                {
                    lot.Vehicles.Add(this);
                    lot.SpaceLeft -= vehicleSizeLeft;
                }
                return true;
            }
            else return false;
        }
        #endregion

        #region UIPark(MyGarage garage) - Add the vehicle to the first available lot/lots
        /// <summary>
        /// Add vehicle to first available lot/lots
        /// </summary>
        public bool UIPark(MyGarage garage)
        {
            bool success = false;
            List<Lot> query = Query.ByMinHeigth(garage.GetAllLots(), Heigth);
            for (int i = 0; i < garage.Locations.Count; i++)
            {
                Location location = garage.Locations[i];
                for (int ii = 0; ii < location.Rows.Count; ii++)
                {
                    Row row = location.Rows[ii];
                    for (int lot = 0; lot < row.Lots.Length; lot++)
                    {
                        success = Park(row, query, lot);
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

        #region UIParkAt() - Return a list of available lots and let user choose
        /// <summary>
        /// Return a list of available lots and let user choose
        /// </summary>
        public void UIParkAt(MyGarage garage)
        {
            
            List<Lot> availableLots = new List<Lot>();
            List<Lot> queryList = Query.ByMinHeigth(garage.GetAllLots(), Heigth); // Returns all lots

            bool isDone = false;
            while (!isDone)
            {
                availableLots = AvailableLots(queryList, garage);
                int nr = 1;
                foreach (var lot in availableLots){ Console.Write($"{nr}: "); lot.Display(); nr++; };

                Console.WriteLine("[1] Parkera fordonet");
                Console.WriteLine("");
                Console.WriteLine("[2] Hitta platser på våning: ");
                Console.WriteLine("");
                Console.WriteLine("[3] Filtrera Med/utan laddningsstation");
                Console.WriteLine("[4] Filtrera min höjd");
                Console.WriteLine("[5] Filtrera max höjd");
                Console.WriteLine("");
                Console.WriteLine("[x] Rensa filter");
                Console.WriteLine("[b] Backa");
                switch (Console.ReadLine())
                {
                    #region Parkera fordonet
                    case "1":
                        {
                            Console.Write("Ange nr från listan: ");
                            int i;
                            bool success = int.TryParse(Console.ReadLine(), out i);
                            if (success && i >= 1 && i <= garage.Size)
                            {
                                i -= 1;
                                Lot lot = availableLots[i];
                                Row row = garage.Locations[lot.LocationNumber].Rows[lot.RowNumber];
                                Park(row, queryList, i);
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
                            if (success && i >= 0 && i < garage.Locations.Count)
                            {
                                queryList = garage.Locations[i].GetAllLots();
                            }

                            break;
                        }
                    #endregion
                    #region Filtrera Med/utan laddningsstation
                    case "3":
                        {
                            Console.WriteLine("[1] Med");
                            Console.WriteLine("[2] Utan");
                            switch (Console.ReadLine())
                            {
                                case "1":
                                    {
                                        queryList = Query.ByCharger(queryList, true);
                                        break;
                                    }
                                case "2":
                                    {
                                        queryList = Query.ByCharger(queryList, false);
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
                            if (success && i >= 0 && i < garage.Locations.Count)
                            {
                                queryList = Query.ByMinHeigth(queryList, i);
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
                            if (success && i >= 0 && i < garage.Locations.Count)
                            {
                                queryList = Query.ByMaxHeigth(queryList, i);
                            }
                            break;
                        }
                    #endregion
                    #region Rensa filter
                    case "x":
                        {
                            queryList = garage.GetAllLots();
                            break;
                        }
                    #endregion
                    #region Backa
                    case "b":
                        {
                            isDone = true;
                            break;
                        }
                    #endregion
                    #region Error
                    default:
                        {

                            break;
                        }
                    #endregion
                }
            }
        }
        #endregion

        #region AvailableLots(List<Lot> query, MyGarage garage)
        /// <summary>
        /// Runs CheckLots() for entire garage with a query filter
        /// </summary>
        /// <param name="query">Will only check lots if they exist in this filter</param>
        /// <param name="garage">the garage to check</param>
        /// <returns>A list of available lots to park at</returns>
        private List<Lot> AvailableLots(List<Lot> query, MyGarage garage)
        {
            List<Lot> availableLots = new List<Lot>();
            for (int i = 0; i < garage.Locations.Count; i++)
            {
                Location location = garage.Locations[i];
                for (int ii = 0; ii < location.Rows.Count; ii++)
                {
                    Row row = location.Rows[ii];
                    for (int lot = 0; lot < row.Lots.Length; lot++)
                    {
                        while (CheckLots(row, query, lot))
                        {
                            availableLots.Add(row.Lots[i]);
                        }
                    }
                }
            }
            return availableLots;
        }
        #endregion
    }
}