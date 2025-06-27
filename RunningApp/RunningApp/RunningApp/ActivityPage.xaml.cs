using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using Xamarin.Essentials;
using System.IO;
using Xamarin.Forms.Internals;

namespace RunningApp
{
    public partial class ActivityPage : ContentPage
    {
        SQLiteConnection conn;
        public ActivityPage()
        {
            InitializeComponent();
            string libFolder = FileSystem.AppDataDirectory;
            string fname = System.IO.Path.Combine(libFolder, "activity.db");
            conn = new SQLiteConnection(fname);
            conn.CreateTable<Activity>();
            for (int h = 0; h < 13; h++)
                hours.Items.Add(h.ToString());
            hours.SelectedIndex = 0;
            for (int m = 0; m < 60; m++)
                minutes.Items.Add(m.ToString());
            for (int l = 0; l < 51; l++)
                mile.Items.Add(l.ToString());
            for (int d = 0; d < 10; d++)
                dec.Items.Add(d.ToString());
            mile.SelectedIndex = 5;
            dec.SelectedIndex = 5;
            hours.SelectedIndex = 1;
            minutes.SelectedIndex = 30;
            lvActivities.ItemsSource = conn.Table<Activity>().ToList();
        }
        protected override void OnAppearing()
        {
            if (Preferences.Get("MileMode", true))
            {
                distanceLabel.Text = "Distance Kilometer";
            }
            else
            {
                distanceLabel.Text = "Distance Mile";
            }
            base.OnAppearing();
            ResetListViewSources(Preferences.Get("MileMode", true));
        }
        private void ResetListViewSources(bool kmUnitChange)
        {
            if (kmUnitChange)
            {
                List<Activity> testing = conn.Table<Activity>().Select(x => new Activity
                {
                    Id = x.Id,
                    DatePerformed = x.DatePerformed,
                    WholeNumber = (int)(x.WholeNumber * 1.6),
                    DecimalNumber = (int)(x.DecimalNumber * 1.6),
                    Time = x.Time * 1.6,
                    Duration = x.Duration,
                    HeartRate = x.HeartRate
                }).ToList();
                lvActivities.ItemsSource = testing;
                
            }
            else
            {
                lvActivities.ItemsSource = conn.Table<Activity>().ToList();
            }
        }
        private async void AddClicked(object sender, EventArgs e)
        {
            if (!Int32.TryParse(rate.Text, out int heart) || heart == 0)
            {
                await DisplayAlert("Error", "Invalid input in Heart Rate please enter in a number (greater than 0)", "OK");
                return;
            }
            if (Int32.TryParse((string)mile.SelectedItem, out int w) && Int32.TryParse((string)dec.SelectedItem, out int d))
            {
                if (w == 0 && d == 0)
                {
                    await DisplayAlert("Error", "Distance cannot be 0", "OK");
                    return;
                }
            }
            if (Int32.TryParse((string)hours.SelectedItem, out int h) && Int32.TryParse((string)minutes.SelectedItem, out int m))
            {
                if (h == 0 && m == 0)
                {
                    await DisplayAlert("Error", "Time cannot be 0", "OK");
                    return;
                }
            }
            Int32.TryParse((string)mile.SelectedItem, out int wholeNumber);
            Int32.TryParse((string)dec.SelectedItem, out int decimalNumber);
            Int32.TryParse((string)hours.SelectedItem, out int hour);
            Int32.TryParse((string)minutes.SelectedItem, out int min);
            if (Preferences.Get("MileMode", true))
            {
                double time = (wholeNumber + (decimalNumber / 10.0)) / 1.6;
                wholeNumber = (int)(Math.Truncate(time));
                decimalNumber = (int)((Math.Round((time - Math.Truncate(time)) * 10)));
            }
            Activity activity = new Activity
            {
                DatePerformed = date.Date,
                WholeNumber = wholeNumber,
                DecimalNumber = decimalNumber,
                Time = wholeNumber + (decimalNumber / 10.0),
                Duration = new TimeSpan(hour, min, 0),
                HeartRate = heart
            };
            conn.Insert(activity);
            ResetListViewSources(Preferences.Get("MileMode", true));

        }
        private async void UpdateClicked(object sender, EventArgs e)
        {
            if (!Int32.TryParse(rate.Text, out int heart) || heart == 0)
            {
                await DisplayAlert("Error", "Invalid input in Heart Rate please enter in a number (greater than 0)", "OK");
                return;
            }
            if (Int32.TryParse((string)mile.SelectedItem, out int w) && Int32.TryParse((string)dec.SelectedItem, out int d))
            {
                if (w == 0 && d == 0)
                {
                    await DisplayAlert("Error", "Distance cannot be 0", "OK");
                    return;
                }
            }
            if (Int32.TryParse((string)hours.SelectedItem, out int h) && Int32.TryParse((string)minutes.SelectedItem, out int m))
            {
                if (h == 0 && m == 0)
                {
                    await DisplayAlert("Error", "Time cannot be 0", "OK");
                    return;
                }
            }
            Int32.TryParse((string)mile.SelectedItem, out int wholeNumber);
            Int32.TryParse((string)dec.SelectedItem, out int decimalNumber);
            Int32.TryParse((string)hours.SelectedItem, out int hour);
            Int32.TryParse((string)minutes.SelectedItem, out int min);
            Activity oldActivity = lvActivities.SelectedItem as Activity;
            double time = 0;
            if (Preferences.Get("MileMode", true))
            {
                time = (wholeNumber + (decimalNumber / 10.0)) / 1.6;
                wholeNumber = (int)(Math.Truncate(time));
                decimalNumber = (int)((Math.Round((time - Math.Truncate(time)) * 10)));
            }
            else
            {
                time = wholeNumber + (decimalNumber / 10.0);
            }
            Activity newActivity = new Activity
            {
                DatePerformed = date.Date,
                WholeNumber = wholeNumber,
                DecimalNumber = decimalNumber,
                Time = time,
                Duration = new TimeSpan(hour, min, 0),
                HeartRate = heart
            };
            if (oldActivity != null)
            {
                newActivity.Id = oldActivity.Id;
                conn.Update(newActivity);
                ResetListViewSources(Preferences.Get("MileMode", true));
            }
            else
            {
                await DisplayAlert("Error", "No selected data available to update.\nPlease select or add data before updating.", "OK");
            }

        }

        private void DeleteClicked(object sender, EventArgs e)
        {
            Activity activity = lvActivities.SelectedItem as Activity;
            if (activity != null)
            {
                int v = conn.Delete(activity);
                if (v > 0)
                {
                    lvActivities.SelectedItem = null;
                    ResetListViewSources(Preferences.Get("MileMode", true));
                }
            }
        }

        private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Activity activity = lvActivities.SelectedItem as Activity;
        }
    }
}