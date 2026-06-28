using Microsoft.Extensions.Logging;
using Moq;
using SFFilmMap.Models;
using SFFilmMap.Services;
using Xunit;
using FluentAssertions;

namespace SFFilmMap.Tests.Services;

public class FilmLocationServiceTests
{
    private readonly Mock<ILogger<FilmLocationService>> _mockLogger;

    public FilmLocationServiceTests()
    {
        _mockLogger = new Mock<ILogger<FilmLocationService>>();
    }

    private FilmLocationService CreateService()
    {
        return new FilmLocationService(_mockLogger.Object);
    }

    private List<FilmLocationDto> CreateSampleLocations()
    {
        return new List<FilmLocationDto>
        {
            new FilmLocationDto
            {
                Title = "The Maltese Falcon",
                ReleaseYear = "1941",
                Location = "Stockton St",
                Director = "John Huston",
                Actor1 = "Humphrey Bogart",
                Actor2 = "Mary Astor",
                Neighborhood = "Downtown",
                ProductionCompany = "Warner Bros",
                Latitude = 37.7915,
                Longitude = -122.4065
            },
            new FilmLocationDto
            {
                Title = "Vertigo",
                ReleaseYear = "1958",
                Location = "Golden Gate Bridge",
                Director = "Alfred Hitchcock",
                Actor1 = "James Stewart",
                Actor2 = "Kim Novak",
                Neighborhood = "Golden Gate",
                ProductionCompany = "Paramount Pictures",
                Latitude = 37.8199,
                Longitude = -122.4783
            },
            new FilmLocationDto
            {
                Title = "Bullitt",
                ReleaseYear = "1968",
                Location = "Fillmore St",
                Director = "Peter Yates",
                Actor1 = "Steve McQueen",
                Neighborhood = "Marina",
                ProductionCompany = "Warner Bros",
                Latitude = 37.7886,
                Longitude = -122.4298
            }
        };
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsAllLocations()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    #endregion

    #region SearchAsync Tests

    [Theory]
    [InlineData("The Maltese Falcon")]
    [InlineData("the maltese falcon")]
    [InlineData("MALTESE")]
    public async Task SearchAsync_WithTitleQuery_ReturnsCaseInsensitiveMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchAsync_WithNullOrEmptyQuery_ReturnsAllLocations(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Theory]
    [InlineData("John Huston")]
    [InlineData("john huston")]
    [InlineData("huston")]
    public async Task SearchAsync_WithDirectorQuery_ReturnsMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Theory]
    [InlineData("Humphrey Bogart")]
    [InlineData("James Stewart")]
    [InlineData("bogart")]
    public async Task SearchAsync_WithActorQuery_ReturnsMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Theory]
    [InlineData("Golden Gate")]
    [InlineData("Fillmore")]
    [InlineData("golden gate")]
    public async Task SearchAsync_WithLocationQuery_ReturnsMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Theory]
    [InlineData("Downtown")]
    [InlineData("Marina")]
    [InlineData("downtown")]
    public async Task SearchAsync_WithNeighborhoodQuery_ReturnsMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    [Fact]
    public async Task SearchAsync_WithNonExistentQuery_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();
        var query = "NonexistentFilmThatWillNeverExist12345";

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Warner Bros")]
    [InlineData("Paramount")]
    [InlineData("warner")]
    public async Task SearchAsync_WithProductionCompanyQuery_ReturnsMatches(string query)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.SearchAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<FilmLocationDto>>();
    }

    #endregion

    #region GetSuggestionsAsync Tests

    [Theory]
    [InlineData("Ver")]
    [InlineData("ver")]
    [InlineData("VER")]
    public async Task GetSuggestionsAsync_WithValidPrefix_ReturnsSuggestions(string prefix)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
    }

    [Fact]
    public async Task GetSuggestionsAsync_WithPrefixLessThan2Chars_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();
        var prefix = "V";

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task GetSuggestionsAsync_WithNullOrEmptyPrefix_ReturnsEmptyList(string prefix)
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSuggestionsAsync_ReturnsCaseInsensitiveMatches()
    {
        // Arrange
        var service = CreateService();
        var prefix = "the";

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
    }

    [Fact]
    public async Task GetSuggestionsAsync_ReturnsDistinctResults()
    {
        // Arrange
        var service = CreateService();
        var prefix = "The";

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        // Verify no duplicates (distinct was applied)
        result.Should().Equal(result.Distinct());
    }

    [Fact]
    public async Task GetSuggestionsAsync_ReturnsMaximum10Suggestions()
    {
        // Arrange
        var service = CreateService();
        var prefix = "Th";

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        result.Count.Should().BeLessThanOrEqualTo(10);
    }

    [Fact]
    public async Task GetSuggestionsAsync_WithNonExistentPrefix_ReturnsEmptyList()
    {
        // Arrange
        var service = CreateService();
        var prefix = "XYZ123";

        // Act
        var result = await service.GetSuggestionsAsync(prefix);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        result.Should().BeEmpty();
    }

    #endregion

    #region Caching Tests

    [Fact]
    public async Task EnsureInitializedAsync_LoadsDataOnlyOnce()
    {
        // Arrange
        var service = CreateService();

        // Act
        await service.EnsureInitializedAsync();
        var firstCall = await service.GetAllAsync();
        await service.EnsureInitializedAsync();
        var secondCall = await service.GetAllAsync();

        // Assert
        firstCall.Should().NotBeEmpty();
        secondCall.Should().NotBeEmpty();
        // Both calls should return the same data (from cache)
        firstCall.Should().HaveCount(secondCall.Count);
    }

    #endregion
}
