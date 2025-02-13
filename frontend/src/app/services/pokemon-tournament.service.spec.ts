import { TestBed } from '@angular/core/testing';

import { PokemonTournamentService } from './pokemon-tournament.service';

describe('PokemonTournamentService', () => {
  let service: PokemonTournamentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PokemonTournamentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
