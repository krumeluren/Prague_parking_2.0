using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Truck : Vehicle
    {
        public Truck(
            DateTime arrival,
            int heigth,
            string id,
            string color = null,
            bool electric = false
            ) : base(
                arrival,
                heigth,
                id,
                color,
                electric
                )
        {
            Size = 16;
            this.Type = "Truck";
        }

        public static Truck UICreate()
        {
            Console.Clear();
            int heigth = 250;
            string id = null;
            string color = null;
            bool electric = false;
            return new Truck(DateTime.Now, heigth, id, color, electric);
        }
    }
}