using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using JumpTrackApi.Models;
using System.Collections.Generic;

namespace JumpTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtenteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UtenteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var utenti = new List<Utente>();
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Utente", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                utenti.Add(new Utente
                {
                    Id = reader.GetInt32("Id"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Nome = reader.GetString("Nome"),
                    SocietaId = reader.GetInt32("SocietaId"),
                    IsAdmin = reader.GetBoolean("IsAdmin"),
                    IsActive = reader.GetBoolean("IsActive")
                });
            }
            return Ok(utenti);
        }

        // GET: api/utente/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Utente WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var utente = new Utente
                {
                    Id = reader.GetInt32("Id"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Nome = reader.GetString("Nome"),
                    SocietaId = reader.GetInt32("SocietaId"),
                    IsAdmin = reader.GetBoolean("IsAdmin"),
                    IsActive = reader.GetBoolean("IsActive")
                };
                return Ok(utente);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Utente WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var utente = new Utente
                {
                    Id = reader.GetInt32("Id"),
                    Email = reader.GetString("Email"),
                    PasswordHash = reader.GetString("PasswordHash"),
                    Nome = reader.GetString("Nome"),
                    SocietaId = reader.GetInt32("SocietaId"),
                    IsAdmin = reader.GetBoolean("IsAdmin"),
                    IsActive = reader.GetBoolean("IsActive")
                };
                return Ok(utente);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Utente utente)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(
                "INSERT INTO Utente (Email, PasswordHash, Nome, SocietaId, IsAdmin, IsActive) VALUES (@Email, @PasswordHash, @Nome, @SocietaId, @IsAdmin, @IsActive)", conn);
            cmd.Parameters.AddWithValue("@Email", utente.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", utente.PasswordHash);
            cmd.Parameters.AddWithValue("@Nome", utente.Nome);
            cmd.Parameters.AddWithValue("@SocietaId", utente.SocietaId);
            cmd.Parameters.AddWithValue("@IsAdmin", utente.IsAdmin);
            cmd.Parameters.AddWithValue("@IsActive", utente.IsActive);
            cmd.ExecuteNonQuery();
            return Ok(new { success = true });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Utente utente)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(
                "UPDATE Utente SET Email=@Email, PasswordHash=@PasswordHash, Nome=@Nome, SocietaId=@SocietaId, IsAdmin=@IsAdmin, IsActive=@IsActive WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Email", utente.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", utente.PasswordHash);
            cmd.Parameters.AddWithValue("@Nome", utente.Nome);
            cmd.Parameters.AddWithValue("@SocietaId", utente.SocietaId);
            cmd.Parameters.AddWithValue("@IsAdmin", utente.IsAdmin);
            cmd.Parameters.AddWithValue("@IsActive", utente.IsActive);
            cmd.Parameters.AddWithValue("@Id", id);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                return Ok(new { success = true });
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand("DELETE FROM Utente WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                return Ok(new { success = true });
            return NotFound();
        }
    }
}
