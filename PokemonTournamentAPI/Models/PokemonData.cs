namespace PokemonTournamentApi.Models {
    
    public class PokemonData {
        
        //Define as unique identifier
        public int Id {get; set;}
        public string Name {get; set; }
        public string Type {get; set; }
        public int Wins {get; set; }

        public int Losses {get; set; }

        public int Ties {get; set; }

        // Constructor
        public PokemonData() {

            // init as needed
            Type = string.Empty;
            Name = string.Empty;
            Wins = 0;
            Losses = 0; 
        }

    }
}