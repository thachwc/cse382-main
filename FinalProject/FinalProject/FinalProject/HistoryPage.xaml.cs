using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using SQLite;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
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
            List<History> list = DB.conn.Table<History>().OrderByDescending(x => x.Order).ToList();
            if (list.Count() != 0)
                labelText.IsVisible = false;
            else
                labelText.IsVisible = true;

            lv.ItemsSource = list;
        }

        private async void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            History dessert = (History)(sender as ListView).SelectedItem;
            RecipePage page = new RecipePage(dessert.DessertName, dessert.CountryName);
            await Navigation.PushAsync(page, true);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            List<History> historyTable = DB.conn.Table<History>().ToList();
            string fname = System.IO.Path.Combine(FileSystem.AppDataDirectory, "log.db");
            SQLiteConnection conn = new SQLiteConnection(fname);
            DB.DeleteTableContents("History");
            conn.CreateTable<History>();
            ResetListViewSources();
        }
    }
}