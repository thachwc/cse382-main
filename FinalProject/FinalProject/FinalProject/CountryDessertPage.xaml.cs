using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountryDessertPage : ContentPage
    {
        string pickedCountry;
        public CountryDessertPage(string countryName)
        {
            InitializeComponent();
            pickedCountry = countryName;
            lv.ItemsSource = DB.conn.Table<DessertInfo>().Where(s => s.CountryName.Equals(pickedCountry)).Select(n => new Dessert()
            {
                DessertName = n.DessertName,
                Image = n.Image
            }).OrderBy(o => o.DessertName).ToList();
        }

        private async void lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Dessert dessertName = (Dessert)(sender as ListView).SelectedItem;
            RecipePage page = new RecipePage(dessertName.DessertName, pickedCountry);
            await Navigation.PushAsync(page, true);
        }

        private class Dessert
        {
            public string DessertName { get; set; }
            public string Image { get; set; }
        }
    }
}