import Foundation

class CitySearchService {
    
    // Note: You'll need to get your own API key from https://openweathermap.org/api
    private let apiKey = "YOUR_API_KEY_HERE"
    private let geocodingURL = "https://api.openweathermap.org/geo/1.0/direct"
    
    enum CitySearchError: Error, LocalizedError {
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
                return "Failed to decode city data"
            case .networkError(let message):
                return "Network error: \(message)"
            case .invalidAPIKey:
                return "Invalid API key. Please get your API key from OpenWeatherMap."
            }
        }
    }
    
    // MARK: - Public Methods
    
    func searchCities(query: String, completion: @escaping (Result<[CitySearchResult], CitySearchError>) -> Void) {
        guard apiKey != "YOUR_API_KEY_HERE" else {
            completion(.failure(.invalidAPIKey))
            return
        }
        
        guard !query.isEmpty else {
            completion(.success([]))
            return
        }
        
        let encodedQuery = query.addingPercentEncoding(withAllowedCharacters: .urlQueryAllowed) ?? query
        let urlString = "\(geocodingURL)?q=\(encodedQuery)&limit=5&appid=\(apiKey)"
        
        performRequest(urlString: urlString, completion: completion)
    }
    
    // MARK: - Private Methods
    
    private func performRequest(urlString: String, completion: @escaping (Result<[CitySearchResult], CitySearchError>) -> Void) {
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
                let cities = try JSONDecoder().decode([CitySearchResult].self, from: data)
                completion(.success(cities))
            } catch {
                print("Decoding error: \(error)")
                completion(.failure(.decodingError))
            }
        }
        
        task.resume()
    }
}

// MARK: - Mock Service for Testing

class MockCitySearchService: CitySearchService {
    
    private let mockCities = [
        CitySearchResult(name: "New York", country: "US", state: "NY", lat: 40.7128, lon: -74.0060),
        CitySearchResult(name: "London", country: "GB", state: nil, lat: 51.5074, lon: -0.1278),
        CitySearchResult(name: "Paris", country: "FR", state: nil, lat: 48.8566, lon: 2.3522),
        CitySearchResult(name: "Tokyo", country: "JP", state: nil, lat: 35.6762, lon: 139.6503),
        CitySearchResult(name: "Sydney", country: "AU", state: "NSW", lat: -33.8688, lon: 151.2093),
        CitySearchResult(name: "San Francisco", country: "US", state: "CA", lat: 37.7749, lon: -122.4194),
        CitySearchResult(name: "Toronto", country: "CA", state: "ON", lat: 43.6532, lon: -79.3832),
        CitySearchResult(name: "Berlin", country: "DE", state: nil, lat: 52.5200, lon: 13.4050),
        CitySearchResult(name: "Rome", country: "IT", state: nil, lat: 41.9028, lon: 12.4964),
        CitySearchResult(name: "Madrid", country: "ES", state: nil, lat: 40.4168, lon: -3.7038)
    ]
    
    override func searchCities(query: String, completion: @escaping (Result<[CitySearchResult], CitySearchError>) -> Void) {
        // Simulate network delay
        DispatchQueue.global().asyncAfter(deadline: .now() + 0.3) {
            let filteredCities = self.mockCities.filter { city in
                city.name.lowercased().contains(query.lowercased()) ||
                city.country.lowercased().contains(query.lowercased()) ||
                (city.state?.lowercased().contains(query.lowercased()) ?? false)
            }
            
            completion(.success(Array(filteredCities.prefix(5))))
        }
    }
}