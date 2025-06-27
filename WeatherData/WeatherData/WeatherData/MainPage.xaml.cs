using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WeatherData
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        YearPage yearPage;
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "Back");
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetListViewSources();
        }
        private void ResetListViewSources()
        {
            mainList.ItemsSource = DB.conn.Table<WeatherInfo>().GroupBy(x => x.Date.Year).Select(x => new
            {
                Year = x.Key,
                AverageTemp = Math.Round(x.Average(y => y.Temperature), 2)
            }).ToList();
        }
        private async void lv1_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            int year = Int32.Parse((sender as ListView).SelectedItem.ToString().Split(' ', ',').GetValue(3).ToString());
            yearPage = new YearPage(year);
            await Navigation.PushAsync(yearPage, true);
        }
    }
}
