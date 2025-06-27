using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritePage : ContentPage
    {
        public FavoritePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetListViewSources();
        }

        private void ResetListViewSources()
        {
            List<DessertInfo> dessertList = DB.conn.Table<DessertInfo>().ToList();

            List<Dessert> itemSource = new List<Dessert>();

            foreach (DessertInfo item in dessertList)
            {
                if (Preferences.Get($"{item.DessertName}_FavoriteMode", "").Equals("fav.png"))
                {
                    itemSource.Add(new Dessert()
                    {
                        Image = item.Image,
                        DessertName = item.DessertName,
                        CountryName = item.CountryName
                    });
                }

            }
            if (itemSource.Count() != 0)
                labelText.IsVisible = false;
            else
                labelText.IsVisible = true;

            favList.ItemsSource = itemSource;
        }


        private async void favList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Dessert dessert = (Dessert)(sender as ListView).SelectedItem;
            RecipePage page = new RecipePage(dessert.DessertName, dessert.CountryName);
            await Navigation.PushAsync(page, true);
        }

        private class Dessert
        {
            public string Image { get; set; }
            public string DessertName { get; set; }
            public string CountryName { get; set; }
        }
    }
}