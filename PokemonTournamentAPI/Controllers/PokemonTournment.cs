using Microsoft.AspNetCore.Mvc;
using PokemonTournamentApi.Models;
using PokemonTournamentApi.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PokemonTournamentApi.Controllers
{
    [ApiController]
    [Route("pokemon/tournament")]
    public class PokemonTournamentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly Random _random = new Random();

        // Inject IHttpClientFactory instead of HttpClient (had issues with HttpClient)
        public PokemonTournamentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetTournamentStatistics()
        {
            // First randomly select 8 unique Pokémon IDs (from #1 to #1151 where #1 is Bulbasaur)
            var chosenPokemonIds = new HashSet<int>();

            // Keep adding a random pokemon until 8 are chosen
            while (chosenPokemonIds.Count < 8)
            {
                int id = _random.Next(1, 151);
                chosenPokemonIds.Add(id);
            }

            var tournamentList = new List<PokemonData>();

            // For each selected ID, fetch Pokémon data from the PokeAPI.
            foreach (var id in chosenPokemonIds)
            {
                try
                {
                    var json = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{id}");

                    // Use JsonDocument to parse the JSON response.
                    using var document = JsonDocument.Parse(json);
                    var root = document.RootElement;

                    // Extract id, name, and base_experience:
                    int pokeId = root.GetProperty("id").GetInt32();
                    string pokeName = root.GetProperty("name").GetString() ?? "";
                    int baseExperience = root.GetProperty("base_experience").GetInt32();

                    // Extract type from the first element of the "types" array (using slot 1).
                    string type = "";
                    if (root.TryGetProperty("types", out JsonElement typesElement) &&
                        typesElement.ValueKind == JsonValueKind.Array && typesElement.GetArrayLength() > 0)
                    {
                        // Get the first type element.
                        var firstTypeObj = typesElement[0];
                        if (firstTypeObj.TryGetProperty("type", out JsonElement typeObj))
                        {
                            type = typeObj.GetProperty("name").GetString() ?? "";
                        }
                    }

                    tournamentList.Add(new PokemonData
                    {
                        Id = pokeId,
                        Name = pokeName,
                        Type = type,
                        BaseExperience = baseExperience,
                        Wins = 0,
                        Losses = 0,
                        Ties = 0
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error fetching Pokemon IDs {id}: {e.Message}");
                }
            }

            if (tournamentList.Count < 8)
            {
                return StatusCode(500, "Error fetching Pokemon data from endpoint.");
            }

            // Simulate the tournament battles based on type advantages.
            TournamentSimulator.SimulateBattles(tournamentList);

            return Ok(tournamentList);
        }
    }
}
