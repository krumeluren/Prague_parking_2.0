using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class MC : Vehicle
    {
        public MC( 
            int heigth,
            string id,
            string color = null,
            bool electric = false
            ) : base (
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
            int heigth = 0;
            string id = null;
            string color = null;
            bool electric = false;
            return new MC(heigth, id, color, electric);
        } 
    }
}