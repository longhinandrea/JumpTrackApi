namespace JumpTrackApi.Models
{
    public class Utente
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Nome { get; set; }
        public int SocietaId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}