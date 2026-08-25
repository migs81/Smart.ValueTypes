# 📚 Smart.ValueTypes
A high-performance .NET library providing strongly typed ValueTypes with built-in format validation.

## 📝 Description
A .NET library providing high-performance, strongly typed ValueTypes with a focus on balancing performance with clean, readable, and maintainable code. Each ValueType includes built-in format validation with clear and consistent validation feedback through dedicated enum values, while avoiding nullable return values wherever possible. The goal is to provide reliable, expressive types that make invalid values difficult to represent without sacrificing performance or code quality.

## ✨ Features
 - High-performance ValueTypes with a focus on efficiency
 - Built-in format validation for reliable input handling
 - Clear validation feedback using dedicated enum values

## ✅ Validation

This library validates the format of each value type only.

For example, an `IBAN` is checked for its format and checksum.
It is **not** checked against a list of country codes, banks, or existing accounts.

In general, a value type only validates what can be determined from its own value.
Any checks that require external data are outside the scope of this library.

## 🧩 Available ValueTypes
The library currently provides ValueTypes for:

- **🏠 Address:** Postal and administrative address-related types
- **🏦 Bank:** Banking and financial identifiers
- **💬 Communication:** Email addresses and communication-related types
- **📐 Coordinates:** Geographic coordinates and positions
- **🆔 Identifiers:** UUIDs, ULIDs, IMEI, IMSI, and other identifiers
- **💾 IO:** File paths and other input/output-related types
- **🌡️ Temperatures:** Temperature values and units
- **🔐 Security:** Passwords, tokens, hashes, and security-related types
- **🔢 Units & Numbers:** Ranges, percentages, bounded values, and numeric types
- **🌐 Web:** URLs, domains, and web-related types
- **📝 Text:** General text-related types such as NonEmptyString

## 🛠️ Prerequisites

- .NET SDK (6.0 or 8.0) or a compatible IDE (Visual Studio, Rider, etc.).

## 📦 Installation

1. Clone the repository:
  ```bash
   git clone https://github.com/migs81/Smart.ValueTypes.git
  ```
2. Navigate to the project directory:
  ```bash
   cd Smart.ValueTypes
  ```
3. Restore dependencies:
  ```bash
   dotnet restore
  ```
4. Build the project:
  ```bash
   dotnet build
  ```

## 💻 Usage

1. Example: Create an EmailAddress directly. Throws an exception if the value is invalid.
```bash
var mail = new EmailAddress("user@example.com");
Console.WriteLine(mail);
```

2. Example: Create an EmailAddress using a static factory method. Throws an exception if the value is invalid.
```bash
var mail2 = EmailAddress.From("user@example.com");
Console.WriteLine(mail2);
```

3. Example: Create an EmailAddress without throwing an exception and return the validation result through an enum.
```bash
var result = EmailAddress.TryFrom("user@example.com", out var mail3);
if (result != EmailAddress.Validation.Ok)
    Console.WriteLine(result);

Console.WriteLine(mail3);
```

4. Example: Validate the format of an email address before creating an EmailAddress instance.
```bash
var text = "user@example.com";
if (EmailAddress.ValidateFormat(text) == EmailAddress.Validation.Ok)
{
    var mail4 = EmailAddress.From(text);
}
```

## 📊 Benchmarks

This project includes a benchmark project to measure the performance of the library.
You can find it in the `/benchmarks/` folder.

### Running Benchmarks

1. Navigate to the benchmark project:
  ```bash
   cd benchmarks/Smart.ValueTypes.Benchmarks
  ```
2. Run the benchmarks using [BenchmarkDotNet](https://benchmarkdotnet.org/):
  ```bash
   dotnet run -c Release
  ```

## 📂 Project Structure

```
Smart.ValueTypes/
├── src/
│   ├── Smart.ValueTypes/              # Core library
├── benchmarks/                       
│   └── Smart.ValueTypes.Benchmarks    # Benchmarks
├── tests/                       
│   └── Smart.ValueTypes.UnitTests         # Unit tests
└── README.md                    # This file
```

## 📄 License

This project is licensed under the [MIT License](LICENSE).