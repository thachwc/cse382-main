using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;


namespace RunningApp
{
    public partial class TotalPage : ContentPage
    {
        SQLiteConnection conn;

        public TotalPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            ObservableCollection<Total> totalList = new ObservableCollection<Total>();
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, "activity.db");
            conn = new SQLiteConnection(fname);
            Func<DateTime, int> groupWeek =
                date => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            List<IGrouping<int, Activity>> groups = new List<IGrouping<int, Activity>>();
            if (Preferences.Get("MileMode", true))
            {
                groups = conn.Table<Activity>().OrderBy(x => x.DatePerformed).Select(x => new Activity
                {
                    Id = x.Id,
                    DatePerformed = x.DatePerformed,
                    WholeNumber = (int)(x.WholeNumber * 1.6),
                    DecimalNumber = (int)(x.DecimalNumber * 1.6),
                    Time = (x.WholeNumber + (x.DecimalNumber / 10.0)) * 1.6,
                    Duration = x.Duration,
                    HeartRate = x.HeartRate
                }).GroupBy(x => groupWeek(x.DatePerformed)).ToList();
            }
            else
            {
                groups = conn.Table<Activity>().OrderBy(x => x.DatePerformed).GroupBy(x => groupWeek(x.DatePerformed)).ToList();
            }
            foreach (var key in groups)
            {
                var valuesCount = key.Count();
                double totalTime = key.Sum(x => x.Time);
                TimeSpan totalTimeSpan = TimeSpan.FromSeconds(new TimeSpan(key.Sum(x => x.Duration.Ticks)).TotalSeconds);
                Total weekTotal = new Total
                {
                    DatePerformed = key.First().DatePerformed,
                    WholeNumber = (int)totalTime,
                    DecimalNumber = (int)((totalTime - Math.Truncate(totalTime)) * 10),
                    Time = totalTime,
                    Duration = new TimeSpan(totalTimeSpan.Hours, totalTimeSpan.Minutes, 0),
                    HeartRate = key.Sum(x => x.HeartRate) / valuesCount
                };
                totalList.Add(weekTotal);
            }
            lvTotal.ItemsSource = totalList;
            base.OnAppearing();
        }
        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}