using JumpTrackApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace JumpTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CavalloController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public CavalloController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult GetCavalli([FromQuery] int? societaId = null)
        {
            var cavalli = new List<Cavallo>();
            using var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            MySqlCommand cmd;
            if (societaId.HasValue)
            {
                cmd = new MySqlCommand("SELECT * FROM Cavalli WHERE SocietaId = @SocietaId", conn);
                cmd.Parameters.AddWithValue("@SocietaId", societaId.Value);
            }
            else
            {
                cmd = new MySqlCommand("SELECT * FROM Cavalli", conn);
            }
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cavalli.Add(new Cavallo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                    Eta = reader.GetInt32(reader.GetOrdinal("Eta")),
                    Scuderia = reader.GetString(reader.GetOrdinal("Scuderia")),
                    Immagine = reader.IsDBNull(reader.GetOrdinal("Immagine")) ? null : reader.GetString(reader.GetOrdinal("Immagine")),
                    SocietaId = reader.GetInt32(reader.GetOrdinal("SocietaId"))
                });
            }
            return Ok(cavalli);
        }

        [HttpPost]
        public IActionResult InserisciCavallo([FromBody] Cavallo cavallo)
        {
            using var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            var cmd = new MySqlCommand("INSERT INTO Cavalli (Nome, Eta, Scuderia, Immagine) VALUES (@Nome, @Eta, @Scuderia, @Immagine)", conn);
            cmd.Parameters.AddWithValue("@Nome", cavallo.Nome);
            cmd.Parameters.AddWithValue("@Eta", cavallo.Eta);
            cmd.Parameters.AddWithValue("@Scuderia", cavallo.Scuderia);
            cmd.Parameters.AddWithValue("@Immagine", cavallo.Immagine ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
            return Ok(new { success = true });
        }

        [HttpPut("{id}")]
        public IActionResult ModificaCavallo(int id, [FromBody] Cavallo cavallo)
        {
            using var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            var cmd = new MySqlCommand("UPDATE Cavalli SET Nome=@Nome, Eta=@Eta, Scuderia=@Scuderia, Immagine=@Immagine WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Nome", cavallo.Nome);
            cmd.Parameters.AddWithValue("@Eta", cavallo.Eta);
            cmd.Parameters.AddWithValue("@Scuderia", cavallo.Scuderia);
            cmd.Parameters.AddWithValue("@Immagine", cavallo.Immagine ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) return NotFound();
            return Ok(new { success = true });
        }

        [HttpDelete("{id}")]
        public IActionResult EliminaCavallo(int id)
        {
            using var conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();
            var cmd = new MySqlCommand("DELETE FROM Cavalli WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            var rows = cmd.ExecuteNonQuery();
            if (rows == 0) return NotFound();
            return Ok(new { success = true });
        }

    }
}