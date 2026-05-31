using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using JumpTrackApi.Models;
using System.Collections.Generic;

namespace JumpTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocietaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SocietaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int? societaId = null)
        {
            var societaList = new List<Societa>();
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            MySqlCommand cmd;
            if (societaId.HasValue)
            {
                cmd = new MySqlCommand("SELECT * FROM Societa WHERE Id = @SocietaId", conn);
                cmd.Parameters.AddWithValue("@SocietaId", societaId.Value);
            }
            else
            {
                cmd = new MySqlCommand("SELECT * FROM Societa", conn);
            }
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                societaList.Add(new Societa
                {
                    Id = reader.GetInt32("Id"),
                    Nome = reader.GetString("Nome"),
                    PartitaIva = reader["PartitaIva"] as string,
                    CodiceFiscale = reader["CodiceFiscale"] as string,
                    Indirizzo = reader["Indirizzo"] as string
                });
            }
            return Ok(societaList);
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM Societa WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var societa = new Societa
                {
                    Id = reader.GetInt32("Id"),
                    Nome = reader.GetString("Nome"),
                    PartitaIva = reader["PartitaIva"] as string,
                    CodiceFiscale = reader["CodiceFiscale"] as string,
                    Indirizzo = reader["Indirizzo"] as string
                };
                return Ok(societa);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Societa societa)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(
                "INSERT INTO Societa (Nome, PartitaIva, CodiceFiscale, Indirizzo) VALUES (@Nome, @PartitaIva, @CodiceFiscale, @Indirizzo)", conn);
            cmd.Parameters.AddWithValue("@Nome", societa.Nome);
            cmd.Parameters.AddWithValue("@PartitaIva", societa.PartitaIva ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodiceFiscale", societa.CodiceFiscale ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Indirizzo", societa.Indirizzo ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
            return Ok(new { success = true });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Societa societa)
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            using var cmd = new MySqlCommand(
                "UPDATE Societa SET Nome=@Nome, PartitaIva=@PartitaIva, CodiceFiscale=@CodiceFiscale, Indirizzo=@Indirizzo WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Nome", societa.Nome);
            cmd.Parameters.AddWithValue("@PartitaIva", societa.PartitaIva ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CodiceFiscale", societa.CodiceFiscale ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Indirizzo", societa.Indirizzo ?? (object)DBNull.Value);
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
            using var cmd = new MySqlCommand("DELETE FROM Societa WHERE Id=@Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
                return Ok(new { success = true });
            return NotFound();
        }
    }
}
