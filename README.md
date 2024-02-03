# TextFilter Solution Overview

This solution comprises two main projects: `TextFilter` and `TextFilter.Tests`. Below is an overview of each component and instructions on how to configure and run the application.

## Projects

### TextFilter

- **Extensions**: Contains the reflection-based code that identifies filter methods within `FilterMethods` and returns a list of `Func` delegates to be executed.
- **Filters**: This is where the filter methods are defined. If new filters are to be added, they should be placed here.
- **Interfaces**: Defines all interfaces used in the application. For larger projects, this could potentially be separated into its own project.
- **Services**: Includes two dependency injection (DI) services initiated at startup: `FilterService` and `FileReader`. `FileReader` is specifically designed as a service to facilitate testing.

#### Configuration and Running

- The application supports four modes of operation: Sequential with/without batching and Parallel with/without batching. However, only Sequential without batching is implemented and available.
- Configuration for the application is managed through `appsettings.Development.json`, where you can set the input file path, word separators, filter names, and operational mode.
- To run the application in the available mode, ensure `System::Parallel::Enabled` and `System::Batching::Enabled` are both set to `false` in `appsettings.Development.json`. Adjust `Application::InputFilePath` to the location of your input file.

### TextFilter.Tests

This project is structured into two main folders for different types of tests:
- **Unit Tests**: Tests for the three provided filters.
- **Integration Tests**: Includes tests that run the application using a test file (`words.txt`) containing the words provided within the pdf as TextInput. The tests compare the output of `app.Run()` against a predefined baseline.

#### Key Points

- Filters are implemented as extension methods within the `TextFilter` project and are not called directly by the main application. Instead, an indirection layer, driven by configuration, allows specifying filter names and their execution order. The filter names must match the method names exactly.
- Filters are loaded using reflection at runtime and are injected as singletons using the built-in Microsoft DI container.
- The application logs are divided into Application and System logs, configurable in `appsettings.Development.json`.
- The `Run()` method returns an array of strings with the filtered results, which are then output using `Console.WriteLine`. Note that this method will throw a `NotImplementedException` for the unimplemented operational modes as a design choice.

## Configuration Files

- **AppSettings.Development.json**: Controls application and system logging settings, including input file path, word separators, filter names, and operational mode settings.

## Enjoy!

To get started, simply configure the `appsettings.Development.json` as needed and run the application. Ensure the system configuration section keeps `Parallel::Enabled` and `Batching::Enable` set to `false` for the current implementation.
