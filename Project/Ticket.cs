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
        public Route RouteInfo { get; set; }
        public string PassengerName { get; set; }
        public string PassengerSurname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Price { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }

        public Ticket() { }

        public Ticket(int id, int price, DateTime departureDateTime, DateTime arrivalDateTime)
        {
            Id = id;
            Price = price;
            DepartureDateTime = departureDateTime;
            ArrivalDateTime = arrivalDateTime;
        }
    }
}
