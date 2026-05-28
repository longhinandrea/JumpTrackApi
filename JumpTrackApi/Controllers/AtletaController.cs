using JumpTrackApi.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JumpTrackApi.Controllers
{
    public class AtletaController : Controller
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
            cmd.CommandText = @"INSERT INTO atleti (Cognome, Nome, Eta, SocietaSportiva, Telefono, Immagine)
                                VALUES (@Cognome, @Nome, @Eta, @SocietaSportiva, @Telefono, @Immagine)";
            cmd.Parameters.AddWithValue("@Cognome", atleta.Cognome ?? "");
            cmd.Parameters.AddWithValue("@Nome", atleta.Nome ?? "");
            cmd.Parameters.AddWithValue("@Eta", atleta.Eta);
            cmd.Parameters.AddWithValue("@SocietaSportiva", atleta.SocietaSportiva ?? "");
            cmd.Parameters.AddWithValue("@Telefono", atleta.Telefono ?? "");
            cmd.Parameters.AddWithValue("@Immagine", atleta.Immagine ?? "");

            await cmd.ExecuteNonQueryAsync();
            return Ok(new { Success = true });
        }

    }
}
