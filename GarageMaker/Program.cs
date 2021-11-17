
using System;

using Prague_Parking_2_0.Garage;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prague_Parking_2_0
{
    class Program
    {
        static void Main(string[] args)
        {

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
                        Garage.Garage garage = garageSerializer.JsonDeserialize(typeof(Garage.Garage), filePath) as Garage.Garage;
                        garage.SetReferences();
                        garage.UIMenu();
                        break;
                    }
                #endregion
                #region Create new
                case "2":
                    {
                        Console.Write("\nEnter a name: ");
                        Garage.Garage garage = new Garage.Garage(Console.ReadLine());
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