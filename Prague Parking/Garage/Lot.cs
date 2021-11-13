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
        public string Name { get; set; } // Optional name
        public int Heigth { get; set; } // Max height to fit a vehicle
        public bool HasCharger { get; set; } // If it has a charging station
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public int Space = 4;
        public int SpaceLeft = 4;
        #endregion

        #region Constructor
        public Lot(Row row, string name = null, int heigth = 0, bool hasCharger = false)
        {
            Row = row;
            Name = name == null ? "Unnamed" : name;
            Heigth = heigth;
            HasCharger = hasCharger;
        }
        #endregion

        #region Unused methods
        #region SetName() set Name prop
        public void SetName(string name)
        {
            Name = name == null ? Name : name;
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
        #region UISetName() Change the Name string
        /// <summary>
        /// Ask user for name. If empty or only spaces it doesn't change.
        /// </summary>
        public void UISetName()
        {
            Console.Write("Name: ");
            string name = Console.ReadLine().Trim();
            name = name == "" ? null : name;
            SetName(name);
        }
        #endregion
        #region UISetHeigth() Change the Heigth int
        /// <summary>
        /// Ask user for input heigth. Must be greater >= 0
        /// </summary>
        public void UISetHeigth()
        {
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
                //  On success
                SetHeigth(h);
            }
            else //if heigth not set
            {
                Console.WriteLine("Heigth didn't change");
            }
        }
        #endregion
        #region UISetHasCharger() Change the HasCharger bool
        /// <summary>
        /// Updates the HasCharger bool of the lot
        /// </summary>
        public void UISetHasCharger()
        {
            Console.WriteLine("Does this lot have a charging station?");
            Console.Write("y/n: ");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y": SetHasCharger(true); Console.WriteLine("Set to True"); break;
                case "n": SetHasCharger(false);  Console.WriteLine("Set to False"); break;
                default: Console.WriteLine("Didn't change");  break;
            }
        }
        #endregion
        #endregion

        #region Display() Display the properties of the Lot
        /// <summary>
        /// Display the Lot. Format: Lot: name, Heigth: number, Charger: true/false
        /// </summary>
        public void Display()
        {
            string hasCharger = HasCharger == true ? "Yes" : "No";
            string floorName = Row.Location.Name == null ? $"Floor: {Row.Location.Index.ToString()}" : Row.Location.Name;
            Console.WriteLine($"{floorName}, Nr: {Number}, Height: {Heigth}, Charging port: {hasCharger}");
            foreach (Vehicle vehicle in Vehicles)
            {
                vehicle.Display();
            }
        }
        #endregion

    }
}