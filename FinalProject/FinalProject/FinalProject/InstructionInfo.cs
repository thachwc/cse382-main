using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace FinalProject
{
    [Table("instruction")]
    class InstructionInfo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int DessertID { get; set; }
        public string Step { get; set; }
        public string Content { get; set; }
    }
}
