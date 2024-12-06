using BookApi.Dtos;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GeneroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeneroController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Generos
        [HttpPost]
        public async Task<IActionResult> PostGenero([FromBody] GeneroCreateDto generoDto)
        {
            if (generoDto == null || string.IsNullOrWhiteSpace(generoDto.Descripcion))
            {
                return BadRequest("La descripción del género es requerida.");
            }

            try
            {
                // Crear una nueva instancia del modelo Genero
                var genero = new Genero
                {
                    Descripcion = generoDto.Descripcion
                };

                _context.Generos.Add(genero); // Agregar el nuevo género
                await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

                // Retornar el género creado con su ID
                return CreatedAtAction(nameof(GetGenero), new { id = genero.GeneroId }, genero);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        // GET: api/Generos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genero>> GetGenero(int id)
        {
            var genero = await _context.Generos.FindAsync(id);

            if (genero == null)
            {
                return NotFound("Género no encontrado.");
            }

            return genero;
        }

    }
}
