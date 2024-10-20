# Basmus.Parametrization

[![NuGet](https://img.shields.io/nuget/v/Basmus.Parametrization.svg)](https://www.nuget.org/packages/Basmus.Parametrization)
[![Tests](https://github.com/basmus/parametrization/actions/workflows/run-tests.yaml/badge.svg)](https://github.com/basmus/parametrization/actions/workflows/run-tests.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A lightweight .NET library that enables parameter extraction directly from C# interpolated strings using `FormattableString`, eliminating the need for separate parameter mapping structures.

## Features

- Direct parameter extraction from interpolated strings without additional value-placeholder mapping
- Zero-parsing approach using native `System.FormattableString`
- Supports both named and positional parameterization
- Processes nested interpolation strings
- Provides full control on placeholder formatting by `PlaceholderBuilder`
    - name conflict resolving
    - name auto generation

## Installation

```bash
dotnet add package Basmus.Parametrization
```

## Quick Start

The `ParameterHelper` class is the main entry point for parameter extraction. It provides two simple methods for working with interpolated strings.

### Named Parameterization

```csharp
var id = 1;
var name = "John";
var date = DateTime.Now;

FormattableString fs = $"User {name.AsParameter("name")} with ID {id.AsParameter("id")} was created on {date.AsParameter()}";
// Original: User John with ID 1 was created on 18.04.2025 15:54:16

var parameters = ParameterHelper.Parametrize(fs, new MyNamedPlaceholderBuilder());

// Parametrized: User @name with ID @id was created on @p0
// Parameters:
// @name: John
// @id: 1
// @p0: 18.04.2025 15:54:16
```

### Positional Parameterization

```csharp
var id = 1;
var name = "John";

FormattableString fs = $"User {name.AsParameter()} with ID {id.AsParameter()}";
// Original: User John with ID 1

var parameters = ParameterHelper.Parametrize(fs, new MyPositionalPlaceholderBuilder());

// Parametrized: User $0 with ID $1
// Parameters:
// [0]: John
// [1]: 1
```

## Performance

The library has been benchmarked for different parameter set sizes with both Named and Positional parametrizers.

### Named Parametrizer

Method            | Mean        | Allocated
----------------- | ----------- | ---------
'1000 parameters' | 342.670 μs | 169.86 KB
'100 parameters'  | 38.295 μs  | 17.12 KB 
'10 parameters'   | 4.668 μs   | 2.19 KB

[Details](tests/Basmus.Parametrization.Benchmarks/artifacts/results/Basmus.Parametrization.Benchmarks.NamedParametrizerBenchmarks-report-github.md)

### Positional Parametrizer

Method            | Mean        | Allocated
----------------- | ----------- | ---------
'1000 parameters' | 236.895 μs | 102.72 KB
'100 parameters'  | 25.677 μs  | 10.36 KB 
'10 parameters'   | 3.241 μs   | 1.09 KB

[Details](tests/Basmus.Parametrization.Benchmarks/artifacts/results/Basmus.Parametrization.Benchmarks.PositionalParametrizerBenchmarks-report-github.md)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright © 2025 Basmus
