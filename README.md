# Configuration.Utils

This is a library/package focused on functionalities related to the validation of the application's configuration.

## Table of Content

- [**Features**](#features)
- [**Installation**](#installation)
- [**Usage / Implementation**](#usage--implementation)
- [**Contribution**](#contribution)
  - [**Test**](#test)

## Features

- **Object Validations**: Allows efficient and fast validation of objects that use **DataAnnotations** in their class.
- **Custom Message**: Once an object is validated and an error is found, the message provided offers a clean and good message.

## Installation

You can install the library/package using **NuGet**. Open the terminal and execute the following command:

```bash
dotnet add package Configuration.Utils
```

## Usage / Implementation

**_This is an example focused on validating values to be replaced in the project's configuration_**

Assuming you have the following values in your `appsettings.json`:

```json
{
  "CoreSettings": {
    "Host": "https://example.com"
  }
}
```

And also, you have the following **Settings** class:

```csharp
using System.ComponentModel.DataAnnotations;

public class CoreSettings
{
    [Required(ErrorMessage = "Core Host must be configured")]
    public string Host { get; init; } = string.Empty;

    [Required(ErrorMessage = "Core Port must be configured")]
    public int Port { get; init; }
}
```

You can then proceed to use the extension in your `Program.cs` file:

```csharp
using Configuration.Utils;

builder.Services.Configure<CoreSettings>(builder.Configuration.GetSection("CoreSettings"));
builder.Configuration.ValidateConfiguration<CoreSettings>("CoreSettings"); // Defined Extension Method
```

The `ValidateConfiguration` method is implemented in [ConfigurationValidationExtension](./src/Configuration.Utils/Extensions/ConfigurationValidationExtension.cs), and in this case it would throw an **exception** of type `InvalidConfigurationException` with a message like the following:

```txt
Invalid Configuration for '{CoreSettings}':

- Core Port must be configured
```

## Contribution

Contributions are welcome! Please fork the repository and submit a pull request.

### Test

This project uses [**xUnit**](https://xunit.net) for **Unit Tests**.

To run the tests:

```bash
cd src\Configuration.Utils.Tests
dotnet test
```
