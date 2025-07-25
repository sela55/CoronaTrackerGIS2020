import UIKit
import CoreLocation
import Combine

class WeatherViewController: UIViewController {
    
    // MARK: - IBOutlets
    
    @IBOutlet weak var searchTextField: UITextField!
    @IBOutlet weak var cityTableView: UITableView!
    @IBOutlet weak var weatherAnimationView: WeatherAnimationView!
    @IBOutlet weak var temperatureLabel: UILabel!
    @IBOutlet weak var descriptionLabel: UILabel!
    @IBOutlet weak var cityNameLabel: UILabel!
    @IBOutlet weak var weatherIconImageView: UIImageView!
    @IBOutlet weak var detailsStackView: UIStackView!
    @IBOutlet weak var feelsLikeLabel: UILabel!
    @IBOutlet weak var humidityLabel: UILabel!
    @IBOutlet weak var windSpeedLabel: UILabel!
    @IBOutlet weak var pressureLabel: UILabel!
    @IBOutlet weak var loadingIndicator: UIActivityIndicatorView!
    @IBOutlet weak var locationButton: UIButton!
    
    // MARK: - Properties
    
    private let weatherModel = WeatherModel()
    private let locationManager = CLLocationManager()
    private var searchTimer: Timer?
    
    // MARK: - Lifecycle
    
    override func viewDidLoad() {
        super.viewDidLoad()
        setupUI()
        setupLocationManager()
        setupBindings()
        setupGestures()
        
        // Load default city weather
        weatherModel.fetchWeather(for: "San Francisco")
    }
    
    // MARK: - Setup Methods
    
    private func setupUI() {
        // Setup navigation
        title = "Weather"
        navigationController?.navigationBar.prefersLargeTitles = true
        
        // Setup search text field
        searchTextField.delegate = self
        searchTextField.placeholder = "Search for a city..."
        searchTextField.borderStyle = .roundedRect
        searchTextField.returnKeyType = .search
        searchTextField.autocorrectionType = .no
        searchTextField.clearButtonMode = .whileEditing
        
        // Setup table view
        cityTableView.delegate = self
        cityTableView.dataSource = self
        cityTableView.isHidden = true
        cityTableView.layer.cornerRadius = 8
        cityTableView.layer.borderWidth = 1
        cityTableView.layer.borderColor = UIColor.systemGray4.cgColor
        cityTableView.register(UITableViewCell.self, forCellReuseIdentifier: "CityCell")
        
        // Setup weather animation view
        weatherAnimationView.layer.cornerRadius = 12
        weatherAnimationView.backgroundColor = UIColor.systemBackground.withAlphaComponent(0.1)
        
        // Setup labels
        temperatureLabel.font = UIFont.systemFont(ofSize: 72, weight: .thin)
        temperatureLabel.textAlignment = .center
        cityNameLabel.font = UIFont.systemFont(ofSize: 24, weight: .medium)
        cityNameLabel.textAlignment = .center
        descriptionLabel.font = UIFont.systemFont(ofSize: 18, weight: .regular)
        descriptionLabel.textAlignment = .center
        
        // Setup details labels
        setupDetailLabels()
        
        // Setup weather icon
        weatherIconImageView.contentMode = .scaleAspectFit
        weatherIconImageView.tintColor = .systemBlue
        
        // Setup loading indicator
        loadingIndicator.style = .large
        loadingIndicator.hidesWhenStopped = true
        
        // Setup location button
        locationButton.setImage(UIImage(systemName: "location.fill"), for: .normal)
        locationButton.tintColor = .systemBlue
        locationButton.layer.cornerRadius = 20
        locationButton.backgroundColor = UIColor.systemBackground.withAlphaComponent(0.8)
        
        // Setup gradients
        setupGradientBackground()
    }
    
    private func setupDetailLabels() {
        let detailLabels = [feelsLikeLabel, humidityLabel, windSpeedLabel, pressureLabel]
        
        detailLabels.forEach { label in
            label?.font = UIFont.systemFont(ofSize: 16, weight: .medium)
            label?.textAlignment = .center
            label?.backgroundColor = UIColor.systemBackground.withAlphaComponent(0.3)
            label?.layer.cornerRadius = 8
            label?.layer.masksToBounds = true
            label?.numberOfLines = 2
        }
    }
    
    private func setupGradientBackground() {
        let gradientLayer = CAGradientLayer()
        gradientLayer.frame = view.bounds
        gradientLayer.colors = [
            UIColor.systemBlue.withAlphaComponent(0.8).cgColor,
            UIColor.systemPurple.withAlphaComponent(0.6).cgColor,
            UIColor.systemPink.withAlphaComponent(0.4).cgColor
        ]
        gradientLayer.startPoint = CGPoint(x: 0, y: 0)
        gradientLayer.endPoint = CGPoint(x: 1, y: 1)
        view.layer.insertSublayer(gradientLayer, at: 0)
    }
    
