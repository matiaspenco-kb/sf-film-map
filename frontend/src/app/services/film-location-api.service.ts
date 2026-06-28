import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FilmLocation } from '../models/film-location.model';

@Injectable({ providedIn: 'root' })
export class FilmLocationApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = 'http://localhost:5000/api/filmlocations';

  getLocations(query?: string): Observable<FilmLocation[]> {
    let params = new HttpParams();
    if (query) params = params.set('q', query);
    return this.http.get<FilmLocation[]>(this.baseUrl, { params });
  }

  getSuggestions(prefix: string): Observable<string[]> {
    const params = new HttpParams().set('prefix', prefix);
    return this.http.get<string[]>(`${this.baseUrl}/suggestions`, { params });
  }
}
