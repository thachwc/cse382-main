using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinalProject
{
    [Table("ingredient")]
    class IngredientInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int DessertID { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public override string ToString()
        {
            return string.Format("{0}/n{1}", Type, Content);
        }
    }
}
