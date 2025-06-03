# Patient Prescription Management - Test Project

This directory contains unit tests for the Patient Prescription Management backend application.

## Test Structure

The test project is organized into the following test files:

1. **PrescriptionServiceTests.cs** - Tests for the `PrescriptionService` class, focusing on:
   - Dosage validation logic
   - Business rule enforcement

2. **PrescriptionControllerTests.cs** - Tests for the `PrescriptionsController` class, covering:
   - CRUD operations (Create, Read, Update, Delete)
   - Response types and status codes
   - Error handling

3. **ValidationTests.cs** - Tests for the `Prescription` model validation, including:
   - Required field validation
   - String length validation
   - Date validation
   - Business rule validation

## Running Tests

To run the tests, navigate to the project root directory and execute:

```bash
dotnet test ServerTests/ServerTests.csproj
```

Or run specific test files:

```bash
dotnet test ServerTests/ServerTests.csproj --filter "FullyQualifiedName~PrescriptionServiceTests"
dotnet test ServerTests/ServerTests.csproj --filter "FullyQualifiedName~PrescriptionControllerTests"
dotnet test ServerTests/ServerTests.csproj --filter "FullyQualifiedName~ValidationTests"
```

## Test Coverage

The tests cover:

- **Service Layer**: Business logic validation and data processing
- **Controller Layer**: API endpoints, request/response handling
- **Model Layer**: Data validation and constraints

## Dependencies

- **NUnit**: Testing framework
- **Moq**: Mocking framework for isolating components
- **Microsoft.NET.Test.Sdk**: Required for test discovery and execution
- **NUnit3TestAdapter**: Adapter for running NUnit tests
- **coverlet.collector**: Code coverage collection

## Guidelines for Adding New Tests

When adding new tests:

1. Follow the Arrange-Act-Assert pattern
2. Use descriptive test names that explain what is being tested
3. Test both positive and negative scenarios
4. Mock external dependencies
5. Keep tests independent of each other
6. Aim for high code coverage

## Troubleshooting

If tests fail to run:

1. Ensure all NuGet packages are restored (`dotnet restore`)
2. Check that the test project references the main project
3. Verify that test classes and methods have the appropriate NUnit attributes
4. Clean and rebuild the solution (`dotnet clean` followed by `dotnet build`)
