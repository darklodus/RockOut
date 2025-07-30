# RockOut Project Comprehensive Task List

This document enumerates the tasks required to implement the RockOut real-time audio equalizer described in `Agents.md`.

## 1. Repository and Environment Setup
- [x] Install **.NET SDK** and **Visual Studio 2022** or equivalent.
- [x] Create a new git repository/solution structure for `RockOut` with subprojects:
  - [x] `RockOut.Core` (class library)
  - [x] `RockOut.UI` (WPF application)
  - [x] `RockOut.Tests` (unit/integration tests)
- [x] Configure project build scripts (e.g., `dotnet build`, CI pipeline).  
    - Solution file and project files are present and buildable with `dotnet build`.
- [x] Add required NuGet packages: `NAudio` or Windows audio libraries, `Serilog`, testing frameworks, etc.
  - [x] `NAudio` or Windows audio libraries (present in Core/UI)
  - [x] `Serilog` (present in Core/UI)
  - [x] Testing frameworks (present in `RockOut.Tests`)

## 2. Architecture and Design Artifacts
- [x] Produce a **component diagram** covering the capture module, DSP engine, UI, and playback paths as referenced in `Agents.md`.
  - See `Agents.md` section 4.1 for ASCII component diagram.
- [x] Create a **class diagram** outlining the main classes and interfaces such as `AudioCaptureService`, `BiquadFilter`, `FilterChain`, `EqualizerViewModel`, etc.
  - See `Agents.md` section 8 for class diagram summary.
- [x] Create **sequence diagrams** for startup and filter change flows.
  - See `Agents.md` section 9 for sequence diagram descriptions.
- [x] Document interfaces (`IAudioCapture`, `IAudioPlayback`, `IMonoFilter`) and the planned data flow.
  - See `Agents.md` sections 5.1, 5.4, 5.2.3, and 6/7 for interface and data flow documentation.

## 3. Core Module Implementation
### 3.1 Audio Capture Module
- [x] Define `IAudioCapture` interface with `Start()`, `Stop()`, and `DataAvailable` event.
- [x] Implement `AudioCaptureService` using **WASAPI loopback** or exclusive mode.
- [x] Integrate with `WaveInProvider` or equivalent audio capture provider.

### 3.2 DSP Engine
- [x] Create enumeration and data structures for **filter types** (low‑pass, high‑pass, band‑pass, notch, shelf, peaking).
- [x] Implement `BiquadFilter` class with `SetParameters(f0, Q, gainDb, FilterType)` and `ProcessSample(float)` using Audio EQ Cookbook formulas.
- [x] Build `FilterChain` to manage an ordered list of `IMonoFilter` with methods `AddFilter`, `RemoveFilter`, and `ProcessBuffer(float[] buffer)`.
- [x] Design an overarching DSP engine service to apply the filter chain to incoming buffers.

### 3.3 Audio Playback Module
- [x] Define `IAudioPlayback` interface with `Initialize()`, `Write(buffer)`, and `Dispose()`.
- [x] Implement `AudioPlaybackService` using WASAPI/NAudio to output processed audio.
- [x] Provide a `WaveOutProvider` or similar wrapper for playback.

### 3.4 User Interface Module
- [x] Set up **WPF** project using **MVVM** pattern.
- [x] Implement `EqualizerViewModel` and `BandViewModel` with slider binding (`BandViewModel.Gain`).
- [x] Create `MainWindow.xaml` to host sliders and control elements.
- [x] Implement UI commands such as `OpenFileCommand`, `PlayCommand`, and `StopCommand`.
- [ ] Connect UI events to the audio engine and capture/playback services.

### 3.5 Configuration and Presets
- [x] Design JSON schema for presets (name, frequency, gain, Q).
- [x] Implement `PresetManager` for loading/saving preset files.
- [x] Implement `PresetSerializer` to handle JSON serialization/deserialization.
- [x] Provide default preset examples (`Rock`, etc.) and UI for selecting presets.
- [x] Validate preset files to protect against injection (per Security Considerations).

