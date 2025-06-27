using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountryPage : ContentPage
    {
        public CountryPage()
        {
            InitializeComponent();
            list.ItemsSource = DB.conn.Table<DessertInfo>().ToList().Select(s => new Country()
            {
                CountryName = s.CountryName,
                CountryFlag = s.CountryFlag
            }).GroupBy(x => x.CountryName).Select(w => w.FirstOrDefault()).OrderBy(o => o.CountryName).ToList();
        }

        private async void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Country countryName = (Country)(sender as ListView).SelectedItem;
            CountryDessertPage page = new CountryDessertPage(countryName.CountryName);
            await Navigation.PushAsync(page, true);
        }

        private class Country
        {
            public string CountryName { get; set; }
            public string CountryFlag { get; set; }
        }
    }
}