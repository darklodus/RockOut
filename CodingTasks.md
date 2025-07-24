# Coding Tasks for RockOut Project

This document lists all tasks required to build and complete the RockOut audio equalizer application described in `Agents.md`.

## 1. Environment Setup
- [ ] Install latest .NET SDK with WPF support
- [ ] Install Visual Studio 2022 or configure `dotnet` CLI
- [ ] Create a solution with projects:
  - `RockOut.Core` (class library)
  - `RockOut.UI` (WPF application)
  - `RockOut.Tests` (unit tests)
- [ ] Add NuGet dependencies: `Serilog`, any WASAPI wrappers, `System.Numerics`

## 2. Audio Capture Module
- [ ] Define `IAudioCapture` interface with `Start()`, `Stop()`, and `DataAvailable` event
- [ ] Implement `AudioCaptureService` using WASAPI loopback (and optional exclusive mode)
- [ ] Implement or integrate `WaveInProvider`
- [ ] Handle errors and log via Serilog
- [ ] Write unit tests for audio capture functionality

## 3. DSP Engine
- [ ] Define `IMonoFilter` interface
- [ ] Implement filter types (low‑pass, high‑pass, band‑pass, notch, shelf, peaking)
- [ ] Implement `BiquadFilter` using Audio EQ Cookbook formulas
- [ ] Implement `FilterChain` with `AddFilter()`, `RemoveFilter()`, and `ProcessBuffer()`
- [ ] Create DSP engine class that manages the filter chain and processes blocks of 512 samples
- [ ] Optimize critical loops using `System.Numerics` SIMD

## 4. User Interface Module
- [ ] Set up MVVM structure
- [ ] Create `EqualizerViewModel` and `BandViewModel`
- [ ] Build `MainWindow.xaml` with sliders bound to band gain values
- [ ] Implement commands: `OpenFile`, `Play`, and `Stop`
- [ ] Connect UI actions to audio capture, DSP engine, and playback services
- [ ] (Optional) Implement real‑time visualization such as a spectrum analyzer

## 5. Audio Playback Module
- [ ] Define `IAudioPlayback` interface with `Initialize()`, `Write(buffer)`, and `Dispose()`
- [ ] Implement `AudioPlaybackService` using WASAPI output
- [ ] Manage buffered audio queue for smooth playback

## 6. Configuration and Presets
- [ ] Design JSON format for EQ presets
- [ ] Implement `PresetManager` to load and save presets
- [ ] Implement `PresetSerializer`
- [ ] Provide example presets (e.g., Rock, Jazz, Classical)

## 7. Logging and Diagnostics
- [ ] Configure Serilog for structured logging to file and console
- [ ] Log filter chain operations, latency metrics, and errors
- [ ] Expose diagnostics information in the UI if useful

## 8. Data Flow Integration
- [ ] Connect audio capture → DSP engine → playback pipeline
- [ ] Ensure processing is performed in a separate thread or async pipeline
- [ ] Verify low latency and correct buffer handoff

## 9. Interfaces and Public APIs
- [ ] Document `IAudioCapture`, `IAudioPlayback`, and `IMonoFilter`
- [ ] Expose public APIs through `RockOut.Core.dll` for potential integration by other applications

## 10. Testing Strategy
- [ ] Create unit tests for `BiquadFilter` frequency response and coefficient calculations
- [ ] Create integration tests that run a WAV file through the filter chain
- [ ] Add UI automation tests using a tool like FlaUI
- [ ] Set up continuous integration to run tests on every commit

## 11. Error Handling
- [ ] Wrap audio callbacks in try/catch blocks
- [ ] Log recoverable errors and continue processing
- [ ] Display user‑friendly messages for unrecoverable errors

## 12. Performance Optimization
- [ ] Benchmark processing latency and CPU usage
- [ ] Minimize garbage collection by reusing buffers
- [ ] Tune SIMD implementations for maximum throughput

## 13. Security Considerations
- [ ] Validate preset files against a schema to prevent injection attacks
- [ ] Run in user mode with minimal privileges

## 14. Deployment and Packaging
- [ ] Create a NuGet package for `RockOut.Core`
- [ ] Build an installer using WiX Toolset or MSIX
- [ ] Document installation steps in `README.md`

## 15. Documentation
- [ ] Produce UML component and class diagrams
- [ ] Add sequence diagrams for startup and filter‑change scenarios
- [ ] Update `README.md` with build and usage instructions

## 16. Future Enhancements
- [ ] Investigate APO driver for exclusive‑mode equalization
- [ ] Explore wrapping the DSP engine as a VST plugin
- [ ] Plan for cross‑platform support using .NET Core

