using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace RunningApp
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();
            if (!Preferences.ContainsKey("GenderMode"))
                Preferences.Set("GenderMode", "Male");
            gender.Text = Preferences.Get("GenderMode", gender.Text);
            if (!Preferences.ContainsKey("MileMode"))
                Preferences.Set("MileMode", false);
            mile.IsToggled = Preferences.Get("MileMode", false);
            if (!Preferences.ContainsKey("DOBMode"))
                Preferences.Set("DOBMode", new DateTime(2000, 1, 1));
            date.Date = Preferences.Get("DOBMode", date.Date);
        }

        private void gender_TextChanged(object sender, TextChangedEventArgs e)
        {
            Preferences.Set("GenderMode", gender.Text);
        }

        private void switch_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("MileMode", mile.IsToggled);
            if (mile.IsToggled)
            {
                mileLabel.Text = "Kilometers";
            }
            else
            {
                mileLabel.Text = "Miles";
            }
        }

        private void date_DateSelected(object sender, DateChangedEventArgs e)
        {
            Preferences.Set("DOBMode", date.Date);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Uri uri = new Uri("https://www.miamioh.edu");
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }
    }
}