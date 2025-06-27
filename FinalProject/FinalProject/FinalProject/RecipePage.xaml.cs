using System;
using System.Collections.Generic;
using System.Linq;
using Plugin.SimpleAudioPlayer;
using SQLite;
using Xamarin.Essentials;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinalProject
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipePage : TabbedPage
    {
        string pickedDessert;
        string pickedCountry;
        private bool currentOrientationLandscape;
        ISimpleAudioPlayer player = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
        ISimpleAudioPlayer player2 = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();

        public RecipePage(string dessertName, string countryName)
        {
            InitializeComponent();
            Load("fav.mp3");
            Load2("unfav.mp3");

            List<History> historyTable = DB.conn.Table<History>().ToList();
            string fname = System.IO.Path.Combine(FileSystem.AppDataDirectory, "log.db");
            SQLiteConnection conn = new SQLiteConnection(fname);

            if (!Preferences.ContainsKey($"{dessertName}_FavoriteMode"))
                Preferences.Set($"{dessertName}_FavoriteMode", "unfav.png");
            button.Source = Preferences.Get($"{dessertName}_FavoriteMode", "fav.png");

            NavigationPage.SetBackButtonTitle(this, "Back");

            pickedDessert = dessertName;
            pickedCountry = countryName;

            //About section
            cName.Text = pickedCountry;
            dName.Text = pickedDessert;
            var recipe = DB.conn.Table<DessertInfo>().Where(x => x.CountryName.Equals(pickedCountry) && x.DessertName.Equals(pickedDessert)).Select(w => new
            {
                Image = w.Image,
                PrepTime = w.PrepTime,
                ServingSize = w.ServingSize,
                CookTime = w.CookTime
            }).ToList();
            image.Source = recipe[0].Image;
            prep.Text = recipe[0].PrepTime;
            cook.Text = recipe[0].CookTime;
            serv.Text = recipe[0].ServingSize;

            List<string> list = DB.conn.Table<DessertInfo>().Where(x => x.CountryName.Equals(pickedCountry) && x.DessertName.Equals(pickedDessert)).Select(w => w.Description ).ToList();
            desc.ItemsSource = list;

            // History section
            History newHistoryObject = new History()
            {
                DessertName = dessertName,
                CountryName = countryName,
                Image = recipe[0].Image,
            };
            History historyObject = conn.Table<History>().Where(x => x.DessertName == dessertName).FirstOrDefault();
            if (historyObject != null)
            {
                conn.Delete(historyObject);
            }
            conn.Insert(newHistoryObject);

            // Ingredient section
            ingredientList.ItemsSource = DB.conn.Table<IngredientInfo>().Join(DB.conn.Table<DessertInfo>(), s => s.DessertID, d => d.DessertID, (s, d) => new { s, d }).Where(x => x.d.CountryName.Equals(pickedCountry) && x.d.DessertName.Equals(pickedDessert)).Select(w => new
            {
                Type = w.s.Type,
                Content = w.s.Content
            }).ToList();

            // Instruction section
            instructionList.ItemsSource = DB.conn.Table<InstructionInfo>().Join(DB.conn.Table<DessertInfo>(), s => s.DessertID, d => d.DessertID, (s, d) => new { s, d }).Where(x => x.d.CountryName.Equals(pickedCountry) && x.d.DessertName.Equals(pickedDessert)).Select(w => new
            {
                Step = w.s.Step,
                Content = w.s.Content
            }).ToList();
        }
        private void Load(string file)
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assembly.GetManifestResourceStream("FinalProject." + file);
            player.Load(audioStream);
        }
        private void Load2(string file)
        {
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assembly.GetManifestResourceStream("FinalProject." + file);
            player2.Load(audioStream);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (button.Source.ToString().Equals("File: unfav.png"))
            {
                button.Source = "fav.png";
                Preferences.Set($"{pickedDessert}_FavoriteMode", "fav.png");
                player.Play();
            }
            else
            {
                button.Source = "unfav.png";
                Preferences.Set($"{pickedDessert}_FavoriteMode", "unfav.png");
                player2.Play();
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            bool isNowLandscape = width > height;
            if (isNowLandscape != currentOrientationLandscape)
            {
                stack.Orientation = isNowLandscape ? StackOrientation.Horizontal : StackOrientation.Vertical;
                innerStack.Orientation = isNowLandscape ? StackOrientation.Vertical : StackOrientation.Vertical;

                currentOrientationLandscape = isNowLandscape;
            }
        }
    }
}