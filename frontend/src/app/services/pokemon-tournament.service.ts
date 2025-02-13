import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PokemonData } from '../models/pokemon_data.model';

@Injectable({
  providedIn: 'root'
})
export class PokemonTournamentService {
  private apiUrl = 'http://localhost:5065/pokemon/tournament/statistics';

  constructor(private http: HttpClient) {}

  getPokemonList(sortBy: string, sortDirection: string = 'desc'): Observable<PokemonData[]> {
    return this.http.get<PokemonData[]>(`${this.apiUrl}?sortBy=${sortBy}&sortDirection=${sortDirection}`);
  }
}
