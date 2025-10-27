# Reflection Search Plugin
[![Auto build](https://github.com/DKorablin/Plugin.ReflectionSearch/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/Plugin.ReflectionSearch/releases/latest)

A powerful plugin extension that enables deep reflection-based searching capabilities in .NET applications. This plugin allows you to search through object properties, fields, and methods using reflection, making it ideal for complex data exploration scenarios.

## Overview

The Reflection Search plugin serves as an extension for other plugins that need to implement specific data retrieval and search functionality. It provides a framework for searching through object structures using reflection, with support for various comparison operations and deep object traversal.

## Requirements

To implement this plugin in your application, you need to implement three required methods:

### 1. GetEntityType()
```csharp
Type GetEntityType()
```
Returns the type of the object that will be searched. This defines the root type for reflection-based searching.

### 2. GetSearchObjects(string folderPath)
```csharp
String[] GetSearchObjects(String folderPath)
```
Returns an array of searchable object paths/identifiers. These are typically file paths or other identifiers that can be used to instantiate the searchable objects.

### 3. CreateEntityInstance(object filePath)
```csharp
Object CreateEntityInstance(Object filePath)
```
Creates an instance of the searchable object using the provided path/identifier. The instance type must match the type returned by `GetEntityType()`.

## Features

- Deep reflection-based search through object hierarchies
- Support for various comparison operations (Equals, NotEquals, More, MoreOrEquals, Less, LessOrEquals, Contains)
- Ability to search through properties, fields, and methods
- Support for both primitive types and complex objects
- Automatic handling of enumerable types and collections
- Built-in support for type conversion and comparison

## Supported Plugins
- [PE Image View](https://github.com/DKorablin/Plugin.PEImageView)
- [ELF Image View](https://github.com/DKorablin/Plugin.ElfImageView)
- [APK Image View](https://github.com/DKorablin/Plugin.ApkImageView)

## Supported Frameworks

- .NET Framework 3.5
- .NET 8.0 Windows

## Usage Example

To use this plugin, implement the required methods in your plugin class:

```csharp
public class YourSearchPlugin : IPlugin
{
    public Type GetEntityType()
    {
        return typeof(YourSearchableType);
    }

    public string[] GetSearchObjects(string folderPath)
    {
        // Return array of searchable object identifiers
        return Directory.GetFiles(folderPath, "*.yourextension");
    }

    public object CreateEntityInstance(object filePath)
    {
        // Create and return an instance of your searchable type
        return new YourSearchableType((string)filePath);
    }
}
```

## License

This project is open source. For specific license terms, please see the LICENSE file in the repository.

## Contributing

Contributions are welcome! Please feel free to submit pull requests or create issues for bugs and feature requests.