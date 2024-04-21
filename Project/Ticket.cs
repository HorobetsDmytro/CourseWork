using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class Ticket
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public User user { get; set; }

        public Ticket() { }
        public Ticket(int id, int price, DateTime departureDateTime, DateTime arrivalDateTime, User user)
        {
            Id = id;
            Price = price;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
            this.user = user;
        }

        public Ticket(int id, int price, DateTime departureDateTime, DateTime arrivalDateTime)
        {
            Id = id;
            Price = price;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
        }
    }
}
