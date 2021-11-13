
using System;

using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prague_Parking_2_0_beta
{
    class Program
    {
        static void Main(string[] args)
        {

            Lot lot = new Lot();
            Console.WriteLine(lot.Name);
            Console.WriteLine("GarageMaker 1.0b");


            Console.WriteLine("[1] Load Garage template");
            Console.WriteLine("[2] Create Garage template");
            Console.Write("Option: ");
            switch (Console.ReadLine())
            {
                #region Load
                case "1":
                    {
                        Console.Write("Enter the file name: ");
                        string fileName = Console.ReadLine();
                        string filePath = $"../../../templates/{fileName}.json";
                        GarageSerializer garageSerializer = new GarageSerializer();
                        MyGarage garage = garageSerializer.JsonDeserialize(typeof(MyGarage), filePath) as MyGarage;
                        garage.SetReferences();
                        garage.UIMenu();
                        break;
                    }
                #endregion
                #region Create new
                case "2":
                    {
                        Console.Write("\nEnter a name: ");
                        MyGarage garage = new MyGarage(Console.ReadLine());
                        garage.UIMenu();
                        break;
                    }
                #endregion
                default:{break;} 
            }

            Console.WriteLine("End of program");
            Console.ReadKey();
        }
    }
}