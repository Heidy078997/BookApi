using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;
using System.Threading.Tasks;
using BookApi.Dtos;


namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class LoginController : ControllerBase
    {

        private readonly AppDbContext _context;

        public LoginController (AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/Login
        [HttpPost("login")]

        
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Correo) || string.IsNullOrWhiteSpace(loginRequest.Pass))
            {
                return BadRequest("Correo y contraseña son requeridos.");
            }

            // Buscar el usuario en la base de datos
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == loginRequest.Correo && u.Pass == loginRequest.Pass);

            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas.");
            }

            // Crear la sesión en el servidor (Ejemplo: Cookie o In-Memory)
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNombre", usuario.Username);

            return Ok(new { mensaje = "Inicio de sesión exitoso", usuario = usuario.Username });
        }

        // GET: api/Auth/Logout
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();
            return Ok(new { mensaje = "Sesión cerrada exitosamente" });
        }

        //obtener datos de usuario logueado

        // GET: api/Login/user
        [HttpGet("user")]
        public async Task<IActionResult> GetLoggedUser()
        {
            try
            {
                // Verificar si existe el UsuarioId en la sesión
                var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
                if (string.IsNullOrEmpty(usuarioIdStr) || !int.TryParse(usuarioIdStr, out int usuarioId))
                {
                    return Unauthorized("No hay un usuario logueado.");
                }

                // Buscar al usuario en la base de datos
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);
                if (usuario == null)
                {
                    return NotFound("El usuario no fue encontrado.");
                }

                // Devolver los datos del usuario
                return Ok(new
                {
                    usuario.Username,
                    usuario.Nombre,
                    usuario.Apellido,
                    usuario.Correo
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los datos del usuario: {ex.Message}");
            }
        }

    }
}    

