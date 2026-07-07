# 📚 Smart.ValueTypes


## 📝 Description


## ✨ Features



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

## ✅ Validation

This library validates the format of each value type only.

For example, an `Iban` is checked for its format and checksum. 
It is **not** checked against a list of country codes, banks, or existing accounts.

In general, a value type only validates what can be determined from its own value. 
Any checks that require external data are outside the scope of this library.

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

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## 📄 License

This project is licensed under the [MIT License](LICENSE).