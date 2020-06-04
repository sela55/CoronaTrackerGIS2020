using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Xamarin.Forms;

namespace CoronaTracker
{
    public partial class MainInfo : ContentPage
    {
        public MainInfo()
        {
            //DESIGN SECTION - I WROTE IT BY CODE AND NOT BY THE REGULAR METHOD VIA THE XAML OPTION.
            IconImageSource = "baseline_home_black_18dp.png";
            Title = "Main Info";

            Label titleLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Title, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                Text = "Corona COVID 19 " +
                    "Global Cases",
                
            };

            Label totalConfirmedLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.None,
                Text = "Total Confirmed",
                TextColor = Color.DarkGray
            };

            Label numOfTotalConfirmedLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                Text = "Total Confirmed Number",
                TextColor = Color.DarkRed
            };

            Label totalConfirmedDeathsLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.None,
                Text = "Total deaths",
                TextColor = Color.DarkGray
            };

            Label numOfTotalConfirmedDeathsLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                Text = "Total Confirmed",
                TextColor = Color.Black
            };

            Label totalRecoveredLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)),
                FontAttributes = FontAttributes.None,
                Text = "Total Recovered",
                TextColor = Color.Green
            };

            Label numOfTotalConfirmedRecoveredLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
                Text = "Total Confirmed",
                TextColor = Color.Green
            };

           

            Content = new StackLayout
            {
                Spacing = 2,

                Children = {
                  titleLabel,
                  totalConfirmedLabel,
                  numOfTotalConfirmedLabel,
                  totalConfirmedDeathsLabel,
                  numOfTotalConfirmedDeathsLabel,
                  totalRecoveredLabel,
                  numOfTotalConfirmedRecoveredLabel
                },
            
          
        };
            var client = new RestClient("https://coronavirus-monitor.p.rapidapi.com/coronavirus/worldstat.php");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "coronavirus-monitor.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response = client.Execute(request);

            string restResponse = response.Content;
            var jsonObject = JObject.Parse(response.Content);

            string numOfSick = jsonObject.GetValue("total_cases").ToString();
            numOfTotalConfirmedLabel.Text = numOfSick;

            string numOfDeaths = jsonObject.GetValue("total_deaths").ToString();
            numOfTotalConfirmedDeathsLabel.Text = numOfDeaths;

            string numOfRecovered = jsonObject.GetValue("total_recovered").ToString();
            numOfTotalConfirmedRecoveredLabel.Text = numOfRecovered;



        }

    }
}
        
    

