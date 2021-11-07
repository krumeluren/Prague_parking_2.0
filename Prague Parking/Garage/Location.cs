using System;
using System.Collections.Generic;

namespace Prague_Parking_2_0_beta.Garage
{
    class Location
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public List<Row> Structure { get; set; }

        public Location()
        {
            Structure.Add(new Row());
        }

        public void CreateRows()
        {
            Location location = new Location();
        }

        public void Save()
        {

        }
    }
}