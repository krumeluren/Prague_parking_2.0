using Prague_Parking;
using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class MC : Vehicle
    {
        #region Constructor
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
            Size = Settings.Load().MC_Size;
            this.Type = "MC";
        }
        #endregion

        public static MC UICreate()
        {
            Console.Clear();
            string id = null;
            string color = null;
            bool electric = false;
            int height = int.MaxValue;

            id = SetId();
            height = SetHeight();
            color = SetColor();
            electric = SetHasCharger();

            return new MC(DateTime.Now, height, id, color, electric);
        } 
    }
}