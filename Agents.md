# RockOut Software Design Document

## Revision History
| Date       | Version | Description                    | Author           |
|------------|---------|--------------------------------|------------------|
| 2025-07-24 | 0.1     | Initial draft                  | Joseph Reynolds |

## Table of Contents
1. [Introduction](#introduction)
2. [Purpose and Scope](#purpose-and-scope)
3. [Definitions and Acronyms](#definitions-and-acronyms)
4. [System Architecture](#system-architecture)
   - 4.1 [High-Level Overview](#high-level-overview)
   - 4.2 [Component Diagram](#component-diagram)
5. [Detailed Design](#detailed-design)
   - 5.1 [Audio Capture Module](#audio-capture-module)
   - 5.2 [DSP Engine](#dsp-engine)
     - 5.2.1 [Filter Types](#filter-types)
     - 5.2.2 [Biquad Filter Implementation](#biquad-filter-implementation)
     - 5.2.3 [Filter Chain Management](#filter-chain-management)
   - 5.3 [User Interface Module](#user-interface-module)
   - 5.4 [Audio Playback Module](#audio-playback-module)
   - 5.5 [Configuration and Presets](#configuration-and-presets)
   - 5.6 [Logging and Diagnostics](#logging-and-diagnostics)
6. [Data Flow](#data-flow)
7. [Interfaces and APIs](#interfaces-and-apis)
8. [Class Diagram](#class-diagram)
9. [Sequence Diagrams](#sequence-diagrams)
10. [Error Handling and Exceptions](#error-handling-and-exceptions)
11. [Performance and Optimization](#performance-and-optimization)
12. [Testing Strategy](#testing-strategy)
13. [Security Considerations](#security-considerations)
14. [Deployment and Packaging](#deployment-and-packaging)
15. [Future Enhancements](#future-enhancements)

---

## 1. Introduction
The RockOut application is a real-time, software-based audio equalizer for Windows. It captures system audio, processes it through configurable DSP filters, and outputs the modified stream back to the audio subsystem. The design leverages C#/.NET, WPF for UI, and WASAPI (or APO) for audio I/O.

## 2. Purpose and Scope
**Purpose:** Provide a clear blueprint for developers to implement the RockOut equalizer.
**Scope:** Covers system-wide user-mode implementation; outlines optional APO driver for exclusive-mode equalization.

## 3. Definitions and Acronyms
- **WASAPI**: Windows Audio Session API
- **APO**: Audio Processing Object
- **DSP**: Digital Signal Processing
- **UI**: User Interface
- **EQ**: Equalizer

## 4. System Architecture

### 4.1 High-Level Overview
```
+------------+      +-------------+      +-------------+
| Audio      | ---> | DSP Engine  | ---> | Audio       |
| Capture    |      | (Filters)   |      | Playback    |
+------------+      +-------------+      +-------------+
        |                                 /
        |                                /
        +-------> Configuration/UI <----+
```

### 4.2 Component Diagram
(Insert UML component diagram reference or ASCII placeholder)

## 5. Detailed Design

### 5.1 Audio Capture Module
- **Responsibility:** Capture system audio streams via WASAPI loopback or exclusive mode.
- **Key Classes:** `AudioCaptureService`, `WaveInProvider`
- **Main Interfaces:** `IAudioCapture` with `Start()`, `Stop()`, `DataAvailable` event.

### 5.2 DSP Engine

#### 5.2.1 Filter Types
- Low-pass, high-pass, band-pass, notch, shelf, peaking EQ.
- User-selectable Q-factor and gain.

#### 5.2.2 Biquad Filter Implementation
- Use coefficients from the Audio EQ Cookbook.
- Class: `BiquadFilter` with methods `SetParameters(f0, Q, gainDb, FilterType)` and `ProcessSample(float)`.

#### 5.2.3 Filter Chain Management
- Class: `FilterChain` holds an ordered list of `IMonoFilter`.
- Methods: `ProcessBuffer(float[] buffer)`, `AddFilter()`, `RemoveFilter()`.

### 5.3 User Interface Module
- **Framework:** WPF + MVVM
- **ViewModels:** `EqualizerViewModel`, `BandViewModel`
- **Views:** `MainWindow.xaml` with sliders bound to `BandViewModel.Gain`.
- **Commands:** `OpenFileCommand`, `PlayCommand`, `StopCommand`.

### 5.4 Audio Playback Module
- **Responsibility:** Send processed audio buffer to output.
- **Key Classes:** `AudioPlaybackService`, `WaveOutProvider`
- **Interfaces:** `IAudioPlayback` with `Initialize()`, `Write(buffer)`, `Dispose()`.

### 5.5 Configuration and Presets
- JSON-based preset files:
  ```json
  {
    "name": "Rock",
    "bands": [ {"f0": 100, "gain": 5, "Q": 1}, ... ]
  }
  ```
- Classes: `PresetManager`, `PresetSerializer`

### 5.6 Logging and Diagnostics
- Use `Serilog` for structured logging.
- Capture filter chain errors, latency metrics.

## 6. Data Flow
1. Capture callback delivers PCM buffer.
2. Buffer passed to `DSP Engine.ProcessBuffer()`.
3. Processed buffer enqueued to playback.
4. UI thread updates visualization (spectrum analyzer).

## 7. Interfaces and APIs
- `IAudioCapture`, `IAudioPlayback`, `IMonoFilter`.
- Public APIs in `RockOut.Core.dll` for integration.

## 8. Class Diagram
(Provide UML class diagram or ASCII summary)

## 9. Sequence Diagrams
- Startup: UI -> AudioCaptureService -> DSP Engine -> AudioPlaybackService
- Filter change: UI event -> BandViewModel -> FilterChain.Update()

## 10. Error Handling and Exceptions
- All audio callbacks wrapped in try/catch.
- On recoverable error, log and continue; on fatal, display user message.

## 11. Performance and Optimization
- Process in blocks of 512 samples.
- Use SIMD intrinsics (System.Numerics) in hot loops.
- Minimize GC by reusing buffers.

## 12. Testing Strategy
- Unit tests for `BiquadFilter` frequency response.
- Integration tests with WAV file processing.
- UI tests via automation (e.g., FlaUI).

## 13. Security Considerations
- Run in user mode; limited attack surface.
- Validate preset files to avoid injection.

## 14. Deployment and Packaging
- NuGet packaging for core library.
- Installer using `WiX Toolset` or `MSIX`.

## 15. Future Enhancements
- APO driver for system-wide exclusive-mode EQ.
- VST plugin wrapper.
- Real-time visualization (FFT).
- Cross-platform port to Linux/macOS using .NET Core.

