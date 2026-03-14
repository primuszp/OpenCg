# OpenCg

OpenCg is a managed .NET wrapper around NVIDIA's legacy Cg runtime, plus a small OpenTK-based example application that ports several original Cg/OpenGL samples.

## Status

Cg is a discontinued NVIDIA technology. Cg 3.1 is the final release and is no longer under active development or support.

- Toolkit overview: https://developer.nvidia.com/cg-toolkit
- Toolkit download: https://developer.nvidia.com/cg-toolkit-download
- Documentation archive: http://developer.download.nvidia.com/cg/

This repository keeps the wrapper usable on modern .NET and includes fixes for interop and marshalling issues in the original binding layer.

## Repository layout

- `Primusz.OpenCg/OpenCg`
  The managed wrapper library.
- `Primusz.OpenCg/OpenCg.Examples`
  A Windows Forms launcher with OpenTK examples and Cg shaders.
- `Primusz.OpenCg/Primusz.OpenCg.sln`
  The main solution file.

## Requirements

- Windows
- .NET 6 SDK
- NVIDIA Cg 3.1 runtime installed and available to the example application
- OpenGL-capable GPU/driver

Notes:

- `OpenCg.Examples` targets `net6.0-windows` and is built as `x86`.
- The examples expect the `.cg` shader files to be copied next to the executable during build.

## Build

From the repository root:

```powershell
dotnet build Primusz.OpenCg\Primusz.OpenCg.sln
```

## Run the examples

Start the examples project from Visual Studio, or run the built executable from:

```text
Primusz.OpenCg\OpenCg.Examples\bin\Debug\net6.0-windows\
```

The example launcher currently includes these samples:

- Vertex Program
- Fragment Program
- Uniform Parameter
- Varying Parameter
- Texture Sampling
- Vertex Twisting
- Two Texture Accesses
- Vertex Transform
- Vertex Lighting
- Fragment Lighting
- Two Lights with Structs
- Light Attenuation
- Spotlight
- Bulge

## Development notes

- The wrapper contains unsafe/native interop code, so marshalling correctness matters.
- Several APIs in the wrapper return pointers owned by the Cg runtime; these should be copied into managed data structures instead of being exposed directly as managed arrays.
- If you extend the examples, prefer time-based animation over frame-based animation so behavior stays stable across different frame rates.
