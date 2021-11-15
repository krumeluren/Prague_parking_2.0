using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Car : Vehicle
    {
        #region Constructor
        public Car(
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
            Size = 4;
            Type = "Car";
        }
        #endregion

        public static Car UICreate()
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

            return new Car(DateTime.Now, height, id, color, electric);
        }
    }
}