using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Bike : Vehicle
    {
        public Bike(
            int heigth,
            string color = null
            ) : base (
                heigth,
                color: color
                )
        {
            this.Type = "Bike";
            Size = 1;
        }
        public static Bike UICreate()
        {
            int heigth = 0;
            string color = null;

            return new Bike(heigth, color);
        }
    }
}