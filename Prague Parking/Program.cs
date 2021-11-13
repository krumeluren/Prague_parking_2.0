
using System;
using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Prague_Parking_2_0_beta
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("V 2.0 beta");


            MainMenu mainMenu = new MainMenu();
            bool isDone = false;
            while (!isDone)
            {
                isDone = mainMenu.Init();
            }


        }
    }
}