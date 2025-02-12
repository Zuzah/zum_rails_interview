using Xunit;
using PokemonTournamentApi.Models;
using PokemonTournamentApi.Services;

namespace PokemonTournamentAPI.Tests
{
    public class TournamentSimulatorTests
    {
        [Fact]
        public void GrassBeatsWater()
        {
            // Arrange: Create two Pokémon with defined types.
            var grassPokemon = new PokemonData { Id = 1, Name = "Bulbasaur", Type = "grass" };
            var waterPokemon = new PokemonData { Id = 2, Name = "Squirtle", Type = "water" };

            // Act: Determine the battle outcome.
            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, waterPokemon);

            // Assert: Grass should beat water.
            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void WaterBeatsFire()
        {
            var waterPokemon = new PokemonData { Id = 3, Name = "Oshawot", Type = "water" };
            var firePokemon = new PokemonData { Id = 4, Name = "Tepig", Type = "fire" };

            var result = TournamentSimulator.DetermineBattleOutcome(waterPokemon, firePokemon);
            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void FireBeatsGrass()
        {
            var grassPokemon = new PokemonData { Id = 5, Name = "Turtwig", Type = "grass" };
            var firePokemon = new PokemonData { Id = 6, Name = "Chimchar", Type = "fire" };

            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, firePokemon);
            Assert.Equal(BattleResult.P2Wins, result);
        }

        [Fact]
        public void UndefinedTypeResultsInTie()
        {
            var electricPokemon = new PokemonData { Id = 7, Name = "Jolteon", Type = "electric" };
            var waterPokemon = new PokemonData { Id = 8, Name = "Vaporeon", Type = "water" };

            var result = TournamentSimulator.DetermineBattleOutcome(electricPokemon, waterPokemon);
            Assert.Equal(BattleResult.Tie, result);
        }
    }
}
