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

        public void DisplayCitiesInDataGridView(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            dataGridView.Refresh();

            foreach (City city in CityList)
            {
                dataGridView.Rows.Add(NameOfCountry, city.NameOfCity, city.ParkingAddress);
            }
        }
    }
}