    private func setupLocationManager() {
        locationManager.delegate = self
        locationManager.desiredAccuracy = kCLLocationAccuracyBest
    }
    
    private func setupBindings() {
        // Observe weather model changes
        weatherModel.$currentWeather.assign(to: \.currentWeather, on: self)
        weatherModel.$isLoading.assign(to: \.isLoading, on: self)
        weatherModel.$errorMessage.assign(to: \.errorMessage, on: self)
        weatherModel.$cities.assign(to: \.cities, on: self)
    }
    
    private func setupGestures() {
        // Add tap gesture to dismiss keyboard and hide suggestions
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(dismissKeyboard))
        tapGesture.cancelsTouchesInView = false
        view.addGestureRecognizer(tapGesture)
    }
    
    // MARK: - Property Observers
    
    private var currentWeather: WeatherData? {
        didSet {
            updateUI()
        }
    }
    
    private var isLoading: Bool = false {
        didSet {
            DispatchQueue.main.async {
                if self.isLoading {
                    self.loadingIndicator.startAnimating()
                } else {
                    self.loadingIndicator.stopAnimating()
                }
            }
        }
    }
    
    private var errorMessage: String? {
        didSet {
            if let error = errorMessage {
                showErrorAlert(message: error)
            }
        }
    }
    
    private var cities: [CitySearchResult] = [] {
        didSet {
            DispatchQueue.main.async {
                self.cityTableView.reloadData()
                self.cityTableView.isHidden = self.cities.isEmpty
            }
        }
    }
    
    // MARK: - UI Update Methods
    
    private func updateUI() {
        guard let weather = currentWeather else { return }
        
        DispatchQueue.main.async {
            // Update main weather info
            self.temperatureLabel.text = self.weatherModel.temperatureString()
            self.cityNameLabel.text = self.weatherModel.cityName()
            self.descriptionLabel.text = self.weatherModel.weatherDescription()
            
            // Update weather icon
            let iconName = self.weatherModel.weatherIconName()
            self.weatherIconImageView.image = UIImage(systemName: iconName)
            
            // Update detail labels
            self.feelsLikeLabel.text = "Feels like\n\(self.weatherModel.feelsLike())"
            self.humidityLabel.text = "Humidity\n\(self.weatherModel.humidity())"
            self.windSpeedLabel.text = "Wind\n\(self.weatherModel.windSpeed())"
            self.pressureLabel.text = "Pressure\n\(self.weatherModel.pressure())"
            
            // Update animation
            if let weatherType = weather.weather.first?.main {
                self.weatherAnimationView.setWeatherType(from: weatherType, isDay: weather.isDay)
            }
            
            // Update background gradient based on weather
            self.updateBackgroundGradient(for: weather)
        }
    }
    
    private func updateBackgroundGradient(for weather: WeatherData) {
        guard let gradientLayer = view.layer.sublayers?.first as? CAGradientLayer else { return }
        
        let weatherType = weather.weather.first?.main.lowercased() ?? "clear"
        let isDay = weather.isDay
        
        var colors: [CGColor]
        
        switch weatherType {
        case "clear":
            colors = isDay ? [
                UIColor.systemBlue.withAlphaComponent(0.8).cgColor,
                UIColor.systemYellow.withAlphaComponent(0.6).cgColor,
                UIColor.systemOrange.withAlphaComponent(0.4).cgColor
            ] : [
                UIColor.systemIndigo.withAlphaComponent(0.8).cgColor,
                UIColor.systemPurple.withAlphaComponent(0.6).cgColor,
                UIColor.black.withAlphaComponent(0.4).cgColor
            ]
        case "clouds":
            colors = [
                UIColor.systemGray.withAlphaComponent(0.8).cgColor,
                UIColor.systemGray2.withAlphaComponent(0.6).cgColor,
                UIColor.systemGray4.withAlphaComponent(0.4).cgColor
            ]
        case "rain", "drizzle":
            colors = [
                UIColor.systemBlue.withAlphaComponent(0.9).cgColor,
                UIColor.systemTeal.withAlphaComponent(0.7).cgColor,
                UIColor.systemGray.withAlphaComponent(0.5).cgColor
            ]
        case "thunderstorm":
            colors = [
                UIColor.darkGray.withAlphaComponent(0.9).cgColor,
                UIColor.systemPurple.withAlphaComponent(0.7).cgColor,
                UIColor.black.withAlphaComponent(0.5).cgColor
            ]
        case "snow":
            colors = [
                UIColor.white.withAlphaComponent(0.9).cgColor,
                UIColor.systemGray6.withAlphaComponent(0.7).cgColor,
                UIColor.systemBlue.withAlphaComponent(0.5).cgColor
            ]
        default:
            colors = [
                UIColor.systemBlue.withAlphaComponent(0.8).cgColor,
                UIColor.systemPurple.withAlphaComponent(0.6).cgColor,
                UIColor.systemPink.withAlphaComponent(0.4).cgColor
            ]
        }
        
        let animation = CABasicAnimation(keyPath: "colors")
        animation.fromValue = gradientLayer.colors
        animation.toValue = colors
        animation.duration = 1.0
        animation.fillMode = .forwards
        animation.isRemovedOnCompletion = false
        
        gradientLayer.colors = colors
        gradientLayer.add(animation, forKey: "colorChange")
    }
    
    // MARK: - Actions
    
    @IBAction func locationButtonTapped(_ sender: UIButton) {
        requestLocationPermission()
    }
    
    @objc private func dismissKeyboard() {
        view.endEditing(true)
        cityTableView.isHidden = true
    }
    
    // MARK: - Helper Methods
    
    private func requestLocationPermission() {
        switch locationManager.authorizationStatus {
        case .notDetermined:
            locationManager.requestWhenInUseAuthorization()
        case .denied, .restricted:
            showLocationPermissionAlert()
        case .authorizedWhenInUse, .authorizedAlways:
            getCurrentLocation()
        @unknown default:
            break
        }
    }
    
    private func getCurrentLocation() {
        loadingIndicator.startAnimating()
        locationManager.requestLocation()
    }
    
    private func showLocationPermissionAlert() {
        let alert = UIAlertController(
            title: "Location Permission Required",
            message: "Please enable location access in Settings to get weather for your current location.",
            preferredStyle: .alert
        )
        
        alert.addAction(UIAlertAction(title: "Settings", style: .default) { _ in
            if let settingsURL = URL(string: UIApplication.openSettingsURLString) {
                UIApplication.shared.open(settingsURL)
            }
        })
        
        alert.addAction(UIAlertAction(title: "Cancel", style: .cancel))
        
        present(alert, animated: true)
    }
    
    private func showErrorAlert(message: String) {
        DispatchQueue.main.async {
            let alert = UIAlertController(
                title: "Error",
                message: message,
                preferredStyle: .alert
            )
            alert.addAction(UIAlertAction(title: "OK", style: .default))
            self.present(alert, animated: true)
        }
    }
}

