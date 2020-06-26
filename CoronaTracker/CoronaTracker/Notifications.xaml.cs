using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
            
            //string check1 = jsonObject.GetValue();

            deserialiseJSON(restResponse);
            var Jcountries = JsonConvert.DeserializeObject<dynamic>(restResponse);
            //string selectedCountry = null;
   

            //now we are taking to countries list into a list to display it in the picker
            List<string> countriesList = new List<string>();
            foreach (var state in Jcountries.countries_stat)
            {
                countriesList.Add(state.country_name.ToString());
            }
            //string temproryVar = Jcountries.countries_stat.country_name;




            IconImageSource = "notifications.png";
            Title = "notifications";

            Picker countryPicker = new Picker()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Title = "Search for a Country",
                TitleColor = Color.Red,
                ItemsSource = countriesList,
                //SelectedItem = selectedCountry,
                

                

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
                TextColor = Color.DarkRed,
                IsVisible = false
       
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
                TextColor = Color.Black,
                IsVisible = false
            };



            Content = new StackLayout
            {
                Children = {
                        new Label {
                        Text = "here we will bulid the picker and also search label",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.CenterAndExpand
                        


                    }, countryPicker,
                        totalConfirmedLabel,
                        numOfTotalConfirmedLabel,
                        totalConfirmedDeathsLabel,
                        numOfTotalConfirmedDeathsLabel
                }
            };

            void DebugOutput(string strDebugText)
            {
                try
                {
                    System.Diagnostics.Debug.Write(strDebugText + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message.ToString() + Environment.NewLine);
                }
            }

            void deserialiseJSON(string strJSON)
            {
                try
                {
                    var icountries = JsonConvert.DeserializeObject<Comments>(strJSON);
                    DebugOutput("heres our JSON object : " + icountries.ToString());
                    System.Console.WriteLine("JIMJIMJIMJIMJIMJIM" + icountries.countries_stat.country_name);
                  

                }
                catch (Exception ex)
                {

                }
            }
            countryPicker.SelectedIndexChanged += (sender, args) =>
            {
                //System.Console.WriteLine("Hello:");
                if (countryPicker.SelectedIndex == -1)
                {

                }
                else
                {
                    string countryString = countryPicker.Items[countryPicker.SelectedIndex].ToString();
                    CheckingStatusaboutSpecificCountry(countryString); 
                }
            };


            void CheckingStatusaboutSpecificCountry(string countryName)
            {
                if (countryName != null)
                {
                    var client1 = new RestClient("https://coronavirus-monitor.p.rapidapi.com/coronavirus/latest_stat_by_country.php?country=" + countryName);
                    var request1 = new RestRequest(Method.GET);
                    request1.AddHeader("x-rapidapi-host", "coronavirus-monitor.p.rapidapi.com");
                    request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
                    IRestResponse response1 = client1.Execute(request1);

                    string restResponse1 = response1.Content;
                    var jsonObject1 = JObject.Parse(response1.Content);

                    //deserialiseJSON(restResponse1);
                    //var Jcountries1 = JsonConvert.DeserializeObject<dynamic>(restResponse1);


                    TurnOnTheLabels(countryName, jsonObject1);

                }
            }

            void TurnOnTheLabels(string countryName, JObject jsonObject1)
            {
                JArray array = (JArray)jsonObject1["latest_stat_by_country"];
                //string numOfSick = array.Value<string>("total_cases").ToString();
                string numOfSick = array[0]["total_cases"].ToString();
                numOfTotalConfirmedLabel.Text = numOfSick;
                numOfTotalConfirmedLabel.IsVisible = true;

                string numOfdeaths = array[0]["total_deaths"].ToString();
                numOfTotalConfirmedDeathsLabel.Text = numOfdeaths;
                numOfTotalConfirmedDeathsLabel.IsVisible = true;


            }
            }

        

        //void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var picker = (Picker)sender;
        //    int selectedIndex = picker.SelectedIndex;

        //    if (selectedIndex != -1)
        //    {
        //        monkeyNameLabel.Text = (string)picker.ItemsSource[selectedIndex];
        //    }
        //}
    }
        

        

    

    

    public class Comments
    {
        public Countries_Stat countries_stat { get; set; }
        public class Countries_Stat
        {

            public string country_name { get; set; }
            public int cases { get; set; }
            public int deaths { get; set; }
            public int total_recovered { get; set; }
        }
    }

}

