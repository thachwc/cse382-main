using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace RunningApp
{
    [Table("activity")]
    public class Activity : IEnumerable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime DatePerformed { get; set; }
        public int WholeNumber { get; set; }
        public double DecimalNumber { get; set; }
        public double Time { get; set; }
        public TimeSpan Duration { get; set; }
        [MaxLength(3)]
        public double HeartRate { get; set; }
        public override string ToString()
        {
            return string.Format("Distance = {0:0.0}   Time = {1}   HR = {2}   Date = {3}", Time, Duration, HeartRate, DatePerformed.Date.ToString("MM/dd/yyyy"));
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            // call the generic version of the method
            return this.GetEnumerator();
        }

        private IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
