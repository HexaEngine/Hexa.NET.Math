# HexaEngine Math Library

The Math library for HexaEngine provides comprehensive mathematical functions and utilities, optimized for performance and accuracy. Available in both minimal and full versions, this library is designed to be versatile and can be used with HexaEngine or any other project.

## Features

### Optimized Math Functions
- **SIMD Optimized Math Functions**: High-performance functions for vectors, matrices, quaternions, and more.
- **Double Precision Math Functions**: For applications requiring high precision.

### Geometric Calculations
- **Frustum, Plane, Ray, Sphere, Box, and AABB Math**: Essential geometric operations for 3D applications.

### Noise Functions
- **Generic Noise**
- **Perlin Noise**
- **Simplex Noise**

### Polynomial and Bezier Functions
- **Polynomial Functions**: For advanced mathematical calculations.
- **Bezier Functions**: For curve and surface modeling.

### Extensions and Utilities
- **Vector Math Extensions**: Additional operations for vector math.
- **Sky Model Functions**: Implementations of Preetham and Hosek-Wilkie sky models.
- **Color Math Functions**: Including RGBA, HSVA, and HSLA color models.
- **Prime Number Functions and Caching**: Efficient prime number generation and caching.

### Shadow Mapping (Full Version Only)
- **Shadow Mapping Functions**: Support for OSM, PSM, DPSM, and CSM techniques.

### Transform Class (Full Version Only)
- **Transform Class**: Comprehensive class for managing object transformations.

## Getting Started

To get started with the HexaEngine Math library, follow these steps:

1. **Install the NuGet package**:
    ```bash
    dotnet add package Hexa.NET.Math
    ```
    Full version ``X.X.X-full`` vs minimal verions ```X.X.X-minimal```, make sure to enable allow pre-releases in Visual Studio to see them.

2. **Include the library in your project**:
    ```csharp
    using Hexa.NET.Mathematics;
    ```

3. **Utilize the math functions**:
    ```csharp
    MathUtil.XXX();
    ```

4. **Perform geometric calculations**:
    ```csharp
    var sphere = new BoundingSphere(new Vector3(0, 0, 0), 1.0f);
    var box = new BoundingBox(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
    ```

5. **Generate noise**:
    ```csharp
    var noiseValue = PerlinNoise.Noise(0.5f, 0.5f, 0.5f);
    ```

6. **Work with colors**:
    ```csharp
    var color = Color.FromRGBA(0xFFAABBCC);
    ```

## Minimal vs Full Version

- **Minimal Version**: Contains essential math functions and utilities.
- **Full Version**: Includes additional features such as shadow mapping functions and the Transform class.

## Contributions

Contributions are welcome! If you have ideas for new features or improvements, feel free to submit a pull request or open an issue.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.
