# LatestGame

**LatestGame** is a 3D navigation and exploration game built using the Mapbox Unity SDK. This project creates an interactive city environment where players control an astronaut character to navigate through a map, with dynamic traffic simulation represented by animated car textures on the roads. The game integrates real-world map data, pathfinding, and visual effects to provide an immersive experience, suitable for both Unity Editor testing and WebGL deployment.

## Key Features

- **Interactive Navigation**: Players can click on the map to move the astronaut character, utilizing Mapbox’s Directions API for real-time pathfinding.
- **Dynamic Traffic Simulation**: Animated car textures on roads, controlled by the `TrafficUvAnimator` script, create the illusion of moving traffic.
- **Customizable Car Appearance**: Adjust the size and density of cars through simple variables in the `TrafficUvAnimator` script, allowing for easy customization of the traffic visuals.
- **Real-World Map Integration**: Uses Mapbox’s `AbstractMap` component to render a 3D city environment with roads, buildings, and greenery, based on real geographic data.
- **WebGL Deployment**: Designed for cross-platform play, supporting both Unity Editor testing and WebGL builds for browser-based access.

## Technologies Used

- **Unity**: The core game engine for building and rendering the 3D environment.
- **Mapbox Unity SDK**: Provides map data, pathfinding, and mesh generation for roads and terrain.
- **C#**: Used for scripting game logic, including character movement (`AstronautMouseController`), traffic animation (`TrafficUvAnimator`), and mesh generation (`LoftModifier`, `MaterialModifier`).
- **WebGL**: Enables browser-based deployment for accessibility across devices.
- **Git**: Manages version control, ensuring collaborative development and project tracking.

## How It Works

1. **Map Setup**: The `AbstractMap` component loads real-world map data, generating a 3D city with roads and buildings using `LoftModifier` for road geometry and `MaterialModifier` for texturing.
2. **Character Navigation**: Players click on the map to set a destination, and the `AstronautMouseController` script queries the Mapbox Directions API to calculate a path. The astronaut moves along the path with smooth rotation and animation.
3. **Traffic Animation**: The `TrafficUvAnimator` script animates car textures on the roads by scrolling the UV offset, creating a dynamic traffic effect. Users can adjust the `Speed` variable to control car density and the `TextureTiling` variable to scale car size.
4. **Customization**: Developers can tweak car appearance by modifying `TextureTiling` in the `TrafficUvAnimator` script, with higher values making cars smaller while maintaining their proportions.
5. **Deployment**: The game can be tested in the Unity Editor or built as a WebGL application for deployment on a web server, accessible via a browser.

### Running the Project

1. **Clone the Repository**: Clone this repository to your local machine using Git:
2. **Install Unity**: Ensure you have Unity Hub installed (version 2020.3 or later recommended). Open Unity Hub, add the project by selecting the cloned folder, and open it.
3. **Install Mapbox SDK**: Import the Mapbox Unity SDK via the Unity Package Manager or manually by adding the SDK package from the Mapbox website. Configure your Mapbox access token in the Unity Inspector under `Window > Mapbox > Mapbox Windows > Access Token`.
4. **Set Up the Scene**: Open the `AstronautGame` scene (located in `Assets/Mapbox/Examples/2_AstronautGame/AstronautGame`). Ensure all scripts (`AstronautMouseController`, `TrafficUvAnimator`, `LoftModifier`, `MaterialModifier`) are attached to their respective GameObjects and references are assigned in the Inspector (e.g., `Map`, `RayPlane`, `Materials`).
5. **Test in Editor**: Press the Play button in Unity to test the game. Click on the map to move the astronaut and observe the traffic animation.
6. **Build for WebGL**: Go to `File > Build Settings`, select **WebGL**, and click **Build and Run**. Save the build to a folder and open the `index.html` file in a browser.
7. **Deploy Locally (Optional)**: For public testing, use ngrok by running `ngrok http 8080` in a terminal from the build folder, then share the provided URL to access the game remotely.

