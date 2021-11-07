using System;

namespace Prague_Parking_2_0_beta.Garage
{
    class Lot
    {
        public string Name { get; set; }
        public Vehicle[] Contains { get; set; }
        public int Space { get; set; }
        public int SpaceLeft { get; set; }
        public Lot(string name)
        {
            Name = name;
            Space = 4;
            SpaceLeft = 4;
        }
    }
}