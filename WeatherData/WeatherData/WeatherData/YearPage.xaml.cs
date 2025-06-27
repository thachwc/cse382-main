using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YearPage : ContentPage
    {
        int pickedYear;
        public YearPage(int year)
        {
            InitializeComponent();
            pickedYear = year;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetListViewSources();
        }
        private void ResetListViewSources()
        {
            DateTime upperYear = new DateTime(pickedYear + 1, 1, 1);
            DateTime lowerYear = new DateTime(pickedYear - 1, 12, 31);
            IEnumerable<WeatherInfo> thisYearsData = DB.conn.Table<WeatherInfo>().Where(d => d.Date > lowerYear && d.Date < upperYear).ToList();
            monthList.ItemsSource = thisYearsData.GroupBy(x => x.Date.Month).Select(x =>
            new Month(
                new DateTime(1, x.Key, 1).ToString("MMM"),
                x.Average(y => y.Temperature).ToString("F")
            )).ToList();
        }
    }
}