# Phone Number Tracer

A Windows desktop application for automated phone number tracing using keyboard simulation. This application helps automate the process of tracing multiple phone numbers by simulating keyboard inputs.

## Features

- Load phone numbers from a text file
- Automated command prompt interaction
- Keyboard simulation for tracing
- Global hotkeys for control
- Status updates and logging
- Pause/Resume functionality

## Requirements

- Windows operating system (Windows 10 or later recommended)
- .NET 6.0 Runtime

## Installation

1. Download the latest release (PhoneTracer.zip)
2. Extract all files to a folder on your Windows computer
3. Run PhoneTracer.exe

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
   - Open command prompt
   - Enter trace commands
   - Process each number in sequence

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
