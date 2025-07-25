# Weather App

A beautiful iOS weather application built with Swift using the MVC (Model-View-Controller) architecture pattern. Features stunning weather animations and city search with autocomplete functionality.

## Features

🌤️ **Beautiful Weather Animations**
- Dynamic animations based on weather conditions (sun rays, rain drops, snow flakes, clouds, thunderstorm, fog)
- Smooth transitions and realistic particle effects
- Weather-based background gradients

🔍 **Smart City Search**
- Real-time city autocomplete using OpenWeatherMap Geocoding API
- Search by city name, state, or country
- Dropdown suggestions with formatted location names

📱 **Modern UI/UX**
- Clean, intuitive interface with iOS design guidelines
- Gradient backgrounds that change based on weather conditions
- Large, easy-to-read temperature display
- Detailed weather information (feels like, humidity, wind speed, pressure)

📍 **Location Services**
- Current location weather detection
- Location permission handling with user-friendly prompts

🏗️ **MVC Architecture**
- **Model**: `WeatherModel`, `WeatherData`, `CitySearchResult`
- **View**: `WeatherAnimationView`, Storyboard UI components
- **Controller**: `WeatherViewController` with proper separation of concerns

## Requirements

- iOS 17.0+
- Xcode 15.0+
- Swift 5.0+
- OpenWeatherMap API key

## Setup Instructions

### 1. Get OpenWeatherMap API Key

1. Visit [OpenWeatherMap](https://openweathermap.org/api)
2. Sign up for a free account
3. Generate an API key

### 2. Configure API Key

Replace `YOUR_API_KEY_HERE` in the following files with your actual API key:

- `WeatherApp/Services/WeatherService.swift` (line 10)
- `WeatherApp/Services/CitySearchService.swift` (line 6)

```swift
private let apiKey = "your_actual_api_key_here"
```

### 3. Build and Run

1. Open `WeatherApp.xcodeproj` in Xcode
2. Select your target device or simulator
3. Build and run (⌘+R)

## Project Structure

```
WeatherApp/
├── WeatherApp/
│   ├── Controllers/
│   │   └── WeatherViewController.swift      # Main view controller
│   ├── Models/
│   │   └── WeatherModel.swift              # Weather data models and business logic
│   ├── Views/
│   │   └── WeatherAnimationView.swift      # Custom weather animations
│   ├── Services/
│   │   ├── WeatherService.swift            # OpenWeatherMap API client
│   │   └── CitySearchService.swift         # City search/geocoding service
│   ├── Base.lproj/
│   │   ├── Main.storyboard                 # Main UI layout
│   │   └── LaunchScreen.storyboard         # Launch screen
│   ├── Assets.xcassets/                    # App icons and assets
│   ├── Info.plist                          # App configuration
│   ├── AppDelegate.swift                   # App lifecycle
│   └── SceneDelegate.swift                 # Scene lifecycle
└── WeatherApp.xcodeproj                    # Xcode project file
```

## MVC Architecture Details

### Model Layer
- **WeatherModel**: Main business logic coordinator
- **WeatherData**: Core weather data structure
- **CitySearchResult**: City search result representation
- **WeatherService**: API communication for weather data
- **CitySearchService**: API communication for city search

### View Layer
- **WeatherAnimationView**: Custom UIView with Core Animation
- **Main.storyboard**: Interface layout and constraints
- **UI Components**: Labels, text fields, table views, buttons

### Controller Layer
- **WeatherViewController**: Coordinates between Model and View
- **Delegates**: Text field, table view, and location delegates
- **Event Handling**: User interactions and API responses

## Key Features Implementation

### Weather Animations
The `WeatherAnimationView` uses Core Animation to create dynamic weather effects:
- **Sun**: Rotating rays with floating particles
- **Rain**: Falling droplets with cloud movement
- **Snow**: Swaying snowflakes with rotation
- **Clouds**: Moving cloud formations
- **Thunderstorm**: Lightning flashes with rain
- **Fog**: Drifting fog layers

### City Autocomplete
Real-time search implementation:
- Debounced text input (500ms delay)
- OpenWeatherMap Geocoding API integration
- Dropdown table view with formatted results
- Tap-to-select functionality

### Location Services
Comprehensive location handling:
- Permission request management
- Current location weather fetching
- Error handling with user-friendly messages
- Settings redirection for denied permissions

## API Usage

### Weather Data
```
GET https://api.openweathermap.org/data/2.5/weather
Parameters:
- q: city name
- lat/lon: coordinates
- appid: API key
- units: metric
```

### City Search
```
GET https://api.openweathermap.org/geo/1.0/direct
Parameters:
- q: search query
- limit: result count
- appid: API key
```

## Customization

### Adding New Weather Types
1. Add case to `WeatherAnimationView.WeatherType`
2. Implement animation method
3. Add case to `setWeatherType(from:isDay:)`
4. Create animation layers and add to `animationLayers`

### Styling
- Modify gradients in `setupGradientBackground()`
- Update colors in `updateBackgroundGradient(for:)`
- Customize fonts and sizes in `setupUI()`

## Testing

The app includes mock services for testing without API keys:
- `MockWeatherService`: Returns sample weather data
- `MockCitySearchService`: Returns predefined city list

To use mock services, replace the service instances in `WeatherModel`.

## Troubleshooting

### Common Issues

1. **API Key Error**: Ensure your API key is correctly set and valid
2. **Location Permission**: Check location services are enabled
3. **Network Issues**: Verify internet connection and API endpoints
4. **Build Errors**: Ensure iOS 17.0+ deployment target

### Debug Mode
- Check console logs for API responses
- Verify location services permissions
- Test with mock services to isolate API issues

## Contributing

1. Fork the repository
2. Create a feature branch
3. Implement your changes following MVC pattern
4. Add appropriate error handling
5. Test thoroughly
6. Submit a pull request

## License

This project is open source and available under the MIT License.

## Acknowledgments

- OpenWeatherMap for weather data API
- Apple's Core Animation framework for animations
- iOS design guidelines for UI/UX patterns