// MARK: - UITextFieldDelegate

extension WeatherViewController: UITextFieldDelegate {
    
    func textField(_ textField: UITextField, shouldChangeCharactersIn range: NSRange, replacementString string: String) -> Bool {
        let newText = (textField.text as NSString?)?.replacingCharacters(in: range, with: string) ?? ""
        
        // Cancel previous search timer
        searchTimer?.invalidate()
        
        // Start new search timer
        searchTimer = Timer.scheduledTimer(withTimeInterval: 0.5, repeats: false) { _ in
            self.weatherModel.searchCities(query: newText)
        }
        
        return true
    }
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        guard let text = textField.text, !text.isEmpty else { return false }
        
        weatherModel.fetchWeather(for: text)
        textField.resignFirstResponder()
        cityTableView.isHidden = true
        
        return true
    }
    
    func textFieldDidBeginEditing(_ textField: UITextField) {
        if !cities.isEmpty {
            cityTableView.isHidden = false
        }
    }
}

// MARK: - UITableViewDataSource

extension WeatherViewController: UITableViewDataSource {
    
    func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return cities.count
    }
    
    func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: "CityCell", for: indexPath)
        let city = cities[indexPath.row]
        
        var displayText = "\(city.name), \(city.country)"
        if let state = city.state {
            displayText = "\(city.name), \(state), \(city.country)"
        }
        
        cell.textLabel?.text = displayText
        cell.backgroundColor = UIColor.systemBackground.withAlphaComponent(0.9)
        cell.textLabel?.font = UIFont.systemFont(ofSize: 16)
        
        return cell
    }
}

// MARK: - UITableViewDelegate

extension WeatherViewController: UITableViewDelegate {
    
    func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        
        let selectedCity = cities[indexPath.row]
        searchTextField.text = selectedCity.name
        
        weatherModel.fetchWeather(latitude: selectedCity.lat, longitude: selectedCity.lon)
        
        searchTextField.resignFirstResponder()
        cityTableView.isHidden = true
    }
}

// MARK: - CLLocationManagerDelegate

extension WeatherViewController: CLLocationManagerDelegate {
    
    func locationManager(_ manager: CLLocationManager, didUpdateLocations locations: [CLLocation]) {
        guard let location = locations.first else { return }
        
        weatherModel.fetchWeather(latitude: location.coordinate.latitude, longitude: location.coordinate.longitude)
    }
    
    func locationManager(_ manager: CLLocationManager, didFailWithError error: Error) {
        loadingIndicator.stopAnimating()
        showErrorAlert(message: "Failed to get current location: \(error.localizedDescription)")
    }
    
    func locationManagerDidChangeAuthorization(_ manager: CLLocationManager) {
        switch manager.authorizationStatus {
        case .authorizedWhenInUse, .authorizedAlways:
            getCurrentLocation()
        case .denied, .restricted:
            showLocationPermissionAlert()
        default:
            break
        }
    }
}