using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project
{
    class Route
    {
        public int routeId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime ShipmentDate { get; set; }
        public List<Ticket> Tickets { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }

        public Route(int id, string startCity, string endCity, City city, Country country)
        {
            routeId = id;
            StartCity = startCity;
            EndCity = endCity;
            Tickets = new List<Ticket>();
            City = city;
            Country = country;
        }

    }
}
