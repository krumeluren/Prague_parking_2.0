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
            int heigth = 500;
            string id = null;
            string color = null;
            bool electric = false;
            DateTime dateTime = DateTime.Now;
            return new Truck(dateTime, heigth, id, color, electric);
        }
    }
}