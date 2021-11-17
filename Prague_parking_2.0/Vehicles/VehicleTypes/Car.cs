using Prague_Parking;
using Prague_Parking_2_0.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0
{
    class Car : Vehicle
    {
        static int Settings_Size = Settings.Load().Car_Size;
        #region Constructor
        public Car(
            DateTime arrival,
            int heigth,
            string id,
            string color = null,
            bool electric = false)
            : base(arrival,
                heigth,
                Settings_Size,
                id,
                color,
                electric)
        {
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