using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Microbus : Vehicle
    {
        public bool HasAirConditioning {  get; set; }

        public Microbus() : base()
        {
            
        }

        public Microbus(bool hasAirConditioning, int numberOfSeats, string model) : base(numberOfSeats, model)
        {
            HasAirConditioning = hasAirConditioning;
        }
    }
}
