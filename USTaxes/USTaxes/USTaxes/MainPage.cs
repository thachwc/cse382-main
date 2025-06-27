using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace USTaxes
{
    public class MainPage : ContentPage
    {
        public class Location
        {
            public int zipcode;
            public string city;
            public string state;
            public double? averagetax;

            public Location(int zipcode, string city, string state, double? averagetax)
            {
                this.zipcode = zipcode;
                this.city = city;
                this.state = state;
                this.averagetax = averagetax;
            }

            public override string ToString()
            {
                if (averagetax != null) return $"{this.zipcode} {this.city} {this.state} {this.averagetax}";
                return $"{this.zipcode} {this.city} {this.state}";
            }
        }
        Entry taxReturn;
        Label taxReturnLabel;
        Entry city;
        Label cityLabel;
        Entry state;
        Label stateLabel;
        Picker query;
        Label queryLabel;
        Label error;
        Button button;

        List<Location> listAddress = new List<Location>();
        ObservableCollection<Location> currentList = new ObservableCollection<Location>();
        CollectionView listViewAmount;

        private static List<Location> GetFileContents(string fileName)
        {
            List<Location> l = new List<Location>();
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MainPage)).Assembly;
            string namespaceName = "USTaxes";
            Stream stream = assembly.GetManifestResourceStream(namespaceName + "." + fileName);
            string text = "";
            using (var reader = new System.IO.StreamReader(stream))
            {
                reader.ReadLine();
                int prevZipcode = 0;
                int totalWages = 0;
                int numberOfTaxReturn = 0;
                string city = "";
                string state = "";
                while (!reader.EndOfStream)
                {
                    text = reader.ReadLine();
                    string[] toks = text.Split('\t');
                    if (prevZipcode == 0)
                    {
                        prevZipcode = Int32.Parse(toks[1]);
                        city = toks[3];
                        state = toks[4];
                        try
                        {
                            totalWages = Int32.Parse(toks[18]);
                        }
                        catch
                        {
                            totalWages = 0;
                        }
                        if (toks[16] != "")
                        {
                            numberOfTaxReturn += Int32.Parse(toks[16]);
                        }
                    }
                    else
                    {
                        if (prevZipcode == Int32.Parse(toks[1]))
                        {
                            continue;
                        }
                        else
                        {
                            if (numberOfTaxReturn == 0)
                            {
                                l.Add(new Location(prevZipcode, city, state, null));
                            }
                            else
                            {
                                l.Add(new Location(prevZipcode, city, state, totalWages / numberOfTaxReturn));
                            }
                            totalWages = 0;
                            numberOfTaxReturn = 0;
                            prevZipcode = Int32.Parse(toks[1]);
                            city = toks[3];
                            state = toks[4];
                            try
                            {
                                totalWages = Int32.Parse(toks[18]);
                            }
                            catch
                            {
                                totalWages = 0;
                            }
                            if (toks[16] != "")
                            {
                                numberOfTaxReturn += Int32.Parse(toks[16]);
                            }
                        }
                    }
                }
            }
            return l;
        }

        public MainPage()
        {
            listAddress = GetFileContents("zipcodes.tsv.txt");
            listAddress = listAddress.OrderBy(x => x.zipcode).ToList();
            currentList = new ObservableCollection<Location>();
            ConfigureListView(out listViewAmount, currentList);


            taxReturnLabel = new Label { Text = "Tax return" };
            taxReturnLabel.FontSize = 16;
            taxReturn = new Entry();

            cityLabel = new Label { Text = "City" };
            city = new Entry();
            cityLabel.FontSize = 16;

            stateLabel = new Label { Text = "State" };
            stateLabel.FontSize = 16;
            state = new Entry();

            queryLabel = new Label { Text = "Query" };
            queryLabel.FontSize = 16;
            query = new Picker();
            string amount = "Amount";
            string citystate = "Citystate";
            query.Items.Add(amount);
            query.Items.Add(citystate);

            error = new Label();
            error.TextColor = Color.Red;

            button = new Button { Text = "GO" };
            button.Clicked += Button_Clicked;

            StackLayout panel = new StackLayout();

            panel.Children.Add(taxReturnLabel);
            panel.Children.Add(taxReturn);

            panel.Children.Add(cityLabel);
            panel.Children.Add(city);

            panel.Children.Add(stateLabel);
            panel.Children.Add(state);

            panel.Children.Add(queryLabel);
            panel.Children.Add(query);

            panel.Children.Add(error);

            panel.Children.Add(button);
            panel.Children.Add(listViewAmount);
            this.Content = panel;
        }

        private ObservableCollection<Location> Query_SelectedIndexChanged()
        {
            var result = new ObservableCollection<Location>();
            if (query.SelectedIndex == 0)
            {
                int low = Int32.Parse(taxReturn.Text) - 100;
                int high = Int32.Parse(taxReturn.Text) + 100;
                foreach (Location l in listAddress)
                {
                    if (l.averagetax < high && l.averagetax > low)
                    {
                        result.Add(l);

                    }
                }

            }
            else
            {
                if (city.Text.Equals("") || state.Text.Equals(""))
                {
                    throw new Exception();
                }
                foreach (Location l in listAddress)
                {
                    if (l.city.Equals(city.Text.ToUpper()) && l.state.Equals(state.Text.ToUpper()))
                    {
                        result.Add(l);
                    }
                }
            }
            return result;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                error.Text = "";
                listViewAmount.ItemsSource = Query_SelectedIndexChanged();
            }
            catch (Exception ex)
            {
                error.Text = "Empty or incorrect type inputs. Please check entries!";
            }
        }

        private void ConfigureListView(out CollectionView l, ObservableCollection<Location> currentList)
        {
            l = new CollectionView();
            l.ItemsSource = currentList;
        }
    }
}
