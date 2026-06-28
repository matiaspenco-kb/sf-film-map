# Quick Start Guide for SF Film Map Tests

## Project Layout

```
C:\repos\sf-film-map\
├── backend\
│   ├── Controllers\
│   │   └── FilmLocationsController.cs
│   ├── Services\
│   │   └── FilmLocationService.cs
│   ├── Models\
│   │   └── FilmLocation.cs
│   ├── SFFilmMap.csproj
│   └── Data\
│       └── Film_Locations_in_San_Francisco.csv
│
└── SFFilmMap.Tests\
	├── Controllers\
	│   └── FilmLocationsControllerTests.cs  (5 tests)
	├── Services\
	│   └── FilmLocationServiceTests.cs      (33 tests)
	├── SFFilmMap.Tests.csproj
	└── README.md
```

## What Was Built

✅ **Test Project**: SFFilmMap.Tests (net8.0)
✅ **Framework**: xUnit with Moq mocking
✅ **Assertions**: FluentAssertions
✅ **Total Tests**: 38
✅ **Pass Rate**: 100%

## Test Files

### FilmLocationsControllerTests.cs
Tests the Film Locations API controller with 5 tests:
- GetAll endpoints (null query, empty query, search query)
- GetSuggestions endpoints (valid/short prefix)

### FilmLocationServiceTests.cs  
Tests the FilmLocationService with 33 tests:
- GetAllAsync: Load all film locations
- SearchAsync: Case-insensitive search across 8 fields
  - Title, Director, Actors (3), Location, Neighborhood, Company
- GetSuggestionsAsync: Autocomplete with 10 tests
  - Prefix validation, case-insensitivity, ordering, limits

## Running Tests

### PowerShell Terminal
```powershell
cd C:\repos\sf-film-map
dotnet test SFFilmMap.Tests\SFFilmMap.Tests.csproj
```

### Visual Studio Test Explorer
1. Open Test Explorer (View → Test Explorer)
2. Click "Run All Tests" 
3. View results in the Test Explorer pane

### Command Line Options
```bash
# Run with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "FilmLocationsControllerTests"

# Run specific test
dotnet test --filter "GetAll_WithoutQuery"

# Run and collect coverage
dotnet test /p:CollectCoverage=true
```

## Test Structure

All tests follow **AAA (Arrange-Act-Assert)** pattern:

```csharp
[Fact]
public async Task TestName_Description()
{
	// Arrange - Setup test data
	var input = "data";

	// Act - Execute the code being tested
	var result = await _service.GetAllAsync();

	// Assert - Verify the results
	result.Should().NotBeNull();
}
```

## NuGet Dependencies

The test project includes:
- **xunit** (2.6.6) - Test framework
- **xunit.runner.visualstudio** (2.5.4) - VS integration
- **Microsoft.NET.Test.Sdk** (17.9.0) - Test scaffolding
- **Moq** (4.20.70) - Mocking library
- **FluentAssertions** (6.12.0) - Assertion library
- **SFFilmMap** - Project reference

## Troubleshooting

**Tests not found?**
- Ensure you're in the correct directory: `C:\repos\sf-film-map`
- Check that SFFilmMap.Tests.csproj exists
- Run `dotnet test --list-tests`

**Build fails?**
- Delete bin/obj folders: `dotnet clean`
- Restore packages: `dotnet restore`
- Rebuild: `dotnet build`

**CSV file not found?**
- Tests require: `backend/Data/Film_Locations_in_San_Francisco.csv`
- This file is copied during build

## Next Steps

Consider adding:
- Integration tests with TestFixture
- Performance/benchmark tests  
- API endpoint tests with WebApplicationFactory
- Data repository pattern for better testability
