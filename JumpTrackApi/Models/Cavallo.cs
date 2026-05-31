namespace JumpTrackApi.Models
{
    public class Cavallo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Eta { get; set; }
        public string Scuderia { get; set; }
        public string Immagine { get; set; }
        public int SocietaId { get; set; } // FK verso Societa
    }
}