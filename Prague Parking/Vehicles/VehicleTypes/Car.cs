using Prague_Parking_2_0_beta.Garage;
using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta
{
    class Car : Vehicle
    {
        public Car(
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
            Size = 4;
            Type = "Car";
        }

        public static Car UICreate()
        {
            int heigth = 0;
            string id = null;
            string color = null;
            bool electric = false;

            Console.Write("Ange höjd på fordon: ");
            heigth = int.TryParse(Console.ReadLine(), out heigth) == true ? heigth : 9999;

            Console.Write("Ange regnr: ");
            id = Console.ReadLine();

            Console.Write("Ange en färg: ");
            Console.ReadLine();

            Console.Write("Elbilsss: y/n ");
            string answer = Console.ReadLine().Trim();
            if(answer == "y")
            {
                electric = true;
            }
            else if(answer == "n")
            {
                electric = false;
            }
            return new Car(DateTime.Now, heigth, id, color, electric);
        }
    }
}