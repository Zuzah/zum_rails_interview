using Xunit;
using PokemonTournamentApi.Models;
using PokemonTournamentApi.Services;

namespace PokemonTournamentAPI.Tests
{
    public class TournamentSimulatorTests
    {
        [Fact]
        public void WaterBeatsFire()
        {
            // water beats fire
            var waterPokemon = new PokemonData 
            { 
                Id = 1, 
                Name = "Squirtle", 
                Type = "water", 
                BaseExperience = 50 
            };
            var firePokemon = new PokemonData 
            { 
                Id = 2, 
                Name = "Charmander", 
                Type = "fire", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(waterPokemon, firePokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void FireBeatsGrass()
        {
            // fire beats grass. When grass is the first parameter and fire the second,
            // then fire's advantage should yield a result of P2Wins.
            var grassPokemon = new PokemonData 
            { 
                Id = 3, 
                Name = "Bulbasaur", 
                Type = "grass", 
                BaseExperience = 50 
            };
            var firePokemon = new PokemonData 
            { 
                Id = 4, 
                Name = "Charmander", 
                Type = "fire", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, firePokemon);

            Assert.Equal(BattleResult.P2Wins, result);
        }

        [Fact]
        public void GrassBeatsElectric()
        {
            // grass beats electric
            var grassPokemon = new PokemonData 
            { 
                Id = 5, 
                Name = "Bulbasaur", 
                Type = "grass", 
                BaseExperience = 50 
            };
            var electricPokemon = new PokemonData 
            { 
                Id = 6, 
                Name = "Pikachu", 
                Type = "electric", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, electricPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void ElectricBeatsWater()
        {
            // electric beats water
            var electricPokemon = new PokemonData 
            { 
                Id = 7, 
                Name = "Pikachu", 
                Type = "electric", 
                BaseExperience = 50 
            };
            var waterPokemon = new PokemonData 
            { 
                Id = 8, 
                Name = "Squirtle", 
                Type = "water", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(electricPokemon, waterPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void GhostBeatsPsychic()
        {
            // ghost beats psychic
            var ghostPokemon = new PokemonData 
            { 
                Id = 9, 
                Name = "Gastly", 
                Type = "ghost", 
                BaseExperience = 50 
            };
            var psychicPokemon = new PokemonData 
            { 
                Id = 10, 
                Name = "Abra", 
                Type = "psychic", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(ghostPokemon, psychicPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void PsychicBeatsFighting()
        {
            // psychic beats fighting
            var psychicPokemon = new PokemonData 
            { 
                Id = 11, 
                Name = "Abra", 
                Type = "psychic", 
                BaseExperience = 50 
            };
            var fightingPokemon = new PokemonData 
            { 
                Id = 12, 
                Name = "Machop", 
                Type = "fighting", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(psychicPokemon, fightingPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void FightingBeatsDark()
        {
            // fighting beats dark
            var fightingPokemon = new PokemonData 
            { 
                Id = 13, 
                Name = "Machop", 
                Type = "fighting", 
                BaseExperience = 50 
            };
            var darkPokemon = new PokemonData 
            { 
                Id = 14, 
                Name = "Umbreon", 
                Type = "dark", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(fightingPokemon, darkPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void DarkBeatsGhost()
        {
            // dark beats ghost
            var darkPokemon = new PokemonData 
            { 
                Id = 15, 
                Name = "Umbreon", 
                Type = "dark", 
                BaseExperience = 50 
            };
            var ghostPokemon = new PokemonData 
            { 
                Id = 16, 
                Name = "Gastly", 
                Type = "ghost", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(darkPokemon, ghostPokemon);

            Assert.Equal(BattleResult.P1Wins, result);
        }

        [Fact]
        public void FallbackBasedOnBaseExperience_WinsHigherExperience()
        {
            // When types do not provide a decisive advantage (e.g. grass vs water),
            // the Pokemon with higher base_experience should win.
            var grassPokemon = new PokemonData 
            { 
                Id = 17, 
                Name = "Bulbasaur", 
                Type = "grass", 
                BaseExperience = 40 
            };
            var waterPokemon = new PokemonData 
            { 
                Id = 18, 
                Name = "Squirtle", 
                Type = "water", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, waterPokemon);

            // grass (first) vs water (second) yields a tie based solely on type,
            // so the one with the higher base_experience (water) should win, resulting in P2Wins.
            Assert.Equal(BattleResult.P2Wins, result);
        }

        [Fact]
        public void FallbackTieWhenEqualExperience()
        {
            // When neither type provides an advantage and base_experience values are equal,
            // the result should be a tie.
            var grassPokemon = new PokemonData 
            { 
                Id = 19, 
                Name = "Bulbasaur", 
                Type = "grass", 
                BaseExperience = 50 
            };
            var waterPokemon = new PokemonData 
            { 
                Id = 20, 
                Name = "Squirtle", 
                Type = "water", 
                BaseExperience = 50 
            };

            var result = TournamentSimulator.DetermineBattleOutcome(grassPokemon, waterPokemon);

            Assert.Equal(BattleResult.Tie, result);
        }
    }
}
