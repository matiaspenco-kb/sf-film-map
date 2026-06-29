# 🎬 SF Film Map

An interactive map showing where movies have been filmed in San Francisco, with autocomplete search.

## Architecture

```
sf-film-map/
├── backend/          # ASP.NET Core 8 Web API (C#)
│   ├── Controllers/  # FilmLocationsController
│   ├── Models/       # FilmLocation, FilmLocationDto
│   ├── Services/     # FilmLocationService (CSV parsing + search)
│   ├── Mappings/     # CsvHelper ClassMap
│   └── Data/         # Film_Locations_in_San_Francisco.csv ← place here
│
└── frontend/         # Angular 17 (standalone components)
    └── src/app/
        ├── features/map/   # MapComponent (Leaflet + Material)
        ├── services/       # FilmLocationApiService (HTTP client)
        └── models/         # FilmLocation interface
```

---

## 🚀 Getting Started

### 1. Place your CSV

Copy your CSV file to:
```
backend/Data/Film_Locations_in_San_Francisco.csv
```

The expected columns are:
```
Title, Release Year, Locations, Fun Facts, Production Company, Distributor, Director, Writer, Actor 1, Actor 2, Actor 3
```

> **Note:** If no CSV is found, the app falls back to built-in sample data of 17 classic SF films.

The public dataset is available at:
https://data.sfgov.org/Culture-and-Recreation/Film-Locations-in-San-Francisco/yitu-d5am

---

### 2. Run the Backend (C# / ASP.NET Core 8)

```bash
cd backend
dotnet restore
dotnet run
# API available at http://localhost:5000
```

**Endpoints:**
| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/filmlocations` | All locations (or filtered with `?q=query`) |
| GET | `/api/filmlocations/suggestions?prefix=xxx` | Autocomplete title suggestions |
| GET | `/swagger` | API docs (development only) |

---

### 3. Run the Frontend (Angular 17)

```bash
cd frontend
npm install
npm start
# App at http://localhost:4200
```

---

## 🗺️ Features

- **Interactive map** using Leaflet + CartoDB Voyager light tiles
- **Custom film-strip markers** (pin with 🎬)
- **Autocomplete search** with debounced API calls — search by:
  - Movie title
  - Director
  - Actor name
  - Location name
  - Production company
- **Detail panel** shown when a marker is clicked — shows title, year, cast, fun facts
- **Bounds fitting** — map zooms to fit all visible results

---

## 🌐 Geocoding Strategy

Locations are matched against a built-in dictionary of 50+ well-known SF landmarks.
For production use, replace `ResolveCoordinates()` in `FilmLocationService.cs` with a
call to a geocoding API (Google Maps, Nominatim, Mapbox, etc.).

---

## 🔧 Configuration

Backend port: set via `ASPNETCORE_URLS` or `launchSettings.json`.  
Frontend API URL: update `baseUrl` in `src/app/services/film-location-api.service.ts`.

---

## Stack

| Layer | Technology |
|-------|-----------|
| Backend | C# / ASP.NET Core 8 / CsvHelper |
| Frontend | Angular 17 (standalone) / Angular Material / Leaflet |
| Map tiles | CartoDB Voyager light (free, no API key needed) |
| Geocoding | Built-in SF landmark dictionary (upgradeable) |
