# Orc.Controls Development Guidelines

## Build and Test Commands
- Build solution: `dotnet build src/Orc.Controls.sln`
- Run all tests: `dotnet test src/Orc.Controls.Tests/Orc.Controls.Tests.csproj`
- Run specific test: `dotnet test src/Orc.Controls.Tests/Orc.Controls.Tests.csproj --filter "FullyQualifiedName~TestName"`
- Run tests with NUnit console: `nunit3-console src/Orc.Controls.Tests/bin/Debug/net8.0-windows/Orc.Controls.Tests.dll`

## Code Style Guidelines
- Use latest C# language features (`LangVersion=latest`)
- Treat warnings as errors
- Use nullable reference types
- Follow C# naming conventions (PascalCase for public members, camelCase for private/local)
- Match surrounding code style for consistency
- Use WPF MVVM pattern with proper separation of concerns
- Add XML documentation for public APIs
- Use dependency injection where appropriate
- Follow IDisposable pattern for disposable resources
- Follow async/await best practices (avoid async void, use ConfigureAwait)
- Create unit tests for new functionality
- Follow existing code organization patterns (folders/namespaces)