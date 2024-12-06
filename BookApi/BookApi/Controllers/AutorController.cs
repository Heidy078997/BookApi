using BookApi.Dtos;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AutorController: ControllerBase
    {
        private readonly AppDbContext _context;

        public AutorController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Autores
        [HttpPost]
        public async Task<IActionResult> PostAutor([FromBody] AutorCreateDto autorDto)
        {
            if (autorDto == null || string.IsNullOrWhiteSpace(autorDto.Nombre) || string.IsNullOrWhiteSpace(autorDto.Apellido))
            {
                return BadRequest("El nombre y apellido del autor son obligatorios.");
            }

            try
            {
                // Crear una nueva instancia del modelo Autor
                var autor = new Autor
                {
                    Nombre = autorDto.Nombre,
                    Apellido = autorDto.Apellido,
                    FechaNacimiento = autorDto.FechaNacimiento
                };
                
                _context.Autors.Add(autor); // Agregar el autor a la base de datos
                await _context.SaveChangesAsync(); // Guardar los cambios

                // Retornar el autor creado con su ID
                return CreatedAtAction(nameof(GetAutor), new { id = autor.AutorId }, autor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        // Método GET para obtener un autor (usado por CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }
    }
}
