using System;
using System.Collections.Generic;
using PokemonTournamentApi.Models;

namespace PokemonTournamentApi.Services
{
    public enum BattleResult
    {
        P1Wins,
        P2Wins,
        Tie
    }

    public static class TournamentSimulator
    {
        // Define type advantages: key beats value.
        private static readonly Dictionary<string, string> TypeAdvantages = new(StringComparer.OrdinalIgnoreCase)
        {
            { "grass", "water" },
            { "water", "fire" },
            { "fire", "grass" }

            // TODO: ADD more as needed, check Bulbapedia for details
        };

        // Returns true if the attacking Pokemon has a type advantage over the other.
        private static bool CanBeat(string attackerType, string defenderType)
        {
            return TypeAdvantages.TryGetValue(attackerType, out var targetType) &&
                   string.Equals(targetType, defenderType, StringComparison.OrdinalIgnoreCase);
        }

        /// Determines the outcome of a battle between two Pokémon based on their types.
        public static BattleResult DetermineBattleOutcome(PokemonData p1, PokemonData p2)
        {
            bool p1CanBeat = CanBeat(p1.Type, p2.Type);
            bool p2CanBeat = CanBeat(p2.Type, p1.Type);

            if (p1CanBeat && !p2CanBeat)
            {
                return BattleResult.P1Wins;
            }
            else if (p2CanBeat && !p1CanBeat)
            {
                return BattleResult.P2Wins;
            }
            else
            {
                return BattleResult.Tie;
            }
        }

        /// Simulates a round-robin tournament: each Pokémon battles every other exactly ONCE.
        public static void SimulateBattles(List<PokemonData> pokemons)
        {
            // Reset all scores.
            foreach (var pokemon in pokemons)
            {
                pokemon.Wins = 0;
                pokemon.Losses = 0;
                pokemon.Ties = 0;
            }

            // Loop through every unique pair and simulate a battle.
            for (int i = 0; i < pokemons.Count; i++)
            {
                for (int j = i + 1; j < pokemons.Count; j++)
                {
                    BattleResult result = DetermineBattleOutcome(pokemons[i], pokemons[j]);

                    // Output the matches
                    Console.WriteLine($"Match Result: {pokemons[i].Name} ({pokemons[i].Type}) vs {pokemons[j].Name} ({pokemons[j].Type}): {result}");

                    switch (result)
                    {
                        case BattleResult.P1Wins:
                            pokemons[i].Wins++;
                            pokemons[j].Losses++;
                            break;
                        case BattleResult.P2Wins:
                            pokemons[j].Wins++;
                            pokemons[i].Losses++;
                            break;
                        case BattleResult.Tie:
                            pokemons[i].Ties++;
                            pokemons[j].Ties++;
                            break;
                    }
                }
            }
        }
    }
}
