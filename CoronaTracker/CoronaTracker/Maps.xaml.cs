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



            //List<Tuple<string, string>> countriesCoords = new List<Tuple<string, string>>();


            readFromCSVfile1();
            //Console.WriteLine(lonLatLocationInfo.Count);
            //foreach(var value in lonLatLocationInfo)
            //{
            //    Circle circle = new Circle
            //    {

            //        Center = new Position(value.lat, value.lon),
            //        Radius = new Distance(25000),
            //        StrokeColor = Color.FromHex("#88FF0000"),
            //        StrokeWidth = 8,
            //        FillColor = Color.FromRgb(255, 0, 0),
            //       // FillColor = Color.FromHex("#88FFC0CB"),

            //    };

            //    map.MapElements.Add(circle);


            //}


            Content = new StackLayout
            {
                Children = { map }
            };



        }
        public ICommand getPositionCommand { get; }
        public dynamic Jcountries { get; }
        List<string> countriesList = new List<string>();
        List<string> countriesCases = new List<string>();

        private async void test(string state)
        {
            var client1 = new RestClient("https://opencage-geocoder.p.rapidapi.com/geocode/v1/json?language=en&key=fd7ac82c32be42289fe41e2c8a777861&q=" + state);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("x-rapidapi-host", "opencage-geocoder.p.rapidapi.com");
            request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response1 = client1.Execute(request1);


            string restResponse1 = response1.Content;
            var jsonObject1 = JObject.Parse(response1.Content);

            var jCoords = JsonConvert.DeserializeObject<dynamic>(restResponse1);
            double lat = jCoords.results[0].geometry["lat"];
            double lon = jCoords.results[0].geometry["lng"];

            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
           {
               Circle circle = new Circle
               {

                   Center = new Position(lat, lon),
                   Radius = new Distance(25000),
                   StrokeColor = Color.FromHex("#88FF0000"),
                   StrokeWidth = 8,
                   FillColor = Color.FromHex("#88FFC0CB"),


               };

               map.MapElements.Add(circle);
           });
            });
        }

        private void test2(string state)
        {
            var client1 = new RestClient("https://opencage-geocoder.p.rapidapi.com/geocode/v1/json?language=en&key=fd7ac82c32be42289fe41e2c8a777861&q=" + state);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("x-rapidapi-host", "opencage-geocoder.p.rapidapi.com");
            request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response1 = client1.Execute(request1);
            string restResponse1 = response1.Content;
            var jsonObject1 = JObject.Parse(response1.Content);

            var jCoords = JsonConvert.DeserializeObject<dynamic>(restResponse1);
            double lat = jCoords.results[0].geometry["lat"];
            double lon = jCoords.results[0].geometry["lng"];




            Circle circle = new Circle
            {
                Center = new Position(lat, lon),
                Radius = new Distance(25000),
                StrokeColor = Color.FromHex("#88FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromHex("#88FFC0CB"),

            };
            map.MapElements.Add(circle);

        }

        List<string> numOfInfected = new List<string>();
        List<string> CountryString = new List<string>();
        public void readFromCSVfile1()

        {



            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Maps)).Assembly;
            //Stream stream1 = assembly.GetManifestResourceStream("CoronaTracker.Resources.countries.csv");
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

            //int j = 0;
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
                        //string casesLevel = item.cases.ToInt();
                        string casesLevel = item.cases.ToString();
                        //int Level = int.Parse(casesLevel);
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
                            // FillColor = Color.FromHex("#88FFC0CB"),

                        };

                        map.MapElements.Add(circle);
                    }



                }


            }
            //foreach (var value in lonLatLocationInfo)
            //{
            //    Circle circle = new Circle
            //    {

            //        Center = new Position(value.lat, value.lon),
            //        Radius = new Distance(25000),
            //        StrokeColor = Color.FromHex("#88FF0000"),
            //        StrokeWidth = 8,
            //        FillColor = Color.FromRgb(255, 0, 0),
            //        // FillColor = Color.FromHex("#88FFC0CB"),

            //    };

            //    map.MapElements.Add(circle);


            //}

            String[] countriesString = new String[] { "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegowina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic",
            "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, the Democratic Republic of the", "Cook Islands", "Costa Rica", "Cote d'Ivoire", "Croatia (Hrvatska)", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "France Metropolitan", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland",
            "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard and Mc Donald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran (Islamic Republic of)", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Kyrgyzstan", "Lao, People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia, The Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico",
            "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico",
            "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia (Slovak Republic)", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "Spain",
            "Sri Lanka", "St. Helena", "St. Pierre and Miquelon", "Sudan", "Suriname", "Svalbard and Jan Mayen Islands", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Vietnam", "Virgin Islands (British)", "Virgin Islands (U.S.)", "Wallis and Futuna Islands", "Western Sahara", "Yemen", "Yugoslavia", "Zambia", "Zimbabwe" };
        }
    }
}
