using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SFFilmMap.Models;

namespace SFFilmMap.Services;

public class FilmLocationService
{
    private readonly ILogger<FilmLocationService> _logger;
    private List<FilmLocationDto> _locations = new();
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public FilmLocationService(ILogger<FilmLocationService> logger)
    {
        _logger = logger;
    }

    public async Task EnsureInitializedAsync()
    {
        if (_initialized) return;
        await _initLock.WaitAsync();
        try
        {
            if (_initialized) return;
            _locations = await LoadLocationsAsync();
            _initialized = true;
        }
        finally
        {
            _initLock.Release();
        }
    }

    private async Task<List<FilmLocationDto>> LoadLocationsAsync()
    {
        var csvPath = Path.Combine(AppContext.BaseDirectory, "Data", "Film_Locations_in_San_Francisco.csv");

        if (!File.Exists(csvPath))
        {
            _logger.LogError("CSV not found at {Path}", csvPath);
            return new List<FilmLocationDto>();
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            MissingFieldFound = null,
            BadDataFound = null,
        };

        var results = new List<FilmLocationDto>();

        using var reader = new StreamReader(csvPath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<FilmLocationMap>();

        await foreach (var record in csv.GetRecordsAsync<FilmLocation>())
        {
            // Skip rows without coordinates or title
            if (string.IsNullOrWhiteSpace(record.Title)) continue;
            if (record.Latitude == null || record.Longitude == null) continue;

            results.Add(new FilmLocationDto
            {
                Title = record.Title,
                ReleaseYear = record.ReleaseYear,
                Location = record.Locations,
                FunFacts = record.FunFacts,
                ProductionCompany = record.ProductionCompany,
                Director = record.Director,
                Writer = record.Writer,
                Actor1 = record.Actor1,
                Actor2 = record.Actor2,
                Actor3 = record.Actor3,
                Neighborhood = record.AnalysisNeighborhood,
                Latitude = record.Latitude.Value,
                Longitude = record.Longitude.Value,
            });
        }

        _logger.LogInformation("Loaded {Count} film locations from CSV", results.Count);
        return results;
    }

    public async Task<List<FilmLocationDto>> GetAllAsync()
    {
        await EnsureInitializedAsync();
        return _locations;
    }

    public async Task<List<FilmLocationDto>> SearchAsync(string query)
    {
        await EnsureInitializedAsync();
        if (string.IsNullOrWhiteSpace(query)) return _locations;

        var q = query.Trim();
        return _locations
            .Where(l =>
                l.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                (l.Director?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Actor1?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Actor2?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Actor3?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Location?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.Neighborhood?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (l.ProductionCompany?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();
    }

    public async Task<List<string>> GetSuggestionsAsync(string prefix)
    {
        await EnsureInitializedAsync();
        if (string.IsNullOrWhiteSpace(prefix) || prefix.Length < 2)
            return new List<string>();

        return _locations
            .Select(l => l.Title)
            .Where(t => t.Contains(prefix, StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(t => t.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? 0 : 1)
            .ThenBy(t => t)
            .Take(10)
            .ToList();
    }
}
