namespace PokemonTournamentApi.Models
{
    public class PokemonData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int BaseExperience { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Ties { get; set; }

        // Constructor
        public PokemonData()
        {
            Type = string.Empty;
            Name = string.Empty;
            BaseExperience = 0;
            Wins = 0;
            Losses = 0;
            Ties = 0;
        }
    }
}
