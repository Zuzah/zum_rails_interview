using Microsoft.AspNetCore.Mvc;
using PokemonTournamentApi.Models;
using PokemonTournamentApi.Services;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public async Task<IActionResult> GetTournamentStatistics(
            [FromQuery] string sortBy,
            [FromQuery] string sortDirection = "desc")
        {
            // Validate query parameters as per Zum Rails attachment
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                return BadRequest("sortBy parameter is required");
            }

            var allowedSortBy = new[] { "wins", "losses", "ties", "name", "id" };

            if (!allowedSortBy.Contains(sortBy.ToLower()))
            {
                return BadRequest("sortBy parameter is invalid");
            }

            var allowedSortDirections = new[] { "asc", "desc" };

            if (!allowedSortDirections.Contains(sortDirection.ToLower()))
            {
                return BadRequest("sortDirection parameter is invalid");
            }

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

            // Sort the tournament list based on query parameters.
            if (sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase))
            {
                switch (sortBy.ToLower())
                {
                    case "wins":
                        tournamentList = tournamentList.OrderBy(p => p.Wins).ToList();
                        break;
                    case "losses":
                        tournamentList = tournamentList.OrderBy(p => p.Losses).ToList();
                        break;
                    case "ties":
                        tournamentList = tournamentList.OrderBy(p => p.Ties).ToList();
                        break;
                    case "name":
                        tournamentList = tournamentList.OrderBy(p => p.Name).ToList();
                        break;
                    case "id":
                        tournamentList = tournamentList.OrderBy(p => p.Id).ToList();
                        break;
                }
            }
            // else descending
            else
            {
                switch (sortBy.ToLower())
                {
                    case "wins":
                        tournamentList = tournamentList.OrderByDescending(p => p.Wins).ToList();
                        break;
                    case "losses":
                        tournamentList = tournamentList.OrderByDescending(p => p.Losses).ToList();
                        break;
                    case "ties":
                        tournamentList = tournamentList.OrderByDescending(p => p.Ties).ToList();
                        break;
                    case "name":
                        tournamentList = tournamentList.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "id":
                        tournamentList = tournamentList.OrderByDescending(p => p.Id).ToList();
                        break;
                }
            }

            return Ok(tournamentList);
        }
    }
}
