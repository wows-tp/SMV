## Overview
This is an ASP.NET Core MVC web application built on .NET 8 that demonstrates memory management and garbage collection in C# through image processing. [cite: 3] [cite_start]The application uploads images, applies processing effects, and tracks memory usage throughout the system lifecycle.

## Feature
* **Image upload**: User selects an image and sets a multiplier
* **Image processor**: Apply blur effect using SixLabors.ImageSharp
* **Memory Simulation**: User able to configure memory bloat simulation to simulate memory pressure
* **Real-time memory result**: User able to monitors RAM usage on different stages
  * Before process
  * Peak when process
  * After garbage collection
* **Visual result**: Display processed image into HTML and detail memory result

## Technology
* **IDE**: Visual Studio 2022
* **Framework**: .NET 8.0
* **Web Framework**: ASP.NET Core MVC
* **Dependencies**: SixLabors.ImageSharp

## Installation & Setup
1. Clone or download the repository from GitHub.
2. Open SMV-InterviewProject.sln in Visual Studio 2022.
3. Open Package Manager Console to restore dependencies: `Update-Package -reinstall`
4. Build the solution.

## Key Components

### HomeController
* **Index()**: Display the upload form and result form
* **ImageProcessor()**: Process uploaded images and track memories
* **Error()**: Handles error [cite: 28]
* **Privacy()**: Default privacy policy page

### ImageProcessViewModel
* **startMemory**: RAM before processing (MB)
* **peakMemory**: Peak RAM during processing (MB)
* **endMemory**: RAM after garbage collection (MB)
* **processedImage**: Base64-encoded processed image
* **increaseMemory**: Calculated memory increase
* **releaseMemory**: Memory released by GC
* **releaseMemoryPercentage**: Efficiency of garbage collection

## How it works
1. **Upload**: User selects an image and sets a multiplier
2. **Memory Baseline**: Application records initial memory state
3. **Memory Bloat**: Creates multiple copies of the image bytes (controlled by multiplier) to simulate memory pressure
4. **Processing**: Apply blur to the image using ImageSharp
5. **Cleanup**: Clears memory bloat and forces garbage collection
6. **Results**: Displays processed image with before/after memory statistics

## Usage

### Running the application
1. Build and run the project in Visual Studio 2022
2. Select an image file 
3. Adjust the test Multiplier (default: 40) to control memory simulation 
4. Click "Start upload"
5. View the blurred image and memory usage statistics 

## Dependencies
* **SixLabors.ImageSharp**: Modern & cross-platform image processing library
* Replace System.Drawing for better performance

## Memory Management Demonstration
This project are created intentionally to demo:
* Manual garbage collection triggering with `GC.Collect()` 
* Memory tracking using `GC.GetTotalMemory()` and `Process.WorkingSet64` 
* Resource cleanup patterns with `using` statements 
* Impact of large object allocation and de-allocation 

## Notes
* Async operations (async/await) to prevent server blocking during file upload 
* Base64 encoding enable direct image display in HTML
* Peak memory includes simulated bloat for interview assessment purposes
