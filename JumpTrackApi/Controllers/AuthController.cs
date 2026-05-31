using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using JumpTrackApi.Models;

namespace JumpTrackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            await conn.OpenAsync();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Email, Nome, SocietaId, IsAdmin, IsActive FROM utente WHERE Email = @Email AND PasswordHash = @Password";
            cmd.Parameters.AddWithValue("@Email", login.Email);
            cmd.Parameters.AddWithValue("@Password", login.Password); // Attenzione: password in chiaro solo per test!

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return Ok(new {
                    Id = reader.GetInt32(0),
                    Email = reader.GetString(1),
                    Nome = reader.GetString(2),
                    SocietaId = reader.GetInt32(3),
                    IsAdmin = reader.GetBoolean(4),
                    IsActive = reader.GetBoolean(5)
                });
            }
            return Unauthorized();
        }
    }

}

