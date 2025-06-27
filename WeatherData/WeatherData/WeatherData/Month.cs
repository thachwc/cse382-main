using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherData
{
    public class Month
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ImageTemp { get; set; }
        public string MonthName { get; set; }
        public string Temperature { get; set; }
        public Month(string month, string temp)
        {
            MonthName = month;
            Temperature = temp;
            double temperature = Double.Parse(temp);
            if (temperature < 40.00)
                ImageTemp = "cold.png";
            else if (temperature > 80.00)
                ImageTemp = "hot.png";
            else
                ImageTemp = "avg.png";
        }
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", ImageTemp, MonthName, Temperature);
        }
    }
}
