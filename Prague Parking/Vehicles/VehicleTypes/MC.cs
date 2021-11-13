using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class MC : Vehicle
    {
        public MC(
            DateTime arrival,
            int heigth,
            string id,
            string color = null,
            bool electric = false
            ) : base (
                arrival,
                heigth,
                id,
                color,
                electric
                )
        {
            Size = 2;
            this.Type = "MC";
        }

        public static MC UICreate()
        {
            Console.Clear();
            int heigth = 150;
            string id = null;
            string color = null;
            bool electric = false;
            return new MC(DateTime.Now, heigth, id, color, electric);
        } 
    }
}