using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class City
    {
        public string NameOfCity { get; set; }
        public string ParkingAddress { get; set; }

        public City() { }

        public City(string nameOfCity, string addressOfParkingLot)
        {
            this.NameOfCity = nameOfCity;
            this.ParkingAddress = addressOfParkingLot;
        }
    }
}
