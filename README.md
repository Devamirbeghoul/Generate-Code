# DataLayer (SQL Server) & BusinessLayer (C#) Generator

## Overview

This code generator creates a complete data access layer (DAL) and business logic layer (BLL) for SQL Server databases, following modern architectural patterns and best practices.

## Features

### DataLayer (SQL Server)
- **Auto-generated Entity Classes** - Strongly typed C# classes matching database tables
- **Repository Pattern Implementation** - CRUD operations for all entities
- **Connection Management** - Secure connection handling with retry logic
- **Bulk Operations** - High-performance bulk insert/update/delete

### BusinessLayer (C#)

- **Service Classes** - Business logic encapsulation

## Getting Started

### Prerequisites
- .NET 6.0+ SDK
- SQL Server 2016+
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
2. Configure connection strings
3. Run

## Generated Code Structure

```
Generated/
├── DataLayer/
│   ├── Entities/          # Database entity classes
│   ├── Settings/           # configuration
│
└── BusinessLayer/
    ├── DTOs/              # Data Transfer Objects
    ├── Services/          # Business services

```

## License

MIT License - Free for commercial and personal use