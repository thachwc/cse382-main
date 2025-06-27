using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (search.Text != null && search.Text != "")
            {
                List<Dessert> itemSource = new List<Dessert>();
                List<Dessert> dessert = DB.conn.Table<DessertInfo>().ToList().Where(x => x.DessertName.IndexOf(search.Text, StringComparison.OrdinalIgnoreCase) >= 0).Select(w => new Dessert()
                {
                    Name = w.DessertName,
                    Image = w.Image
                }).ToList();
                List<Dessert> country = DB.conn.Table<DessertInfo>().ToList().Where(x => x.CountryName.IndexOf(search.Text, StringComparison.OrdinalIgnoreCase) >= 0).Select(w => new Dessert()
                {
                    Name = w.CountryName,
                    Image = w.CountryFlag
                }).GroupBy(g => g.Name).Select(s => s.FirstOrDefault()).ToList();
                itemSource = dessert;
                itemSource.AddRange(country);
                list.ItemsSource = itemSource;
            }
            else
                list.ItemsSource = null;
        }
        private async void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Dessert dessert = (Dessert)(sender as ListView).SelectedItem;
            var dessertCountryList = DB.conn.Table<DessertInfo>().Select(w => new
            {
                w.DessertName,
                w.CountryName
            }).ToList();
            var countryList = DB.conn.Table<DessertInfo>().Select(s => new
            {
                s.CountryName,
                s.CountryFlag
            }).Distinct().ToList();
            foreach (var item in dessertCountryList)
            {
                if (dessert.Name.Equals(item.DessertName))
                {
                    RecipePage page = new RecipePage(item.DessertName, item.CountryName);
                    await Navigation.PushAsync(page, true);
                }
            }
            foreach (var item in countryList)
            {
                if (dessert.Name.Equals(item.CountryName))
                {
                    CountryDessertPage page2 = new CountryDessertPage(item.CountryName);
                    await Navigation.PushAsync(page2, true);
                }
            }
        }

        private class Dessert
        {
            public string Image { get; set; }
            public string Name { get; set; }
        }
    }
}