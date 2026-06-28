namespace SFFilmMap.Models;

public class FilmLocation
{
    public string Title { get; set; } = string.Empty;
    public string? ReleaseYear { get; set; }
    public string? Locations { get; set; }
    public string? FunFacts { get; set; }
    public string? ProductionCompany { get; set; }
    public string? Distributor { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actor1 { get; set; }
    public string? Actor2 { get; set; }
    public string? Actor3 { get; set; }
    public string? AnalysisNeighborhood { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}

public class FilmLocationDto
{
    public string Title { get; set; } = string.Empty;
    public string? ReleaseYear { get; set; }
    public string? Location { get; set; }
    public string? FunFacts { get; set; }
    public string? ProductionCompany { get; set; }
    public string? Director { get; set; }
    public string? Writer { get; set; }
    public string? Actor1 { get; set; }
    public string? Actor2 { get; set; }
    public string? Actor3 { get; set; }
    public string? Neighborhood { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
