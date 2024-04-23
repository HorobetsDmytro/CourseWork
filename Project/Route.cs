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
        public int RouteId { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<(City, DateTime)> IntermediateStations { get; set; }
        public int Price { get; set; }
        public Vehicle Vehicle { get; set; }

        public Route()
        {
            IntermediateStations = new List<(City, DateTime)>();
        }

        public Route(int routeId, string startCity, string endCity, DateTime departureDate, DateTime arrivalDate, DateTime departureTime, DateTime arrivalTime, List<(City, DateTime)> intermediateStations, int price, Vehicle vehicle)
        {
            RouteId = routeId;
            StartCity = startCity;
            EndCity = endCity;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            IntermediateStations = intermediateStations;
            Price = price;
            Vehicle = vehicle;
        }

        /*public Route(int routeId, string startCity, string endCity, DateTime departureDate, DateTime arrivalDate, DateTime departureTime, DateTime arrivalTime, List<(City, DateTime)> intermediateStations, int price, Bus bus)
        {
            this.routeId = routeId;
            StartCity = startCity;
            EndCity = endCity;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            IntermediateStations = intermediateStations;
            Price = price;
            this.bus = bus;
        }

        public Route(int routeId, string startCity, string endCity, DateTime departureDate, DateTime arrivalDate, DateTime departureTime, DateTime arrivalTime, List<(City, DateTime)> intermediateStations, int price, Microbus microbus)
        {
            this.routeId = routeId;
            StartCity = startCity;
            EndCity = endCity;
            DepartureDate = departureDate;
            ArrivalDate = arrivalDate;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            IntermediateStations = intermediateStations;
            Price = price;
            this.microbus = microbus;
        }*/
    }
}
