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
- [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) - **Required**

## Installation

1. Download [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and install it
2. Download the latest release (PhoneTracer.zip) from this repository
3. Extract all files to a folder on your Windows computer
4. Run PhoneTracer.exe

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
   - Enter the trace command with the phone number
   - Process each number in sequence

## Troubleshooting

If the application doesn't start:
1. Make sure you have installed [.NET 6.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Check the logs folder (created next to PhoneTracer.exe) for error messages
3. Ensure all files are extracted from the ZIP file to the same folder

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