using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace RunningApp
{
    [Table("total")]
    public class Total
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DatePerformed { get; set; }
        public int WholeNumber { get; set; }
        public int DecimalNumber { get; set; }
        public double Time { get; set; }
        public TimeSpan Duration { get; set; }
        [MaxLength(3)]
        public double HeartRate { get; set; }
        public override string ToString()
        {
            return string.Format("Week = {0}   Time = {1}   Distance = {2:0.0}   HR = {3}", DatePerformed.Date.ToString("MM/dd/yyyy"), Duration, Time, HeartRate);
        }
    }
}
