using Microsoft.AspNetCore.Mvc;
using PokemonTournamentApi.Models;


///pokemon/tournament
namespace PokemonTournamentApi.Controllers {

    [ApiController]
    [Route("pokemon/tournament")]
    public class PokemonTournamentController : ControllerBase {

        // Define the HTTP GET (takes parms from mode)
        // Route is pokemon/tournament/name
        [HttpGet("statistics")]
        public IActionResult GetTournamentStatistics() {

            // Return a hard-coded list
            var tournamentList = new List<PokemonData> {
                // return new models
                new PokemonData { Id = 1, Losses = 2, Wins = 10 , Ties = 0, Name = "Charmander", Type = "Fire" },
                new PokemonData { Id = 2, Losses = 12, Wins = 1 , Ties = 2, Name = "Bulbasaur", Type = "Grass" }
            };

            return Ok(tournamentList);
        }
    }


}