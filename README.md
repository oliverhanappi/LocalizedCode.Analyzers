# LocalizedCode.Analyzers

[![Nuget](https://img.shields.io/nuget/v/LocalizedCode.Analyzers)](https://www.nuget.org/packages/LocalizedCode.Analyzers)

Embracing principles of domain-driven design leads to the definition of a
ubiquitous language, which should be used by all stake holders.

Although code is usually written in English, in non English environments it
is desirable to write domain specific code in the language used by domain experts,
thus in the ubiquitous language.

This project provides Roslyn analyzers for C# which aid writing code in non English
languages. Please note that this project is not targeted at writing localized software
whose code is written in English.

## Installation

You need to add the NuGet package `LocalizedCode.Analyzers` to the project containing
the localized code. Please note that only the new SDK based projects are supported.

## Diagnostics

### Declaration with non-ASCII identifier (LC1000)

Although it it desirable to write code in the ubiquitous language, using characters
other than ASCII characters in identifiers may lead to compatibility issues in some
parts of the system, i.e. URLs or databases.

Therefore the analyzer produces warnings if code items like namespaces, classes,
members etc. contain non-ASCII characters within their identifiers.

A quick fix is provided to replace the non-ASCII characters with ASCII approximations.

Please note that this diagnostics is only useful for Latin languages.

