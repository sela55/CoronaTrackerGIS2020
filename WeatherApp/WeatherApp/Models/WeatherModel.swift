import Foundation
import CoreLocation
import Combine

// MARK: - Weather Data Models

struct WeatherData: Codable {
    let main: MainWeather
    let weather: [WeatherDescription]
    let wind: Wind
    let name: String
    let sys: Sys
    let coord: Coordinates
}

struct MainWeather: Codable {
    let temp: Double
    let feelsLike: Double
    let tempMin: Double
    let tempMax: Double
    let pressure: Int
    let humidity: Int
    
    enum CodingKeys: String, CodingKey {
        case temp
        case feelsLike = "feels_like"
        case tempMin = "temp_min"
        case tempMax = "temp_max"
        case pressure
        case humidity
    }
}

struct WeatherDescription: Codable {
    let id: Int
    let main: String
    let description: String
    let icon: String
}

struct Wind: Codable {
    let speed: Double
    let deg: Int?
}

struct Sys: Codable {
    let country: String
    let sunrise: Int
    let sunset: Int
}

struct Coordinates: Codable {
    let lon: Double
    let lat: Double
}

// MARK: - City Search Models

struct CitySearchResult: Codable {
    let name: String
    let country: String
    let state: String?
    let lat: Double
    let lon: Double
}

// MARK: - Weather Model (Main Model Class)

class WeatherModel: ObservableObject {
    @Published var currentWeather: WeatherData?
    @Published var isLoading: Bool = false
    @Published var errorMessage: String?
    @Published var cities: [CitySearchResult] = []
    
    private let weatherService = WeatherService()
    private let citySearchService = CitySearchService()
    
    // MARK: - Public Methods
    
    func fetchWeather(for city: String) {
        isLoading = true
        errorMessage = nil
        
        weatherService.getWeather(for: city) { [weak self] result in
            DispatchQueue.main.async {
                self?.isLoading = false
                switch result {
                case .success(let weather):
                    self?.currentWeather = weather
                case .failure(let error):
                    self?.errorMessage = error.localizedDescription
                }
            }
        }
    }
    
    func fetchWeather(latitude: Double, longitude: Double) {
        isLoading = true
        errorMessage = nil
        
        weatherService.getWeather(latitude: latitude, longitude: longitude) { [weak self] result in
            DispatchQueue.main.async {
                self?.isLoading = false
                switch result {
                case .success(let weather):
                    self?.currentWeather = weather
                case .failure(let error):
                    self?.errorMessage = error.localizedDescription
                }
            }
        }
    }
    
    func searchCities(query: String) {
        guard !query.isEmpty else {
            cities = []
            return
        }
        
        citySearchService.searchCities(query: query) { [weak self] result in
            DispatchQueue.main.async {
                switch result {
                case .success(let cities):
                    self?.cities = cities
                case .failure:
                    self?.cities = []
                }
            }
        }
    }
    
    // MARK: - Helper Methods
    
    func temperatureString() -> String {
        guard let weather = currentWeather else { return "--°" }
        return "\(Int(weather.main.temp))°"
    }
    
    func weatherDescription() -> String {
        guard let weather = currentWeather else { return "No weather data" }
        return weather.weather.first?.description.capitalized ?? "Unknown"
    }
    
    func cityName() -> String {
        guard let weather = currentWeather else { return "Unknown Location" }
        return "\(weather.name), \(weather.sys.country)"
    }
    
    func weatherIconName() -> String {
        guard let weather = currentWeather,
              let weatherInfo = weather.weather.first else { return "sun.max" }
        
        switch weatherInfo.main.lowercased() {
        case "clear":
            return "sun.max"
        case "clouds":
            return "cloud"
        case "rain", "drizzle":
            return "cloud.rain"
        case "thunderstorm":
            return "cloud.bolt.rain"
        case "snow":
            return "cloud.snow"
        case "mist", "fog", "haze":
            return "cloud.fog"
        default:
            return "sun.max"
        }
    }
    
    func windSpeed() -> String {
        guard let weather = currentWeather else { return "0 m/s" }
        return "\(Int(weather.wind.speed)) m/s"
    }
    
    func humidity() -> String {
        guard let weather = currentWeather else { return "0%" }
        return "\(weather.main.humidity)%"
    }
    
    func pressure() -> String {
        guard let weather = currentWeather else { return "0 hPa" }
        return "\(weather.main.pressure) hPa"
    }
    
    func feelsLike() -> String {
        guard let weather = currentWeather else { return "--°" }
        return "\(Int(weather.main.feelsLike))°"
    }
}

// MARK: - Extensions

extension WeatherData {
    var isDay: Bool {
        let currentTime = Date().timeIntervalSince1970
        return currentTime >= Double(sys.sunrise) && currentTime < Double(sys.sunset)
    }
}