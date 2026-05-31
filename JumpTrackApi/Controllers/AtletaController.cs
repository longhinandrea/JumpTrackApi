using JumpTrackApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JumpTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtletaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AtletaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> InserisciAtleta([FromBody] Atleta atleta)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO atleti (Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine, SocietaId)
                                VALUES (@Cognome, @Nome, @Eta, @SocietaSportiva, @Telefono, @Immagine, @SocietaId)";
            cmd.Parameters.AddWithValue("@Cognome", atleta.Cognome ?? "");
            cmd.Parameters.AddWithValue("@Nome", atleta.Nome ?? "");
            cmd.Parameters.AddWithValue("@Eta", atleta.Eta);
            cmd.Parameters.AddWithValue("@SocietaSportiva", atleta.SocietaSportiva ?? "");
            cmd.Parameters.AddWithValue("@Telefono", atleta.Telefono ?? "");
            cmd.Parameters.AddWithValue("@Immagine", atleta.Immagine ?? "");
            cmd.Parameters.AddWithValue("@SocietaId", atleta.SocietaId);

            await cmd.ExecuteNonQueryAsync();
            return Ok(new { Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAtleti([FromQuery] int? societaId = null)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            if (societaId.HasValue)
            {
                cmd.CommandText = "SELECT Id, Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine, SocietaId FROM atleti WHERE SocietaId = @SocietaId";
                cmd.Parameters.AddWithValue("@SocietaId", societaId.Value);
            }
            else
            {
                cmd.CommandText = "SELECT Id, Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine, SocietaId FROM atleti";
            }
            var reader = await cmd.ExecuteReaderAsync();

            var lista = new List<Atleta>();
            while (await reader.ReadAsync())
            {
                lista.Add(new Atleta
                {
                    Id = reader.GetInt32(0),
                    Cognome = reader.GetString(1),
                    Nome = reader.GetString(2),
                    Eta = reader.GetInt32(3),
                    SocietaSportiva = reader.GetString(4),
                    Telefono = reader.GetString(5),
                    Immagine = reader.GetString(6),
                    SocietaId = reader.GetInt32(7)
                });
            }
            return Ok(lista);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAtletaById(int id, [FromQuery] int? societaId = null)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            if (societaId.HasValue)
            {
                cmd.CommandText = "SELECT Id, Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine, SocietaId FROM atleti WHERE Id = @Id AND SocietaId = @SocietaId";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@SocietaId", societaId.Value);
            }
            else
            {
                cmd.CommandText = "SELECT Id, Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine, SocietaId FROM atleti WHERE Id = @Id";
                cmd.Parameters.AddWithValue("@Id", id);
            }

            var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var atleta = new Atleta
                {
                    Id = reader.GetInt32(0),
                    Cognome = reader.GetString(1),
                    Nome = reader.GetString(2),
                    Eta = reader.GetInt32(3),
                    SocietaSportiva = reader.GetString(4),
                    Telefono = reader.GetString(5),
                    Immagine = reader.GetString(6),
                    SocietaId = reader.GetInt32(7)
                };
                return Ok(atleta);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificaAtleta(int id, [FromBody] Atleta atleta)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE atleti SET
                            Cognome = @Cognome,
                            Nome = @Nome,
                            Eta = @Eta,
                            SocietaSportiva = @SocietaSportiva,
                            Telefono = @Telefono,
                            Immagine = @Immagine,
                            SocietaId = @SocietaId
                        WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Cognome", atleta.Cognome ?? "");
            cmd.Parameters.AddWithValue("@Nome", atleta.Nome ?? "");
            cmd.Parameters.AddWithValue("@Eta", atleta.Eta);
            cmd.Parameters.AddWithValue("@SocietaSportiva", atleta.SocietaSportiva ?? "");
            cmd.Parameters.AddWithValue("@Telefono", atleta.Telefono ?? "");
            cmd.Parameters.AddWithValue("@Immagine", atleta.Immagine ?? "");
            cmd.Parameters.AddWithValue("@SocietaId", atleta.SocietaId);
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows > 0)
                return Ok(new { Success = true });
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminaAtleta(int id)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM atleti WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            var rows = await cmd.ExecuteNonQueryAsync();
            if (rows > 0)
                return Ok(new { Success = true });
            return NotFound();
        }



    }
}

