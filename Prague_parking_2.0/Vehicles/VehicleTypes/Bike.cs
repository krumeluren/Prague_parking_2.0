using Prague_Parking;
using Prague_Parking_2_0.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0
{
    class Bike : Vehicle
    {
        static int Settings_Size = Settings.Load().Bike_Size;
        #region Constructor
        public Bike(
            DateTime arrival,
            int heigth,
            string color)
            : base(
                arrival,
                heigth,
                Settings_Size,
                color: color)
        {
            this.Type = "Bike";
        }
        #endregion

        public static Bike UICreate()
        {
            Console.Clear();
            string color = null;
            int height = int.MaxValue;

            height = SetHeight();
            color = SetColor();

            return new Bike(DateTime.Now, height, color);
        }
    }
}