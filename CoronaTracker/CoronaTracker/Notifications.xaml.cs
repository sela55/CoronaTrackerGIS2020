using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xamarin.Forms;

namespace CoronaTracker
{
    public partial class Notifications : ContentPage
    {
        //[Obsolete]
        public Notifications()
        {
            InitializeComponent();

            var client = new RestClient("https://coronavirus-monitor.p.rapidapi.com/coronavirus/cases_by_country.php");
            var request = new RestRequest(Method.GET);
            //request.RootElement = "country_name";
            request.AddHeader("x-rapidapi-host", "coronavirus-monitor.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response = client.Execute(request);

            string restResponse = response.Content;
            var jsonObject = JObject.Parse(response.Content);



            IconImageSource = "notifications.png";
            Title = "notifications";

            Picker countryPicker = new Picker()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Title = "Search for a Country",
                

            };

            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "here we will bulid the picker and also search label",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand


                    }, countryPicker
                }
            };
        }
    }
}
