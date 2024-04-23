using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Bus : Vehicle
    {
        public bool HasToilet { get; set; }
        public bool HasWifi { get; set; }

        public Bus() : base()
        {
            
        }

        public Bus(bool hasToilet, bool hasWifi, int numberOfSeats, string model) : base(numberOfSeats, model)
        {
            HasToilet = hasToilet;
            HasWifi = hasWifi;
        }
    }
}
