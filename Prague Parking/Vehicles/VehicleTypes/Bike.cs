using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Bike : Vehicle
    {
        public Bike(
            DateTime arrival,
            int heigth,
            string color = null
            ) : base (
                arrival,
                heigth,
                color: color
                )
        {
            this.Type = "Bike";
            Size = 1;
        }
        public static Bike UICreate()
        {
            Console.Clear();
            int heigth = 150;
            string color = null;
            return new Bike(DateTime.Now, heigth, color);
        }
    }
}