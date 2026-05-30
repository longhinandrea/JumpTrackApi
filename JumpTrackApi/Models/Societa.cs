namespace JumpTrackApi.Models
{
    public class Societa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? PartitaIva { get; set; }
        public string? CodiceFiscale { get; set; }
        public string? Indirizzo { get; set; }
    }
}