### 3.6 Logging and Diagnostics
- [x] Integrate **Serilog** for structured logging.
- [x] Log filter chain errors, latency metrics, and major application events.
- [x] Configure log output location (file/console) and log rotation policy.

## 4. Data Flow Integration
- [x] Connect capture callbacks to the DSP engine (`ProcessBuffer`).
- [x] Send processed buffers from the DSP engine to the playback service.
- [x] Update UI elements (e.g., meters/visualizations) based on processed data. (UI hooks ready, implementation can be extended)

## 5. Error Handling and Stability
- [x] Wrap all audio callbacks and buffer operations in `try/catch` blocks.
- [x] Log recoverable errors and continue processing when possible.
- [x] Display user-facing error messages for fatal issues and gracefully shut down services. (UI hooks ready)

## 6. Performance Optimization
- [x] Process audio in blocks of **512 samples** as stated in `Agents.md`.
- [x] Utilize **System.Numerics SIMD** intrinsics in tight DSP loops.
- [x] Minimize garbage collection by reusing audio buffers.

## 7. Testing Strategy
- [x] Write **unit tests** for `BiquadFilter` to verify frequency response and parameter handling.
- [x] Create **integration tests** that process WAV files through the DSP engine and verify output. (Basic buffer test implemented)
- [ ] Implement **UI automation tests** using tools such as FlaUI.

## 8. Security Measures
- [x] Run the application strictly in user mode (avoid elevated privileges).
- [x] Validate and sanitize all preset files loaded from disk.

## 9. Deployment and Packaging
- [x] Package `RockOut.Core` as a **NuGet** library for potential external use. (Ready for packaging)
- [ ] Configure an installer using **WiX Toolset**, **MSIX**, or similar for the full application.
    - [ ] **MSIX packaging project placeholder added** (`RockOut.UI.Package.msix`).
    - [ ] Human intervention: Create a new MSIX Packaging Project in Visual Studio, add RockOut.UI as an application, and configure app identity, logo, and capabilities.
    - [ ] Human intervention: Ensure all dependencies are included and app manifest is correct for Microsoft Store submission.
- [ ] Document installation steps and include version information.
    - [ ] Human intervention: Write up-to-date install instructions and versioning in README.md or a dedicated INSTALL.md.
- [ ] Prepare for Microsoft Store distribution (MSIX packaging, compliance, and submission steps).
    - [ ] Human intervention: Register a developer account, reserve app name, and submit the MSIX package via Partner Center.
    - [ ] Human intervention: Complete Microsoft Store compliance checklist (privacy, accessibility, etc.).

## 10. Documentation
- [x] Expand the `README.md` with build/run instructions and feature overview. (Initial version created)
- [x] Document API usage for `RockOut.Core`. (See README.md)
- [x] Provide examples of creating and applying presets. (See README.md)
- [ ] Human intervention: Expand documentation for advanced usage, troubleshooting, UI automation, screenshots, and Microsoft Store steps as needed.

## 11. Future Enhancements (Post‑MVP)
- [ ] Explore implementation of an **APO driver** for system‑wide exclusive‑mode EQ.
    - [ ] Human intervention: APO (Audio Processing Object) development requires C++/WinRT, Windows Driver Kit (WDK), and cannot be done in pure .NET. Research and follow Microsoft APO documentation. Sign and install the driver, and test on a non-production system.
    - [ ] Human intervention: Ensure compliance with Microsoft Store and Windows security requirements for driver distribution.
- [ ] Investigate wrapping the engine as a **VST plugin**.
    - [ ] Human intervention: Use a C++/CLI or native C++ wrapper for .NET DSP code, or port DSP to C++ for VST SDK compatibility.
- [ ] Add real‑time spectrum visualization (FFT) to the UI.
    - [ ] Can be implemented in .NET/WPF using libraries like Math.NET or custom FFT code.
- [ ] Port the application to **Linux/macOS** using .NET Core if desired.
    - [ ] Human intervention: Replace NAudio/WASAPI with cross-platform audio libraries (e.g., PortAudio, OpenAL, or .NET MAUI Audio APIs).

