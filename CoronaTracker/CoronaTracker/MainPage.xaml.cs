using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoronaTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            

            InitializeComponent();

            var navigationPage = new NavigationPage(new MainInfo());
            navigationPage.Title = "temporary label";

            

            Children.Add(new MainInfo());
            Children.Add(new Maps());
            Children.Add(new Notifications());



        }

        private void DisplayAlert(string v1, IRestResponse response, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
