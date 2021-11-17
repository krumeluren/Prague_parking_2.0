
using System;
using Prague_Parking_2_0.Garage;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Prague_Parking_2_0
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("V 2.0");

            Initializer initializer = new Initializer();
            bool isDone = false;
            while (!isDone)
            {
                isDone = initializer.Init();
            }

            Console.WriteLine("Program avslutat");
            Console.ReadKey();
        }
    }
}