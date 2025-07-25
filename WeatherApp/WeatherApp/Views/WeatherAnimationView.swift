import UIKit

class WeatherAnimationView: UIView {
    
    // MARK: - Properties
    
    private var animationLayers: [CALayer] = []
    private var currentWeatherType: WeatherType = .clear
    
    enum WeatherType {
        case clear
        case cloudy
        case rainy
        case snowy
        case thunderstorm
        case foggy
    }
    
    // MARK: - Initialization
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        setupView()
    }
    
    required init?(coder: NSCoder) {
        super.init(coder: coder)
        setupView()
    }
    
    // MARK: - Setup
    
    private func setupView() {
        backgroundColor = UIColor.clear
        clipsToBounds = true
    }
    
    override func layoutSubviews() {
        super.layoutSubviews()
        // Restart animation when view layout changes
        if !animationLayers.isEmpty {
            startAnimation(for: currentWeatherType)
        }
    }
    
    // MARK: - Public Methods
    
    func setWeatherType(from weatherMain: String, isDay: Bool = true) {
        let weatherType: WeatherType
        
        switch weatherMain.lowercased() {
        case "clear":
            weatherType = .clear
        case "clouds":
            weatherType = .cloudy
        case "rain", "drizzle":
            weatherType = .rainy
        case "snow":
            weatherType = .snowy
        case "thunderstorm":
            weatherType = .thunderstorm
        case "mist", "fog", "haze":
            weatherType = .foggy
        default:
            weatherType = .clear
        }
        
        startAnimation(for: weatherType)
    }
    
    func startAnimation(for weatherType: WeatherType) {
        stopAnimation()
        currentWeatherType = weatherType
        
        switch weatherType {
        case .clear:
            animateSunshine()
        case .cloudy:
            animateClouds()
        case .rainy:
            animateRain()
        case .snowy:
            animateSnow()
        case .thunderstorm:
            animateThunderstorm()
        case .foggy:
            animateFog()
        }
    }
    
    func stopAnimation() {
        animationLayers.forEach { $0.removeFromSuperlayer() }
        animationLayers.removeAll()
    }
    
    // MARK: - Animation Methods
    
    private func animateSunshine() {
        // Create sun rays animation
        for i in 0..<8 {
            let rayLayer = createSunRay(at: i)
            layer.addSublayer(rayLayer)
            animationLayers.append(rayLayer)
            
            let rotationAnimation = CABasicAnimation(keyPath: "transform.rotation")
            rotationAnimation.fromValue = 0
            rotationAnimation.toValue = 2 * Double.pi
            rotationAnimation.duration = 10.0
            rotationAnimation.repeatCount = .infinity
            rayLayer.add(rotationAnimation, forKey: "rotation")
        }
        
        // Add floating particles
        addFloatingParticles(color: UIColor.systemYellow.withAlphaComponent(0.3), count: 5)
    }
    
    private func animateClouds() {
        // Create moving clouds
        for i in 0..<3 {
            let cloudLayer = createCloud()
            cloudLayer.position = CGPoint(x: -100, y: CGFloat(50 + i * 40))
            layer.addSublayer(cloudLayer)
            animationLayers.append(cloudLayer)
            
            let moveAnimation = CABasicAnimation(keyPath: "position.x")
            moveAnimation.fromValue = -100
            moveAnimation.toValue = bounds.width + 100
            moveAnimation.duration = Double(15 + i * 3)
            moveAnimation.repeatCount = .infinity
            cloudLayer.add(moveAnimation, forKey: "move")
        }
    }
    
    private func animateRain() {
        // Create rain drops
        for _ in 0..<20 {
            DispatchQueue.main.asyncAfter(deadline: .now() + Double.random(in: 0...2)) {
                self.createRainDrop()
            }
        }
        
        // Add background clouds
        animateClouds()
    }
    
    private func animateSnow() {
        // Create snowflakes
        for _ in 0..<15 {
            DispatchQueue.main.asyncAfter(deadline: .now() + Double.random(in: 0...3)) {
                self.createSnowflake()
            }
        }
    }
    
    private func animateThunderstorm() {
        // Start with rain
        animateRain()
        
        // Add lightning flashes
        addLightningFlashes()
    }
    
    private func animateFog() {
        // Create fog layers
        for i in 0..<4 {
            let fogLayer = createFogLayer()
            fogLayer.position = CGPoint(x: bounds.width / 2, y: bounds.height * 0.2 + CGFloat(i * 30))
            layer.addSublayer(fogLayer)
            animationLayers.append(fogLayer)
            
            let moveAnimation = CABasicAnimation(keyPath: "position.x")
            moveAnimation.fromValue = -bounds.width / 2
            moveAnimation.toValue = bounds.width * 1.5
            moveAnimation.duration = Double(20 + i * 5)
            moveAnimation.repeatCount = .infinity
            fogLayer.add(moveAnimation, forKey: "move")
        }
    }
    
    // MARK: - Helper Methods
    
    private func createSunRay(at index: Int) -> CAShapeLayer {
        let rayLayer = CAShapeLayer()
        let path = UIBezierPath()
        
        let angle = Double(index) * Double.pi / 4
        let startRadius: CGFloat = 30
        let endRadius: CGFloat = 60
        
        let startPoint = CGPoint(
            x: bounds.width / 2 + startRadius * cos(angle),
            y: bounds.height / 2 + startRadius * sin(angle)
        )
        let endPoint = CGPoint(
            x: bounds.width / 2 + endRadius * cos(angle),
            y: bounds.height / 2 + endRadius * sin(angle)
        )
        
        path.move(to: startPoint)
        path.addLine(to: endPoint)
        
        rayLayer.path = path.cgPath
        rayLayer.strokeColor = UIColor.systemYellow.withAlphaComponent(0.6).cgColor
        rayLayer.lineWidth = 3
        rayLayer.lineCap = .round
        
        return rayLayer
    }
    
    private func createCloud() -> CAShapeLayer {
        let cloudLayer = CAShapeLayer()
        let path = UIBezierPath()
        
        // Create cloud shape with multiple circles
        let baseY: CGFloat = 0
        path.addArc(withCenter: CGPoint(x: 30, y: baseY), radius: 15, startAngle: 0, endAngle: 2 * .pi, clockwise: true)
        path.addArc(withCenter: CGPoint(x: 50, y: baseY - 5), radius: 20, startAngle: 0, endAngle: 2 * .pi, clockwise: true)
        path.addArc(withCenter: CGPoint(x: 70, y: baseY), radius: 15, startAngle: 0, endAngle: 2 * .pi, clockwise: true)
        path.addArc(withCenter: CGPoint(x: 50, y: baseY + 10), radius: 25, startAngle: 0, endAngle: 2 * .pi, clockwise: true)
        
        cloudLayer.path = path.cgPath
        cloudLayer.fillColor = UIColor.systemGray3.withAlphaComponent(0.7).cgColor
        cloudLayer.strokeColor = UIColor.clear.cgColor
        
        return cloudLayer
    }
    
    private func createRainDrop() {
        let dropLayer = CAShapeLayer()
        let path = UIBezierPath(ovalIn: CGRect(x: 0, y: 0, width: 2, height: 8))
        
        dropLayer.path = path.cgPath
        dropLayer.fillColor = UIColor.systemBlue.withAlphaComponent(0.7).cgColor
        dropLayer.position = CGPoint(x: CGFloat.random(in: 0...bounds.width), y: -10)
        
        layer.addSublayer(dropLayer)
        animationLayers.append(dropLayer)
        
        let fallAnimation = CABasicAnimation(keyPath: "position.y")
        fallAnimation.fromValue = -10
        fallAnimation.toValue = bounds.height + 10
        fallAnimation.duration = Double.random(in: 0.5...1.5)
        fallAnimation.delegate = AnimationDelegate { [weak self] in
            dropLayer.removeFromSuperlayer()
            if let index = self?.animationLayers.firstIndex(of: dropLayer) {
                self?.animationLayers.remove(at: index)
            }
            
            // Create new rain drop
            if self?.currentWeatherType == .rainy || self?.currentWeatherType == .thunderstorm {
                DispatchQueue.main.asyncAfter(deadline: .now() + Double.random(in: 0.1...0.5)) {
                    self?.createRainDrop()
                }
            }
        }
        
        dropLayer.add(fallAnimation, forKey: "fall")
    }
    
    private func createSnowflake() {
        let snowLayer = CAShapeLayer()
        let path = UIBezierPath()
        
        // Create snowflake shape
        let center = CGPoint(x: 5, y: 5)
        for i in 0..<6 {
            let angle = Double(i) * Double.pi / 3
            let endPoint = CGPoint(
                x: center.x + 4 * cos(angle),
                y: center.y + 4 * sin(angle)
            )
            path.move(to: center)
            path.addLine(to: endPoint)
        }
        
        snowLayer.path = path.cgPath
        snowLayer.strokeColor = UIColor.white.cgColor
        snowLayer.lineWidth = 1
        snowLayer.lineCap = .round
        snowLayer.position = CGPoint(x: CGFloat.random(in: 0...bounds.width), y: -10)
        
        layer.addSublayer(snowLayer)
        animationLayers.append(snowLayer)
        
        let fallAnimation = CABasicAnimation(keyPath: "position.y")
        fallAnimation.fromValue = -10
        fallAnimation.toValue = bounds.height + 10
        fallAnimation.duration = Double.random(in: 3...6)
        
        let swayAnimation = CABasicAnimation(keyPath: "position.x")
        swayAnimation.fromValue = snowLayer.position.x - 20
        swayAnimation.toValue = snowLayer.position.x + 20
        swayAnimation.duration = Double.random(in: 2...4)
        swayAnimation.repeatCount = .infinity
        swayAnimation.autoreverses = true
        
        let rotationAnimation = CABasicAnimation(keyPath: "transform.rotation")
        rotationAnimation.fromValue = 0
        rotationAnimation.toValue = 2 * Double.pi
        rotationAnimation.duration = Double.random(in: 2...5)
        rotationAnimation.repeatCount = .infinity
        
        fallAnimation.delegate = AnimationDelegate { [weak self] in
            snowLayer.removeFromSuperlayer()
            if let index = self?.animationLayers.firstIndex(of: snowLayer) {
                self?.animationLayers.remove(at: index)
            }
            
            // Create new snowflake
            if self?.currentWeatherType == .snowy {
                DispatchQueue.main.asyncAfter(deadline: .now() + Double.random(in: 0.5...2)) {
                    self?.createSnowflake()
                }
            }
        }
        
        snowLayer.add(fallAnimation, forKey: "fall")
        snowLayer.add(swayAnimation, forKey: "sway")
        snowLayer.add(rotationAnimation, forKey: "rotate")
    }
    
    private func createFogLayer() -> CAShapeLayer {
        let fogLayer = CAShapeLayer()
        let path = UIBezierPath(roundedRect: CGRect(x: 0, y: 0, width: bounds.width + 100, height: 40), cornerRadius: 20)
        
        fogLayer.path = path.cgPath
        fogLayer.fillColor = UIColor.systemGray.withAlphaComponent(0.3).cgColor
        
        return fogLayer
    }
    
    private func addLightningFlashes() {
        let flashLayer = CALayer()
        flashLayer.frame = bounds
        flashLayer.backgroundColor = UIColor.white.withAlphaComponent(0.8).cgColor
        flashLayer.opacity = 0
        
        layer.addSublayer(flashLayer)
        animationLayers.append(flashLayer)
        
        let flashAnimation = CAKeyframeAnimation(keyPath: "opacity")
        flashAnimation.values = [0, 1, 0, 1, 0]
        flashAnimation.keyTimes = [0, 0.1, 0.2, 0.3, 0.4]
        flashAnimation.duration = 0.4
        flashAnimation.repeatCount = .infinity
        flashAnimation.beginTime = CACurrentMediaTime() + Double.random(in: 2...8)
        
        flashLayer.add(flashAnimation, forKey: "flash")
    }
    
    private func addFloatingParticles(color: UIColor, count: Int) {
        for _ in 0..<count {
            let particleLayer = CAShapeLayer()
            let path = UIBezierPath(ovalIn: CGRect(x: 0, y: 0, width: 4, height: 4))
            
            particleLayer.path = path.cgPath
            particleLayer.fillColor = color.cgColor
            particleLayer.position = CGPoint(
                x: CGFloat.random(in: 0...bounds.width),
                y: CGFloat.random(in: 0...bounds.height)
            )
            
            layer.addSublayer(particleLayer)
            animationLayers.append(particleLayer)
            
            let floatAnimation = CAKeyframeAnimation(keyPath: "position")
            let path = CGMutablePath()
            path.move(to: particleLayer.position)
            for _ in 0..<10 {
                path.addLine(to: CGPoint(
                    x: particleLayer.position.x + CGFloat.random(in: -50...50),
                    y: particleLayer.position.y + CGFloat.random(in: -30...30)
                ))
            }
            
            floatAnimation.path = path
            floatAnimation.duration = Double.random(in: 8...15)
            floatAnimation.repeatCount = .infinity
            
            particleLayer.add(floatAnimation, forKey: "float")
        }
    }
}

// MARK: - Animation Delegate

class AnimationDelegate: NSObject, CAAnimationDelegate {
    private let completion: () -> Void
    
    init(completion: @escaping () -> Void) {
        self.completion = completion
        super.init()
    }
    
    func animationDidStop(_ anim: CAAnimation, finished flag: Bool) {
        if flag {
            completion()
        }
    }
}