using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeatherData
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DB.OpenConnection();
            Page mainPage = new MainPage();
            MainPage = new NavigationPage(mainPage);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
