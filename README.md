# CS_3502_Project_1

## Multi-Threaded Banking Simulation and IPC Producer-Consumer Model
### Overview 
This project consists of two major components:
1. **Multi-Threaded Banking Simulation** - A simulation of banking transactions, both individual and multi-account transfers, using multiple threads to concurrently manage deposits, withdrawals, and account transfers. Through integrating mutex locks, the program ensures proper synchronization and prevents race conditions and deadlocks while managing multiple customer threads.
2. **Inter-Process Communication (IPC) Producer-Consumer Model** - A custom IPC implementation using named pipes to facilitate communication and data transmission between a producer and consumer process. The producer generates random decimal numbers, transmits them via named pipes, and the consumer receives and processes the data while measuring transmission latency.

### Build and Run Instructions

#### Prerequisites
- .NET SDK (for running C# programs)
- A compatible C# compiler (Visual Studio Code, Visual Studio)
- Ubuntu (or a Linux-based distribution) for testing IPC features

#### Steps 
##### Multi-Threaded Banking Simulation
1. Copy the repository/source files (Folder CS_3502_Project_A) into your local computer.
2. Open the project in your C# development environment of choice (VS Code, Visual Studio)
3. Use the build and run commands provided by your IDE to compile and execute the program.

#### IPC Producer-Consumer Model
1. Copy the repository/source files for the two separate programs into your local computer (Folders CS_3502_Project_1_B and CS_3502_Project_1_B.2).
2. Open both projects in a separate window in your C# development environment.
3. Run the Producer in one instance of your IDE and the Consumer in another using your IDE's run options.

### Dependencies
- .NET SDK
- C# Standard Libraries
### Installation Instructions 
1. Install the .NET SDK from [Microsoft's .NET website](https://dotnet.microsoft.com/).
2. Verify the installation by running:
   ```sh
   dotnet --version
   ```
3. Use VS Code, Visual Studio, or any C#-compatible IDE to open the project and manage dependencies.

### Notes
- **Multi-Threaded Banking Simulation:** Uses mutex locks to prevent concurrency issues and race conditions such as deadlocks or priority inversion, and supports stress testing under high loads.
- **IPC Producer-Consumer Model:** Uses NamedPipeServerStream and NamedPipeClientStream for inter-process communication.
- **Error-Handling:** Both the banking simulation and producer-consumer programs gracefully handle errors including deadlocks and broken pipes by detecting issues and displaying relevant error messages.

For further details and notes, refer to the in-line comments contained in the project source code explaining synchronization and inter-process communication mechanisms. 
