# Phone Number Tracer

A Windows desktop application for automated phone number tracing using keyboard simulation. This application helps automate the process of tracing multiple phone numbers by simulating keyboard inputs.

## Features

- Load phone numbers from a text file
- Automated tracing with 't' key simulation
- Global hotkeys for control
- Status updates and logging
- Pause/Resume functionality

## Requirements

- Windows operating system (Windows 10 or later recommended)
- No additional installations required - everything is included!

## Installation

1. Download PhoneTracer-Release.zip from the latest release
2. Right-click the ZIP file and select "Extract All..."
3. Choose a destination folder
4. Double-click PhoneTracer.exe to run

## Usage

### Input File Format
Create a text file with phone numbers in the following format:
```
Name    PhoneNumber
John Smith    555-0123
Jane Doe    555-0124
```
Note: Names and phone numbers should be separated by a tab character.

### Controls
- **Load File**: Click to load a phone numbers file
- **Start Tracing**: Begin the tracing process
- **Hotkeys**:
  - Ctrl+O: Start tracing
  - Ctrl+H: Pause tracing
  - Ctrl+R: Restart tracing

### Running a Trace
1. Launch the application
2. Click "Load File" and select your phone numbers file
3. Click "Start Tracing" or use Ctrl+O to begin
4. The application will automatically:
   - Type 't' for trace command
   - Enter "/trace" followed by the phone number
   - Process each number in sequence

## Troubleshooting

If the application doesn't start or crashes:

1. Right-click PhoneTracer.exe and select "Properties"
2. If you see a "Security" warning, check "Unblock" and click OK
3. Make sure you extracted ALL files from the ZIP file
4. Check the logs folder (created next to PhoneTracer.exe) for error messages in:
   - error.log: Contains crash reports and error details
   - app.log: Contains general application activity

Common Issues:
- "File Missing" error: Make sure you extracted all files from the ZIP
- Application crashes: Check error.log for details
- Tracing not working: Ensure the target window is active when tracing starts

## Development

This application is built using:
- C# (.NET 6.0)
- Windows Forms
- Windows native keyboard simulation

### Building from Source
1. Clone the repository
2. Open in Visual Studio or preferred IDE
3. Build using .NET 6.0 SDK

## License

MIT License - Feel free to use and modify as needed.