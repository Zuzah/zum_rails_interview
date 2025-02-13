import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PokemonStatsComponent } from './components/pokemon-stats/pokemon-stats.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, PokemonStatsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'frontend';
}
