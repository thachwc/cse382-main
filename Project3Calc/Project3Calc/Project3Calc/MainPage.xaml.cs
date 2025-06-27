using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Project3Calc
{
    public partial class MainPage : ContentPage
    {
        int temp = 0;
        string operation = "";
        int memory = 0;
        int start = 0;
        int years = 0;
        double perc = 0.0;
        int investment = 0;
        int depositsPerYear = 12;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClicked(object sender, EventArgs e)
        {
            string button = (sender as Button).Text;
            switch (button)
            {
                case "MC":
                    memory = 0;
                    break;
                case "MR":
                    total.Text = memory.ToString();
                    break;
                case "M+":
                    memory += Int32.Parse(total.Text);
                    break;
                case "M-":
                    memory -= Int32.Parse(total.Text);
                    break;
                case "C":
                    total.Text = "0";
                    break;
                case "+/-":
                    if (!(total.Text.Equals("0")))
                    {
                        int neg = Int32.Parse(total.Text) * -1;
                        total.Text = neg.ToString();
                    }
                    break;
                case "+":
                case "-":
                case "/":
                case "X":
                    temp = Int32.Parse(total.Text);
                    total.Text = "0";
                    operation = button;
                    break;
                case "=":
                    if (operation == "")
                    {
                        break;
                    }
                    else
                    {
                        switch (operation)
                        {
                            case "+":
                                total.Text = (temp + Int32.Parse(total.Text)).ToString();
                                break;
                            case "-":
                                total.Text = (temp - Int32.Parse(total.Text)).ToString();
                                break;
                            case "X":
                                total.Text = (temp * Int32.Parse(total.Text)).ToString();
                                break;
                            case "/":
                                if (Int32.Parse(total.Text) == 0)
                                {
                                    total.Text = "ERROR";
                                }
                                else
                                {
                                    total.Text = (temp / Int32.Parse(total.Text)).ToString();
                                }
                                break;
                        }
                    }
                    break;
                case "START":
                    amount.Text = "$" + total.Text;
                    start = Int32.Parse(total.Text);
                    total.Text = "0";
                    break;
                case "YEARS":
                    year.Text = total.Text;
                    years = Int32.Parse(total.Text);
                    total.Text = "0";
                    break;
                case "RATE":
                    returnRate.Text = total.Text + "%";
                    perc = Double.Parse(total.Text) / 100;
                    total.Text = "0";
                    break;
                case "INVEST":
                    regular.Text = "$" + total.Text;
                    investment = Int32.Parse(total.Text);
                    total.Text = "0";
                    break;
                case "FREQ":
                    string result = await DisplayActionSheet("Frequency", "Cancel", null, "Monthly", "Quarterly", "Annually");
                    if (result.Equals("Monthly"))
                    {
                        depositsPerYear = 12;
                        frequency.Text = result;
                    }
                    if (result.Equals("Quarterly"))
                    {
                        depositsPerYear = 4;
                        frequency.Text = result;
                    }
                    if (result.Equals("Annually"))
                    {
                        depositsPerYear = 1;
                        frequency.Text = result;
                    }
                    break;
                case "FINAL":
                    final.Text = "$" + Compute(start, years, perc, investment, depositsPerYear).ToString();
                    break;
                default:
                    if (total.Text == "0")
                    {
                        total.Text = button;
                    }
                    else
                    {
                        total.Text = total.Text + button;
                    }
                    break;
            }
            if (!(total.Text.Equals("ERROR")) && Int32.Parse(total.Text) < 0)
            {
                iButton.IsEnabled = false;
                rButton.IsEnabled = false;
                yButton.IsEnabled = false;
                sButton.IsEnabled = false;
            }
            else
            {
                iButton.IsEnabled = true;
                rButton.IsEnabled = true;
                yButton.IsEnabled = true;
                sButton.IsEnabled = true;
            }
        }

        /*
         * start is the starting balance
         * years is the number of years that the investment will encompass
         * perc is the annual rate of return (e.g., 6% should sent in as 0.06)
         * investment is the amount of money added to the account on a regular basis
         * depositsPerYear is the number times per year (12, 4, or 1) that a deposit is made.
         */
        private int Compute(int start, int years, double perc, int investment, int depositsPerYear)
        {
            double bal = start;
            double monthlyRate = perc / 12.0;
            int monthsToDeposit = 12 / depositsPerYear;
            for (int y = 0; y < years; y++)
            {
                for (int m = 1; m <= 12; m++)
                {
                    bal += bal * monthlyRate;
                    if (m % monthsToDeposit == 0)
                    {
                        bal += investment;      // make deposits at the end of the month
                    }
                }
            }
            return (int)Math.Round(bal);
        }
    }
}
