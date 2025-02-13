import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PokemonTournamentService } from '../../services/pokemon-tournament.service';
import { PokemonData } from '../../models/pokemon_data.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-pokemon-stats',
  standalone: true,
  imports: [CommonModule, FormsModule], // Add FormsModule
  templateUrl: './pokemon-stats.component.html'
})
export class PokemonStatsComponent {
  pokemons: PokemonData[] = [];
  sortBy: string = 'wins';
  sortDirection: string = 'desc';
  private unsubscribe$ = new Subject<void>();

  constructor(private pokemonTournamentService: PokemonTournamentService) {}

  // Subscribe to service, use takeUntil to ensure proper un-subscribe
  fetchPokemon() {
    this.pokemonTournamentService.getPokemonList(this.sortBy, this.sortDirection)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(data => this.pokemons = data);
  }

  ngOnDestroy() {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
  
}
