using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Truck : Vehicle
    {
        public Truck(
            int heigth,
            string id,
            string color = null,
            bool electric = false
            ) : base(
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
            int heigth = 0;
            string id = null;
            string color = null;
            bool electric = false;
            return new Truck(heigth, id, color, electric);
        }
    }
}