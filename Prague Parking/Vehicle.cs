using Prague_Parking_2_0_beta.VehicleTypes;
using System;

namespace Prague_Parking_2_0_beta
{
    class Vehicle
    {
        #region Properties
        public string Id { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public double? Heigth { get; set; }
        public bool Electric { get; set; }
        public DateTime Arrival { get; set; }
        public string Location { get; set; }
        public string MainLot { get; set; }
        public string[] LotsOccupied { get; set; }
        #endregion

        #region Constructor
        public Vehicle
           (
            string id,
            string type,
            int size,
            string color = null,
            double? heigth = null,
            bool electric = false,
            DateTime? arrival = null
            )
        {
            Id = id;
            Type = type;
            Size = size;
            Color = color;
            Heigth = heigth;
            Electric = electric;
            Arrival = arrival == null ? Arrival = DateTime.Today : Arrival = (DateTime)arrival;
        }
        #endregion

        #region Create()
        public static Vehicle Create()
        {
            string id = null;
            string type = null;
            int size = -1;
            DateTime arrival;
            string color = null;
            int? heigth = null;
            bool electric = false;

            bool isDone = false; // for exiting while loop
            #region Interface for vehicle data
            while (isDone == false)
            {
                Console.WriteLine("Lägg till ett fordon");
                Console.Write("Ange ett registreringsnummer: ");
                id = Console.ReadLine().ToUpper();
                id = id == "" ? id = null : id;

                while (type == null && size == -1)
                {
                    var vehicle = ISelectVehicle();
                    type = vehicle.Item1; //   vehicle name
                    size = vehicle.Item2; //   vehicle size
                }

                Console.WriteLine("Enter för att skippa");
                Console.Write("Ange en färg: ");
                color = Console.ReadLine();
                color = color == "" ? color = null : color;

                #region Set heigth
                bool parseError = true;
                while (parseError)
                {
                    Console.WriteLine("Enter för att skippa");
                    Console.Write("Ange en höjd i cm: ");
                    string str = Console.ReadLine();

                    if (str == "")
                    {
                        Console.WriteLine("Höjd ej satt");
                        parseError = false;
                    }
                    else
                    {
                        int nonNullHeigth;
                        bool success = int.TryParse(str, out nonNullHeigth);
                        if (success)
                        {
                            heigth = nonNullHeigth;
                        }
                        else
                        {
                            Console.WriteLine("Inte ett giltigt tal. Försök igen.");
                        }
                    }
                }
                #endregion

                Console.WriteLine("Är fordonet el-driven?");
                Console.WriteLine("[Y] Ja");
                Console.WriteLine("[N] Nej");
                switch (Console.ReadLine().ToUpper())
                {
                    case "Y": electric = true; break;
                    default: electric = false; break;
                }

                Console.WriteLine("Fordonstyp: " + type);
                Console.WriteLine("Registreringsnummer: " + (id == null ? "Ej satt" : id));
                Console.WriteLine("Färg: " + (color == null ? "Ej satt" : color));
                Console.WriteLine("Höjd: " + (heigth == null ? "Ej satt" : heigth + " cm"));
                Console.WriteLine("Elektrisk: " + (electric ? "Ja" : "Nej"));

                Console.WriteLine("Ok?");
                Console.WriteLine("[Y] Ja");
                Console.WriteLine("[N] Gör om");
                Console.WriteLine("[X] Lämna");
                switch (Console.ReadLine())
                {
                    case "Y": isDone = true; break;
                    case "N": break;
                    default: return null; break;
                }
            } // end of while()
            #endregion

            return new Vehicle(id, type, size, color, heigth, electric);
        }
        #endregion

        #region ISelectVehicle

        //  Return (string, int)
        //  string "Car" "MC" etc
        //  int 4, 2 etc
        public static (string, int) ISelectVehicle()
        {
            string type = null;
            int size = -1;

            bool isDone = false;
            while (isDone == false)
            {
                Console.WriteLine("Ange ett fordonstyp: ");
                Console.WriteLine("[1] MC ");
                Console.WriteLine("[2] Personbil ");
                Console.WriteLine("[3] Cykel ");
                Console.WriteLine("[4] Lastbil ");
                Console.Write("Val: ");
                string option = Console.ReadLine();
                Console.WriteLine("");
                switch (option)
                {
                    case "1": type = MC.Type; isDone = true; size = MC.Size; break;
                    case "2": type = Car.Type; isDone = true; size = Car.Size; break;
                    case "3": type = Bike.Type; isDone = true; size = Bike.Size; break;
                    case "4": type = Truck.Type; isDone = true; size = Truck.Size; break;
                    default: Console.WriteLine("Ogiltigt\n"); break;
                }
            }
            return (type, size);
        }
        #endregion
    }
}