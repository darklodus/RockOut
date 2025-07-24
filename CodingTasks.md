# RockOut Project Comprehensive Task List

This document enumerates the tasks required to implement the RockOut real-time audio equalizer described in `Agents.md`.

## 1. Repository and Environment Setup
- [ ] Install **.NET SDK** and **Visual Studio 2022** or equivalent.
- [ ] Create a new git repository/solution structure for `RockOut` with subprojects:
  - `RockOut.Core` (class library)
  - `RockOut.UI` (WPF application)
  - `RockOut.Tests` (unit/integration tests)
- [ ] Configure project build scripts (e.g., `dotnet build`, CI pipeline).
- [ ] Add required NuGet packages: `NAudio` or Windows audio libraries, `Serilog`, testing frameworks, etc.

## 2. Architecture and Design Artifacts
- [ ] Produce a **component diagram** covering the capture module, DSP engine, UI, and playback paths as referenced in `Agents.md`.
- [ ] Create a **class diagram** outlining the main classes and interfaces such as `AudioCaptureService`, `BiquadFilter`, `FilterChain`, `EqualizerViewModel`, etc.
- [ ] Create **sequence diagrams** for startup and filter change flows.
- [ ] Document interfaces (`IAudioCapture`, `IAudioPlayback`, `IMonoFilter`) and the planned data flow.

## 3. Core Module Implementation
### 3.1 Audio Capture Module
- [ ] Define `IAudioCapture` interface with `Start()`, `Stop()`, and `DataAvailable` event.
- [ ] Implement `AudioCaptureService` using **WASAPI loopback** or exclusive mode.
- [ ] Integrate with `WaveInProvider` or equivalent audio capture provider.

### 3.2 DSP Engine
- [ ] Create enumeration and data structures for **filter types** (low‑pass, high‑pass, band‑pass, notch, shelf, peaking).
- [ ] Implement `BiquadFilter` class with `SetParameters(f0, Q, gainDb, FilterType)` and `ProcessSample(float)` using Audio EQ Cookbook formulas.
- [ ] Build `FilterChain` to manage an ordered list of `IMonoFilter` with methods `AddFilter`, `RemoveFilter`, and `ProcessBuffer(float[] buffer)`.
- [ ] Design an overarching DSP engine service to apply the filter chain to incoming buffers.

### 3.3 Audio Playback Module
- [ ] Define `IAudioPlayback` interface with `Initialize()`, `Write(buffer)`, and `Dispose()`.
- [ ] Implement `AudioPlaybackService` using WASAPI/NAudio to output processed audio.
- [ ] Provide a `WaveOutProvider` or similar wrapper for playback.

### 3.4 User Interface Module
- [ ] Set up **WPF** project using **MVVM** pattern.
- [ ] Implement `EqualizerViewModel` and `BandViewModel` with slider binding (`BandViewModel.Gain`).
- [ ] Create `MainWindow.xaml` to host sliders and control elements.
- [ ] Implement UI commands such as `OpenFileCommand`, `PlayCommand`, and `StopCommand`.
- [ ] Connect UI events to the audio engine and capture/playback services.

### 3.5 Configuration and Presets
- [ ] Design JSON schema for presets (name, frequency, gain, Q).
- [ ] Implement `PresetManager` for loading/saving preset files.
- [ ] Implement `PresetSerializer` to handle JSON serialization/deserialization.
- [ ] Provide default preset examples (`Rock`, etc.) and UI for selecting presets.
- [ ] Validate preset files to protect against injection (per Security Considerations).

### 3.6 Logging and Diagnostics
- [ ] Integrate **Serilog** for structured logging.
- [ ] Log filter chain errors, latency metrics, and major application events.
- [ ] Configure log output location (file/console) and log rotation policy.

## 4. Data Flow Integration
- [ ] Connect capture callbacks to the DSP engine (`ProcessBuffer`).
- [ ] Send processed buffers from the DSP engine to the playback service.
- [ ] Update UI elements (e.g., meters/visualizations) based on processed data.

## 5. Error Handling and Stability
- [ ] Wrap all audio callbacks and buffer operations in `try/catch` blocks.
- [ ] Log recoverable errors and continue processing when possible.
- [ ] Display user-facing error messages for fatal issues and gracefully shut down services.

## 6. Performance Optimization
- [ ] Process audio in blocks of **512 samples** as stated in `Agents.md`.
- [ ] Utilize **System.Numerics SIMD** intrinsics in tight DSP loops.
- [ ] Minimize garbage collection by reusing audio buffers.

## 7. Testing Strategy
- [ ] Write **unit tests** for `BiquadFilter` to verify frequency response and parameter handling.
- [ ] Create **integration tests** that process WAV files through the DSP engine and verify output.
- [ ] Implement **UI automation tests** using tools such as FlaUI.

## 8. Security Measures
- [ ] Run the application strictly in user mode (avoid elevated privileges).
- [ ] Validate and sanitize all preset files loaded from disk.

## 9. Deployment and Packaging
- [ ] Package `RockOut.Core` as a **NuGet** library for potential external use.
- [ ] Configure an installer using **WiX Toolset** or **MSIX** for the full application.
- [ ] Document installation steps and include version information.

## 10. Documentation
- [ ] Expand the `README.md` with build/run instructions and feature overview.
- [ ] Document API usage for `RockOut.Core`.
- [ ] Provide examples of creating and applying presets.

## 11. Future Enhancements (Post‑MVP)
- [ ] Explore implementation of an **APO driver** for system‑wide exclusive‑mode EQ.
- [ ] Investigate wrapping the engine as a **VST plugin**.
- [ ] Add real‑time spectrum visualization (FFT) to the UI.
- [ ] Port the application to **Linux/macOS** using .NET Core if desired.

