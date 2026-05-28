using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace JumpTrackApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new MySqlConnection(connStr);
            conn.Open();

            using var cmd = new MySqlCommand("SELECT NOW();", conn);
            var result = cmd.ExecuteScalar();

            return Ok(new { ServerTime = result });
        }
    }
}
