using System;

namespace Prague_Parking_2_0_beta.Garage
{
    class Row
    {
        public string Name { get; set; }
        public Lot[] Lots { get; set; }
        public int? Heigth { get; set; }
        public Row()
        {

        }
        public Row(int size, string name = "Unnamed row", int? heigth = null)
        {
            Name = name;
            size = size < 1 ? 1 : size; //  If size is less than 1, set to 1.
            Lots = new Lot[size];
            for (int i = 0; i < size; i++)
            {
                Lots[i] = new Lot(name);
            }
        }
        public static Row Create()
        {
            #region Set name of row
            Console.WriteLine("Enter a name of this row (Optional)");
            string name = Console.ReadLine();
            #endregion

            #region Set size of row
            Console.WriteLine("Enter the size of this row (Minimum of 1)");
            int size;
            while(!(int.TryParse(Console.ReadLine(), out size)) || size < 1) // While parse fails or size is smaller than minumum
            {
                Console.Write(size + " is invalid. Try again: ");
            }
            #endregion

            #region Set heigth
            Console.WriteLine("Enter a heigth of this row in centimeters (Optional)");
            int? heigth;
            string heigthStr = Console.ReadLine().Trim();
            int h;
            if (heigthStr != "") // If not empty input
            {
                while (!(int.TryParse(heigthStr, out h))) // While parse fails
                {
                    Console.Write("Invalid. Try again: ");
                    heigthStr = Console.ReadLine().Trim();
                }
                heigth = h; //  On success
            }
            else //if heigth not set
            {
                heigth = null;
            }
            Console.WriteLine(heigth);
            #endregion

            return new Row(size, name, heigth);
        }

        public Lot[] Content()
        {
            foreach (var Lot in Lots)
            {
                Console.WriteLine(Lot.Contains);
            }
            return Lots;
        }
    }
}