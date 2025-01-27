# Windows Input Monitoring Method Tests

Tests various options for implementing input monitoring via the Windows API:
- Low level keyboard hook
 - RawInput
 - Keyboard hook (Only works for inputs to the app's window)
 - WM Message monitoring (Only shows messages to the app's window)

## Screenshot

<img width="913" alt="image" src="https://github.com/user-attachments/assets/d654b0c8-5143-42a9-b08e-41646d8be4ff" />

## How To Compile:

### Requirements:
 - Requires Visual Studio 2022 + The ".NET Desktop Development" workload
   - At the moment there are no external dependencies beyond the .NET Framework 4.8 built into windows (no need to download anything extra, install any Nuget packages, etc)
