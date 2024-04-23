using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Vehicle
    {
        public int NumberOfSeats { get; set; }
        public string Model { get; set; }

        public Vehicle() { }

        public Vehicle(int numberOfSeats, string model)
        {
            NumberOfSeats = numberOfSeats;
            Model = model;
        }
    }
}
