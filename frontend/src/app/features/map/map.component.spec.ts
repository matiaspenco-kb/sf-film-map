import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { MapComponent } from './map.component';
import { FilmLocationApiService } from '../../services/film-location-api.service';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';

class MockFilmLocationApiService {
  getLocations = jasmine.createSpy('getLocations').and.returnValue(of([{ title: 'Vertigo', latitude: 37.8, longitude: -122.4 }]));
  getSuggestions = jasmine.createSpy('getSuggestions').and.returnValue(of(['Vertigo']));
}

describe('MapComponent', () => {
  let component: MapComponent;
  let fixture: ComponentFixture<MapComponent>;
  let apiService: MockFilmLocationApiService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MapComponent, NoopAnimationsModule],
      providers: [{ provide: FilmLocationApiService, useClass: MockFilmLocationApiService }],
    }).compileComponents();

    fixture = TestBed.createComponent(MapComponent);
    component = fixture.componentInstance;
    apiService = TestBed.inject(FilmLocationApiService) as unknown as MockFilmLocationApiService;
    fixture.detectChanges();
  });

  it('should load all locations on init and expose the result count', () => {
    expect(apiService.getLocations).toHaveBeenCalled();
    expect(component.resultCount()).toBe(1);
    expect(component.isLoading()).toBeFalse();
  });

  it('should update the search query and trigger a search request', () => {
    apiService.getLocations.calls.reset();

    component.onSearchChange('Hitchcock');
    expect(component.searchQuery()).toBe('Hitchcock');

    component.onSearch();
    expect(apiService.getLocations).toHaveBeenCalledWith('Hitchcock');
  });

  it('should clear the search state and reset the selection', () => {
    component.selectedLocation.set({ title: 'Vertigo', latitude: 37.8, longitude: -122.4 });
    component.clearSearch();

    expect(component.searchQuery()).toBe('');
    expect(component.selectedLocation()).toBeNull();
  });
});
