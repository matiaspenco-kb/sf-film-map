using CsvHelper.Configuration;
using SFFilmMap.Models;

namespace SFFilmMap;

public sealed class FilmLocationMap : ClassMap<FilmLocation>
{
    public FilmLocationMap()
    {
        Map(m => m.Title).Name("title");
        Map(m => m.ReleaseYear).Name("release_year");
        Map(m => m.Locations).Name("locations");
        Map(m => m.FunFacts).Name("fun_facts");
        Map(m => m.ProductionCompany).Name("production_company");
        Map(m => m.Distributor).Name("distributor");
        Map(m => m.Director).Name("director");
        Map(m => m.Writer).Name("writer");
        Map(m => m.Actor1).Name("actor_1");
        Map(m => m.Actor2).Name("actor_2");
        Map(m => m.Actor3).Name("actor_3");
        Map(m => m.AnalysisNeighborhood).Name("analysis_neighborhood");
        Map(m => m.Latitude).Name("latitude");
        Map(m => m.Longitude).Name("longitude");
    }
}
