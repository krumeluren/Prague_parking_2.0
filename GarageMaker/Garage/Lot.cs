using System;

namespace Prague_Parking_2_0_beta.Garage
{
    [Serializable]
    public class Lot
    {
        #region Properties
        public string Name { get; set; }
        public int LocationNumber { get; set; }
        public int RowNumber { get; set; }
        public int Number { get; set; }
        public int Heigth { get; set; }
        public bool HasCharger { get; set; }
        #endregion

        #region Constructor
        public Lot() { }
        public Lot(string name = null, int? heigth = 0, bool hasCharger = false)
        {
            Name = name == null ? "Unnamed" : name;
            Heigth = heigth == null ? 9999 : (int)heigth;
            HasCharger = hasCharger;
        }
        #endregion

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

        //  User Interfaces
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

        #region Display() Display the properties of the Lot
        /// <summary>
        /// Display the Lot. Format: Lot: name, Heigth: number, Charger: true/false
        /// </summary>
        public void Display()
        {
            Console.WriteLine($"Name: {Name}, Heigth: {Heigth}, Charger: {HasCharger} ");
        }
        #endregion
    }
}