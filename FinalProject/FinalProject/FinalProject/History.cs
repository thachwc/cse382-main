using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalProject
{
    [Table("History")]
    public class History
    {
        [PrimaryKey, AutoIncrement]
        public int Order { get; set; }
        public string CountryName { get; set; }
        public string DessertName { get; set; }
        public string Image { get; set; }
    }
}
