﻿
using System;
using Prague_Parking_2_0_beta.VehicleTypes;
using Prague_Parking_2_0_beta.Garage;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prague_Parking_2_0_beta
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("V 2.0 beta");




            Row row = Row.Create();


            Vehicle vehicle = Vehicle.Create();
            Console.WriteLine(vehicle);

        }
    }
}