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

namespace CoronaTracker
{
    public partial class Maps : ContentPage
    {
        public Maps()
        {
            InitializeComponent();

            IconImageSource = "maps.png";
            Title = "maps";

            Map map = new Map();

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
            foreach (var state in countriesList)
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


            }

            List<Tuple<string,string>> countriesCoords = new List<Tuple<string,string>>();

           
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
          


            Content = new StackLayout
            {
                Children = {map}
            };

           

        }
        public ICommand getPositionCommand { get; }
        public dynamic Jcountries { get; }
        List<string> countriesList = new List<string>();

        
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

        //Task IRestResponse response()
        //{
        //    var client1 = new RestClient("https://opencage-geocoder.p.rapidapi.com/geocode/v1/json?language=en&key=fd7ac82c32be42289fe41e2c8a777861&q=" + state);
        //    var request1 = new RestRequest(Method.GET);
        //    request1.AddHeader("x-rapidapi-host", "opencage-geocoder.p.rapidapi.com");
        //    request1.AddHeader("x-rapidapi-key", "1e9bfb544dmsh018403b9672dd9ap1b7dfajsncc70eed878e3");
        //    IRestResponse response1 = client1.Execute(request1);
        //    return response1;

        //}

        //private void deserialiseJSON(string restResponse)
        //{
        //    throw new NotImplementedException();
        //}

        // void deserialiseJSON(string restResponse)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
