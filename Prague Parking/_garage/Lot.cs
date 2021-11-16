using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    class Lot
    {
        #region Properties
        public int Index { get; set; } // Index inside row
        public Row Row { get; set; }
        public int Number { get; set; } // Index of all lots in garage
        public int Heigth { get; set; } // Max height to fit a vehicle
        public bool HasCharger { get; set; } // If it has a charging station
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public int Space = 4;
        public int SpaceLeft = 4;
        #endregion
        #region Constructor
        public Lot(int heigth = 0, bool hasCharger = false)
        {
            Heigth = heigth;
            HasCharger = hasCharger;
        }
        #endregion

        //  Functions
        #region Unpark()
        /// <summary>
        /// Remove vehicle from the lot and UpdateAvailableSpace()
        /// </summary>
        /// <param name="vehicle"></param>
        /// <return>true if successfully removed</return>
        public bool Unpark(Vehicle vehicle)
        {
            if (Vehicles.Remove(vehicle))
            {
                UpdateAvailableSpace();
                return true;
            }
            return false;
        }
        #endregion
        #region UpdateAvailableSpace()
        /// <summary>
        /// Update available space on the lot based on parked vehicles 
        /// </summary>
        public void UpdateAvailableSpace()
        {
            int spaceUsed = 0;
            foreach (Vehicle vehicle in Vehicles)
            {
                if (vehicle.Size >= 4)
                {
                    spaceUsed += 4;
                }
                else
                {
                    spaceUsed += vehicle.Size;
                }
            }
            SpaceLeft = Space - spaceUsed;
        }
        #endregion
        #region SetHeigth() set Heigth prop
        public void SetHeigth(int h)
        {
            if (h >= 0)
            {
                Heigth = h;
            }
        }
        #endregion
        #region SetHasCharger() set bool HasCharger prop
        public void SetHasCharger(bool hasCharger)
        {
            HasCharger = hasCharger;
        }
        #endregion

        //  Displays
        #region Display() - display the lot and all vehicles
        /// <summary>
        /// Display the Lot and vehicles
        /// </summary>
        public void Display()
        {
            string hasCharger = HasCharger == true ? "Ja" : "Nej";
            string floorName = Row.Location.Name == null ? $"Våning: {Row.Location.Index.ToString()}" : Row.Location.Name;
            Console.Write($"{floorName}, Nr: {Number}, Height: {Heigth}, Laddningsstation: {hasCharger}\n");
            foreach (Vehicle vehicle in Vehicles)
            {
                vehicle.Display();
            }
        }
        #endregion

        //  User interfaces
        #region UIMenu
        /// <summary>
        /// Main UI for the lot
        /// </summary>
        public void UIMenu()
        {
            bool isDone = false;
            while (!isDone)
            {
                Console.Clear();
                Display();
                #region Menu
                Console.WriteLine($"Parkering {Number} Menu");
                Console.WriteLine(" 1) Sätt höjden på parkering");
                Console.WriteLine(" 2) Ändra laddningsstation på parkering");
                Console.WriteLine(" b) Backa");
                #endregion
                Console.Write("Val: ");
                #region Switch
                switch (Console.ReadLine())
                {
                    case "1":
                        {
                            UISetHeight();
                            break;
                        }
                    case "2":
                        {
                            UISetHasCharger();
                            break;
                        }
                    case "b":
                        {
                            Console.WriteLine("Backar..");
                            isDone = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Fel.");
                            break;
                        }
                }
                #endregion
            }
        }
        #endregion
        #region UISetHeigth() - Interface for setting Heigth
        /// <summary>
        /// Ask for int, if not empty, set height
        /// </summary>
        public void UISetHeight()
        {
            int heigth;
            Console.WriteLine("Enter för att skippa");
            Console.Write("Höjd: ");
            string heigthStr = Console.ReadLine().Trim();
            if (heigthStr != "") // If not empty input
            {
                if (int.TryParse(heigthStr, out heigth)) // While parse fails
                {
                    Heigth = heigth;
                }
            }
        }
        #endregion
        #region UISetHasCharger() Change the HasCharger bool
        /// <summary>
        /// Updates the HasCharger bool of the lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Har denna parkering en laddningsstation?");
            Console.Write("y/n: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y": SetHasCharger(true); Console.WriteLine("Set to True"); break;
                case "n": SetHasCharger(false); Console.WriteLine("Set to False"); break;
                default: Console.WriteLine("Didn't change"); break;
            }
        }
        #endregion
    }
}