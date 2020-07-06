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
        Map map = new Map();
        public List<details> lonLatLocationInfo = new List<details>();
        public class details
        {
            public string country;
            public double lon;
            public double lat;

            public details(string lat,string lon)
            {
                string temp = lon.Trim('\'');
                temp = temp.Trim('"');
                this.lon = Double.Parse(temp);
                string tempLat = lat.Trim('"');
                this.lat = Double.Parse(tempLat);

            }
/*            public static details FromCsv(string csvLine)
            {
                //char t = '\'';
                //csvLine.Trim(t);
                string[] values = csvLine.Split(',');
                //string values = csvLine;
                //.Trim(t);
                details detail = new details();
                detail.country = Convert.ToString(values[4]);
                //detail.lon = Convert.ToString(values[3]);
                detail.lon = Double.Parse(values[3].Trim('\'','"'));
                detail.lat = Double.Parse(values[2].Trim('\'','"'));
                //detail.lat = Convert.ToString(values[2]);

                return detail;
            }*/
        }
        public Maps()
        {
            InitializeComponent();

            IconImageSource = "maps.png";
            Title = "maps";
            //var list = new List<details>();

            // ...


            //Geocoder geoCoder = new Geocoder();

            //IEnumerable<Position> approximateLocations = geoCoder.GetPositionsForAddressAsync("Pacific Ave, San Francisco, California");
            //Position position = approximateLocations.FirstOrDefault();
            //string coordinates = $"{position.Latitude}, {position.Longitude}";

            var client = new RestClient("https://coronavirus-monitor.p.rapidapi.com/coronavirus/cases_by_country.php");
            var request = new RestRequest(Method.GET);
            //request.RootElement = "country_name";
            request.AddHeader("x-rapidapi-host", "coronavirus-monitor.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
            IRestResponse response = client.Execute(request);
            string restResponse = response.Content;
            var jsonObject = JObject.Parse(response.Content);
            //deserialiseJSON(restResponse);
            Jcountries = JsonConvert.DeserializeObject<dynamic>(restResponse);

            foreach (var state in Jcountries.countries_stat)
            {

                countriesList.Add(state.country_name.ToString());
                System.Console.WriteLine(state.country_name.ToString());
            }

            //var PositionCommand = new Command(async () => await getPositionCommand2());

            //getPositionCommand = new Command(async () => await OnGetPosition());

            // the following API is meant for latitdude and longitude countries, later we will use them to drop pins in the map
            ////////////////////
            //Parallel.ForEach(countriesList, new ParallelOptions { MaxDegreeOfParallelism = 150 },
            //state =>
            //{
            //    // logic
            //    test2(state);
            //});

            /*  foreach (var state in countriesList)
              {
                  var client1 = new RestClient("https://opencage-geocoder.p.rapidapi.com/geocode/v1/json?language=en&key=fd7ac82c32be42289fe41e2c8a777861&q=" + state);
                  var request1 = new RestRequest(Method.GET);
                  request1.AddHeader("x-rapidapi-host", "opencage-geocoder.p.rapidapi.com");
                  request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
                  IRestResponse response1 = client1.Execute(request1);

                  //var client1 = new RestClient("https://geoapify-platform.p.rapidapi.com/v1/geocode/search?lang=en&lat=40.74&lon=-73.98&limit=1&apiKey=325e50d4bff6405289831fb983b4523c&text=" + state);
                  //var request1 = new RestRequest(Method.GET);
                  //request1.AddHeader("x-rapidapi-host", "geoapify-platform.p.rapidapi.com");
                  //request1.AddHeader("x-rapidapi-key", "d3e375452c3f4121b36bc9bdbc52f186");
                  //IRestResponse response1 = client.Execute(request1);

                  string restResponse1 = response1.Content;
                  var jsonObject1 = JObject.Parse(response1.Content);

                  //deserialiseJSON(restResponse);
                  var jCoords = JsonConvert.DeserializeObject<dynamic>(restResponse1);
                  //System.Console.WriteLine("the geocoding for the state " + state + " are: \n" + jCoords.results[0].geometry);
                  double lat = jCoords.results[0].geometry["lat"];
                  double lon = jCoords.results[0].geometry["lng"];
                  //Pin pin = new Pin
                  //{
                  //    Label = "1000",
                  //    //Address = "The city with a boardwalk",
                  //    Type = PinType.Place,
                  //    Position = new Position(lat, lon)
                  //};


                  //map.MapElements.Add(circle);
                  //map.Pins.Add(pin);

                  Circle circle = new Circle
                  {

                      Center = new Position(lat, lon),
                      Radius = new Distance(25000),
                      StrokeColor = Color.FromHex("#88FF0000"),
                      StrokeWidth = 8,
                      FillColor = Color.FromHex("#88FFC0CB"),

                  };
                  map.MapElements.Add(circle);


              }*/

            List<Tuple<string, string>> countriesCoords = new List<Tuple<string, string>>();


            ///////////////////


            //Circle circle = new Circle
            //{

            //    Center = new Position(46.82, 8.23),
            //    Radius = new Distance(25000),
            //    StrokeColor = Color.FromHex("#88FF0000"),
            //    StrokeWidth = 8,
            //    FillColor = Color.FromHex("#88FFC0CB"),

            //};



            //Pin pin = new Pin
            //{
            //    Label = "1000",
            //    //Address = "The city with a boardwalk",
            //    Type = PinType.Place,
            //    Position = new Position(46.82, 8.23)
            //};


            //map.MapElements.Add(circle);
            //map.Pins.Add(pin);

            //readFromCSVfile1();
            //            List<details> values = File.ReadAllLines("/Users/sela/Desktop/CoronaTracker/CoronaTracker/CoronaTracker/Resources/worldcities.csv")
            //                                         .Skip(1)
            //                                       .Select(v => details.FromCsv(v))
            //                                     .ToList();
            readFromCSVfile1();
            Console.WriteLine(lonLatLocationInfo.Count);
            foreach(var value in lonLatLocationInfo)
            {
                //Console.WriteLine("longitute: " + value.lon + " lat:" + value.lat);
                //double lat = Double.Parse(value.lat);
                //double lon = Double.Parse(value.lon);

                //double lat = 5.5;
                //double lon = 3.5;
                Circle circle = new Circle
                {

                    Center = new Position(value.lat, value.lon),
                    Radius = new Distance(25000),
                    StrokeColor = Color.FromHex("#88FF0000"),
                    StrokeWidth = 8,
                    FillColor = Color.FromHex("#88FFC0CB"),

                };
                map.MapElements.Add(circle);
            }


            Content = new StackLayout
            {
                Children = { map }
            };



        }
        public ICommand getPositionCommand { get; }
        public dynamic Jcountries { get; }
        List<string> countriesList = new List<string>();

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

            await Task.Run(async () =>
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

            //deserialiseJSON(restResponse);
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
        //async Task getPositionCommand2()
        //{

        //            foreach (var state in countriesList)
        //            {
        //                var client1 = new RestClient("https://opencage-geocoder.p.rapidapi.com/geocode/v1/json?language=en&key=fd7ac82c32be42289fe41e2c8a777861&q=" + state);
        //        var request1 = new RestRequest(Method.GET);
        //        request1.AddHeader("x-rapidapi-host", "opencage-geocoder.p.rapidapi.com");
        //                request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
        //                IRestResponse response1 = client1.Execute(request1);
        //        var result = response1;

        //        string restResponse1 = response1.Content;
        //        var jsonObject1 = JObject.Parse(response1.Content);

        //        //deserialiseJSON(restResponse);
        //        var jCoords = JsonConvert.DeserializeObject<dynamic>(restResponse1);
        //        System.Console.WriteLine("the geocoding for the state " + state + " are: \n" + jCoords.results[0].geometry);

        //            }
        //}

        public void readFromCSVfile1()
        //{
        //    List<details> values = File.ReadAllLines("/Users/sela/Desktop/CoronaTracker/CoronaTracker/CoronaTracker/Resources/worldcities.csv")
        //                                   .Skip(1)
        //                                   .Select(v => details.FromCsv(v))
        //                                   .ToList();

        {

            //    return values;
            string[] lines = System.IO.File.ReadAllLines("/Users/sela/Desktop/CoronaTracker/CoronaTracker/CoronaTracker/Resources/worldcities.csv");
                string[] lines2 = System.IO.File.ReadAllLines("/Users/sela/Desktop/CoronaTracker/CoronaTracker/CoronaTracker/Resources/countries.csv");
         /*   for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                string countryName = fields[4].Trim('"');
                if (countriesString.Contains(countryName))
                {
                    details detail = new details(fields[2], fields[3]);
                    lonLatLocationInfo.Add(detail);
                }

                */
            for (int i = 1; i<lines2.Length ; i++)
            {
                //string country = countriesString[i];

                string[] fields = lines2[i].Split(',');

                string countryName = fields[2].Trim('"');
                if (countriesString.Contains(countryName))
                {
                    details detail = new details(fields[0], fields[1]);
                    lonLatLocationInfo.Add(detail);
                }
                // System.Console.WriteLine(fields[2]);
                // System.Console.WriteLine(fields[3]);
            }
        }

        String[] countriesString = new String[] { "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegowina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic",
            "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, the Democratic Republic of the", "Cook Islands", "Costa Rica", "Cote d'Ivoire", "Croatia (Hrvatska)", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "France Metropolitan", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland",
            "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard and Mc Donald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran (Islamic Republic of)", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Kyrgyzstan", "Lao, People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia, The Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico",
            "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico",
            "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia (Slovak Republic)", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "Spain",
            "Sri Lanka", "St. Helena", "St. Pierre and Miquelon", "Sudan", "Suriname", "Svalbard and Jan Mayen Islands", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Vietnam", "Virgin Islands (British)", "Virgin Islands (U.S.)", "Wallis and Futuna Islands", "Western Sahara", "Yemen", "Yugoslavia", "Zambia", "Zimbabwe" };
    }
}
