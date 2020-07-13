using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using Map = Xamarin.Forms.Maps.Map;
using RestSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using System.IO;
using System.Reflection;
using System.Data;

namespace CoronaTracker
{
    public partial class Maps : ContentPage
    {
        Map map = new Map(MapSpan.FromCenterAndRadius(new Position(31.046051, 34.851612), Distance.FromKilometers(2000)));
     


        public List<details> lonLatLocationInfo = new List<details>();
        public class details
        {
            public string country;
            public double lon;
            public double lat;

            public details(string lat, string lon)
            {
                string temp = lon.Trim('\'');
                temp = temp.Trim('"');
                this.lon = Double.Parse(temp);
                string tempLat = lat.Trim('"');
                lat.Trim('\r');
                this.lat = Double.Parse(tempLat);

            }
        }
        public Maps()
        {
            InitializeComponent();

            IconImageSource = "maps.png";
            Title = "maps";
            var client = new RestClient("https://coronavirus-monitor.p.rapidapi.com/coronavirus/cases_by_country.php");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "coronavirus-monitor.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response = client.Execute(request);
            string restResponse = response.Content;
            var jsonObject = JObject.Parse(response.Content);
            Jcountries = JsonConvert.DeserializeObject<dynamic>(restResponse);


            foreach (var state in Jcountries.countries_stat)
            {

                countriesList.Add(state.country_name.ToString());


            }

            readFromCSVfile1();
            
            Content = new StackLayout
            {
                Children = { map }
            };



        }
        public ICommand getPositionCommand { get; }
        public dynamic Jcountries { get; }
        List<string> countriesList = new List<string>();
        List<string> countriesCases = new List<string>();

        

       
        List<string> numOfInfected = new List<string>();
        List<string> CountryString = new List<string>();
        public void readFromCSVfile1()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Maps)).Assembly;
            Stream stream1 = assembly.GetManifestResourceStream("CoronaTracker.Resources.countries2.csv");

            string text1 = null;
            using (var reader = new System.IO.StreamReader(stream1))
            {
                text1 = reader.ReadToEnd();
            }

            List<string> ls = new List<string>();
            ls.Add(text1.Replace('\n', ','));

            string[] fields = null;


            fields = ls[0].Split(',');

            string tempLat = "";
            string tempLon = "";
            string tempCountryName = "";

            for (int i = 0; i < fields.Length; i = i + 2)
            {
                tempLat = fields[i];
                tempLon = fields[i + 1];

                tempCountryName = fields[i + 2];
                i++;
                tempCountryName.Trim('\r');
                CountryString.Add(tempCountryName);

                details detail = new details(tempLat, tempLon);

                lonLatLocationInfo.Add(detail);

                foreach (var item in Jcountries.countries_stat)
                {
                    if (item.country_name.ToString() == tempCountryName.Trim('\r'))
                    {
                        Pin pin = new Pin
                        {
                            Label = item.country_name.ToString(),
                            Address = "Active corona cases: " + item.active_cases.ToString(),
                            Type = PinType.Place,
                            Position = new Position(detail.lat, detail.lon),
                        };
                        map.Pins.Add(pin);
                        int red = 255;
                        int green = 0;
                        string casesLevel = item.cases.ToString();
                        string deathLevel = item.deaths.ToString();
                        double deathNum = Double.Parse(deathLevel);
                        double Level = Double.Parse(casesLevel);
                        if (Level < 200000 )
                        {
                            red = 200;
                        }
                        else
                        {
                            red = 0;
                        }

                        if (deathNum < 15000)
                        {
                            green = 200;
                        }
                        else
                        {
                            green = 0;
                        }
                        Circle circle = new Circle
                        {

                            Center = new Position(detail.lat, detail.lon),
                            Radius = new Distance(75000),
                            StrokeColor = Color.FromRgba(0, green, 0, 0.5),
                            StrokeWidth = 11,
                            FillColor = Color.FromRgba(red, 0, 0,0.5),

                        };

                        map.MapElements.Add(circle);
                    }



                }


            }    
        }
    }
}
