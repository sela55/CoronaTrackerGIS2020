using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

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

            Content = new StackLayout
            {
                Children = {map}
            };
        }
    }
}
