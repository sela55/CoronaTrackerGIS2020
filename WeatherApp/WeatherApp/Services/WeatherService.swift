import Foundation
import CoreLocation

class WeatherService {
    
    // Note: You'll need to get your own API key from https://openweathermap.org/api
    private let apiKey = "YOUR_API_KEY_HERE"
    private let baseURL = "https://api.openweathermap.org/data/2.5/weather"
    
    enum WeatherError: Error, LocalizedError {
        case invalidURL
        case noData
        case decodingError
        case networkError(String)
        case invalidAPIKey
        
        var errorDescription: String? {
            switch self {
            case .invalidURL:
                return "Invalid URL"
            case .noData:
                return "No data received"
            case .decodingError:
                return "Failed to decode weather data"
            case .networkError(let message):
                return "Network error: \(message)"
            case .invalidAPIKey:
                return "Invalid API key. Please get your API key from OpenWeatherMap."
            }
        }
    }
    
    // MARK: - Public Methods
    
    func getWeather(for city: String, completion: @escaping (Result<WeatherData, WeatherError>) -> Void) {
        guard apiKey != "YOUR_API_KEY_HERE" else {
            completion(.failure(.invalidAPIKey))
            return
        }
        
        let encodedCity = city.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? city
        let urlString = "\(baseURL)?q=\(encodedCity)&appid=\(apiKey)&units=metric"
        
        performRequest(urlString: urlString, completion: completion)
    }
    
    func getWeather(latitude: Double, longitude: Double, completion: @escaping (Result<WeatherData, WeatherError>) -> Void) {
        guard apiKey != "YOUR_API_KEY_HERE" else {
            completion(.failure(.invalidAPIKey))
            return
        }
        
        let urlString = "\(baseURL)?lat=\(latitude)&lon=\(longitude)&appid=\(apiKey)&units=metric"
        
        performRequest(urlString: urlString, completion: completion)
    }
    
    // MARK: - Private Methods
    
    private func performRequest(urlString: String, completion: @escaping (Result<WeatherData, WeatherError>) -> Void) {
        guard let url = URL(string: urlString) else {
            completion(.failure(.invalidURL))
            return
        }
        
        let task = URLSession.shared.dataTask(with: url) { data, response, error in
            if let error = error {
                completion(.failure(.networkError(error.localizedDescription)))
                return
            }
            
            guard let data = data else {
                completion(.failure(.noData))
                return
            }
            
            // Check for HTTP errors
            if let httpResponse = response as? HTTPURLResponse {
                switch httpResponse.statusCode {
                case 401:
                    completion(.failure(.invalidAPIKey))
                    return
                case 200...299:
                    break
                default:
                    completion(.failure(.networkError("HTTP \(httpResponse.statusCode)")))
                    return
                }
            }
            
            do {
                let weatherData = try JSONDecoder().decode(WeatherData.self, from: data)
                completion(.success(weatherData))
            } catch {
                print("Decoding error: \(error)")
                completion(.failure(.decodingError))
            }
        }
        
        task.resume()
    }
}

// MARK: - Mock Service for Testing

class MockWeatherService: WeatherService {
    
    override func getWeather(for city: String, completion: @escaping (Result<WeatherData, WeatherError>) -> Void) {
        // Simulate network delay
        DispatchQueue.global().asyncAfter(deadline: .now() + 1.0) {
            let mockWeather = WeatherData(
                main: MainWeather(
                    temp: 22.5,
                    feelsLike: 24.0,
                    tempMin: 20.0,
                    tempMax: 25.0,
                    pressure: 1013,
                    humidity: 65
                ),
                weather: [WeatherDescription(
                    id: 800,
                    main: "Clear",
                    description: "clear sky",
                    icon: "01d"
                )],
                wind: Wind(speed: 3.5, deg: 180),
                name: city,
                sys: Sys(
                    country: "US",
                    sunrise: 1640678400,
                    sunset: 1640713200
                ),
                coord: Coordinates(lon: -122.4, lat: 37.7)
            )
            
            completion(.success(mockWeather))
        }
    }
    
    override func getWeather(latitude: Double, longitude: Double, completion: @escaping (Result<WeatherData, WeatherError>) -> Void) {
        getWeather(for: "Current Location", completion: completion)
    }
}