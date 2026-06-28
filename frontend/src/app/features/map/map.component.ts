import {
  Component, OnInit, AfterViewInit, OnDestroy,
  ViewChild, ElementRef, inject, signal, computed
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, debounceTime, distinctUntilChanged, switchMap, takeUntil, of } from 'rxjs';
import * as L from 'leaflet';
import { FilmLocationApiService } from '../../services/film-location-api.service';
import { FilmLocation } from '../../models/film-location.model';

@Component({
  selector: 'app-map',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatAutocompleteModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatChipsModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('mapContainer') mapContainer!: ElementRef<HTMLDivElement>;

  private readonly api = inject(FilmLocationApiService);
  private readonly destroy$ = new Subject<void>();
  private readonly searchInput$ = new Subject<string>();

  private map!: L.Map;
  private markerGroup!: L.LayerGroup;
  private allLocations: FilmLocation[] = [];

  searchQuery = signal('');
  suggestions = signal<string[]>([]);
  isLoading = signal(true);
  selectedLocation = signal<FilmLocation | null>(null);
  resultCount = signal(0);

  ngOnInit(): void {
    // Load all locations on startup
    this.api.getLocations().pipe(takeUntil(this.destroy$)).subscribe({
      next: (locs) => {
        this.allLocations = locs;
        this.resultCount.set(locs.length);
        this.isLoading.set(false);
        this.renderMarkers(locs);
      },
      error: () => this.isLoading.set(false)
    });

    // Autocomplete suggestions
    this.searchInput$.pipe(
      debounceTime(250),
      distinctUntilChanged(),
      switchMap(prefix => prefix.length >= 2 ? this.api.getSuggestions(prefix) : of([])),
      takeUntil(this.destroy$)
    ).subscribe(s => this.suggestions.set(s));
  }

  ngAfterViewInit(): void {
    this.initMap();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.map?.remove();
  }

  private initMap(): void {
    this.map = L.map(this.mapContainer.nativeElement, {
      center: [37.7749, -122.4194],
      zoom: 13,
      zoomControl: true,
    });

    L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', {
      attribution: '© OpenStreetMap © CARTO',
      subdomains: 'abcd',
      maxZoom: 19
    }).addTo(this.map);

    this.markerGroup = L.layerGroup().addTo(this.map);
  }

  private renderMarkers(locations: FilmLocation[]): void {
    if (!this.markerGroup) return;
    this.markerGroup.clearLayers();

    const icon = L.divIcon({
      className: '',
      html: `<div class="film-marker"><span class="film-reel">🎬</span></div>`,
      iconSize: [32, 32],
      iconAnchor: [16, 16],
      popupAnchor: [0, -16],
    });

    locations.forEach(loc => {
      const marker = L.marker([loc.latitude, loc.longitude], { icon })
        .bindPopup(this.buildPopup(loc), { maxWidth: 280 })
        .on('click', () => this.selectedLocation.set(loc));

      this.markerGroup.addLayer(marker);
    });

    if (locations.length === 1) {
      this.map.setView([locations[0].latitude, locations[0].longitude], 16);
    } else if (locations.length > 1) {
      const bounds = L.latLngBounds(locations.map(l => [l.latitude, l.longitude] as [number, number]));
      this.map.fitBounds(bounds, { padding: [50, 50] });
    }
  }

  private buildPopup(loc: FilmLocation): string {
    const actors = [loc.actor1, loc.actor2, loc.actor3].filter(Boolean).join(', ');
    return `
      <div class="popup-content">
        <h3>${loc.title}</h3>
        ${loc.releaseYear ? `<span class="year">${loc.releaseYear}</span>` : ''}
        ${loc.location ? `<p class="location-label"><strong>📍</strong> ${loc.location}</p>` : ''}
        ${loc.director ? `<p><strong>Director:</strong> ${loc.director}</p>` : ''}
        ${actors ? `<p><strong>Cast:</strong> ${actors}</p>` : ''}
        ${loc.funFacts ? `<p class="fun-facts">"${loc.funFacts}"</p>` : ''}
      </div>
    `;
  }

  onSearchChange(value: string): void {
    this.searchQuery.set(value);
    this.searchInput$.next(value);
  }

  onSearch(): void {
    const q = this.searchQuery();
    this.isLoading.set(true);
    this.api.getLocations(q || undefined).pipe(takeUntil(this.destroy$)).subscribe({
      next: (locs) => {
        this.resultCount.set(locs.length);
        this.renderMarkers(locs);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  onSuggestionSelected(title: string): void {
    this.searchQuery.set(title);
    this.suggestions.set([]);
    this.onSearch();
  }

  clearSearch(): void {
    this.searchQuery.set('');
    this.suggestions.set([]);
    this.resultCount.set(this.allLocations.length);
    this.renderMarkers(this.allLocations);
    this.selectedLocation.set(null);
  }

  closePanel(): void {
    this.selectedLocation.set(null);
  }
}
