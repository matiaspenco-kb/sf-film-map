using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFFilmMap.Controllers;
using SFFilmMap.Models;
using SFFilmMap.Services;
using Xunit;
using FluentAssertions;

namespace SFFilmMap.Tests.Controllers;

public class FilmLocationsControllerTests
{
    private readonly FilmLocationsController _controller;
    private readonly FilmLocationService _service;

    public FilmLocationsControllerTests()
    {
        var mockLogger = new Mock<ILogger<FilmLocationService>>();
        _service = new FilmLocationService(mockLogger.Object);
        _controller = new FilmLocationsController(_service);
    }

    [Fact]
    public async Task GetAll_WithoutQuery_ReturnsActionResult()
    {
        var result = await _controller.GetAll(null);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_WithEmptyQuery_ReturnsActionResult()
    {
        var result = await _controller.GetAll("   ");
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_WithSearchQuery_ReturnsActionResult()
    {
        var result = await _controller.GetAll("film");
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSuggestions_WithValidPrefix_ReturnsActionResult()
    {
        var result = await _controller.GetSuggestions("The");
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetSuggestions_WithShortPrefix_ReturnsActionResult()
    {
        var result = await _controller.GetSuggestions("V");
        result.Should().NotBeNull();
    }
}
