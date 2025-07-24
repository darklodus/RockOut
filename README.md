# RockOut

RockOut is a real-time audio equalizer for Windows. The application captures system audio, processes it through a configurable Digital Signal Processing (DSP) engine, and plays back the modified stream. Core components are implemented in C#/.NET with a WPF front end.

## Features
- WASAPI-based audio capture and playback
- Biquad filter implementation with support for common EQ filter types
- Configurable filter chains and presets stored in JSON
- Structured logging via Serilog
- Future support for system-wide APO driver and VST plugin integration

## Building
1. Open the solution in Visual Studio 2022 or run `dotnet build`.
2. Run the application to capture and modify system audio in real time.

See `Agents.md` for the complete software design document.
