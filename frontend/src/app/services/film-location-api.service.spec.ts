import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { FilmLocationApiService } from './film-location-api.service';

describe('FilmLocationApiService', () => {
  let service: FilmLocationApiService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    service = TestBed.inject(FilmLocationApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch locations without query params by default', () => {
    service.getLocations().subscribe((locations) => {
      expect(locations.length).toBe(1);
      expect(locations[0].title).toBe('Vertigo');
    });

    const req = httpMock.expectOne('http://localhost:5000/api/filmlocations');
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('q')).toBeNull();
    req.flush([{ title: 'Vertigo', latitude: 37.8, longitude: -122.4 }]);
  });

  it('should include the query when searching locations', () => {
    service.getLocations('Hitchcock').subscribe();

    const req = httpMock.expectOne((request) => request.url === 'http://localhost:5000/api/filmlocations');
    expect(req.request.params.get('q')).toBe('Hitchcock');
    req.flush([]);
  });

  it('should request suggestions with the provided prefix', () => {
    service.getSuggestions('Alc').subscribe((suggestions) => {
      expect(suggestions).toEqual(['Alcatraz']);
    });

    const req = httpMock.expectOne('http://localhost:5000/api/filmlocations/suggestions?prefix=Alc');
    expect(req.request.method).toBe('GET');
    req.flush(['Alcatraz']);
  });
});
