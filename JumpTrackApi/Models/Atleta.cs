namespace JumpTrackApi.Models
{
    public class Atleta
    {
        public int Id { get; set; }
        public string? Cognome { get; set; }
        public string? Nome { get; set; }
        public int Eta { get; set; }
        public string? SocietaSportiva { get; set; }
        public string? Immagine { get; set; }
        public string? Telefono { get; set; }
        public int SocietaId { get; set; } // FK verso Societa
    }
}