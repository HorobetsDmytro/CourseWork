using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    class Country
    {
        public string NameOfCountry {  get; set; }
        public List<City> CityList { get; set; }
        public Country() { }

        public Country(string nameOfCountry, List<City> cityList)
        {
            this.NameOfCountry = nameOfCountry;
            CityList = cityList;
        }
    }
}
