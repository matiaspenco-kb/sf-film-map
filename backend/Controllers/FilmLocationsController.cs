using Microsoft.AspNetCore.Mvc;
using SFFilmMap.Models;
using SFFilmMap.Services;

namespace SFFilmMap.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilmLocationsController : ControllerBase
{
    private readonly FilmLocationService _service;

    public FilmLocationsController(FilmLocationService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get all film locations, optionally filtered by search query.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FilmLocationDto>), 200)]
    public async Task<ActionResult<IEnumerable<FilmLocationDto>>> GetAll([FromQuery] string? q)
    {
        var results = string.IsNullOrWhiteSpace(q)
            ? await _service.GetAllAsync()
            : await _service.SearchAsync(q);

        return Ok(results);
    }

    /// <summary>
    /// Get autocomplete suggestions for movie titles.
    /// </summary>
    [HttpGet("suggestions")]
    [ProducesResponseType(typeof(IEnumerable<string>), 200)]
    public async Task<ActionResult<IEnumerable<string>>> GetSuggestions([FromQuery] string prefix)
    {
        var suggestions = await _service.GetSuggestionsAsync(prefix);
        return Ok(suggestions);
    }
}
