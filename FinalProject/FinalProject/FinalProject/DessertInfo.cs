using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinalProject
{
    [Table("dessert")]
    public class DessertInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int DessertID { get; set; }
        public string CountryName { get; set; }
        public string DessertName { get; set; }
        public string PrepTime { get; set; }
        public string CookTime { get; set; }
        public string ServingSize { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string CountryFlag { get; set; }
    }
}
