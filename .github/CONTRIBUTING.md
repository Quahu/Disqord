## Contributing to Disqord

Thanks for your interest in contributing to Disqord! This document provides guidelines and instructions for contributing.

### Getting Started

1. Fork the repository on GitHub
2. Clone your fork locally
3. Create a new branch for your changes
4. Make your changes
5. Submit a pull request

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) or later
- An IDE with C# support (Visual Studio, Rider, VS Code with C# extension)

### Building

```bash
dotnet restore
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Code Style

- Follow existing code conventions in the project
- Use `private` setters for cached entity properties (update via `Update` methods instead)
- Use `EditorBrowsable(EditorBrowsableState.Never)]` on internal-facing public methods
- Keep nullable annotations enabled (`#nullable enable`)

### Pull Request Guidelines

- Keep PRs focused on a single change
- Include tests for new functionality when possible
- Update documentation if your change affects the public API
- Ensure all existing tests pass before submitting

### Reporting Issues

For bug reports and feature requests, please open an issue on GitHub or join the [support Discord server](https://discord.gg/eUMSXGZ).

### Areas for Contribution

- **Tests** - The test suite is minimal; adding tests for Gateway, REST, Bot, Voice, or Webhook components is highly valuable
- **Documentation** - Improve XML doc comments or examples
- **Bug fixes** - Check the issue tracker for reported bugs
- **TODO items** - Look for `TODO` comments in the codebase for known improvement areas

### License

By contributing to Disqord, you agree that your contributions will be licensed under the LGPL-3.0-only license.
