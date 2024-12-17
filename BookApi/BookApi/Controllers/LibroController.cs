using BookApi.Dtos;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class LibroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LibroController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<IActionResult> PostLibro([FromBody] LibroCreateDto libroDto)
        {
            if (libroDto == null || string.IsNullOrWhiteSpace(libroDto.Titulo) || string.IsNullOrWhiteSpace(libroDto.Isbn))
            {
                return BadRequest("El título y el ISBN del libro son obligatorios.");
            }

            try
            {
                // Verificar si el AutorId y GeneroId existen (si se proporcionaron)
                if (libroDto.AutorId.HasValue && !_context.Autors.Any(a => a.AutorId == libroDto.AutorId.Value))
                {
                    return NotFound("El autor especificado no existe.");
                }

                if (libroDto.GeneroId.HasValue && !_context.Generos.Any(g => g.GeneroId == libroDto.GeneroId.Value))
                {
                    return NotFound("El género especificado no existe.");
                }

                // Crear una instancia del modelo Libro
                var libro = new Libro
                {
                    Titulo = libroDto.Titulo,
                    Isbn = libroDto.Isbn,
                    AnhoPublicacion = libroDto.AnhoPublicacion,
                    GeneroId = libroDto.GeneroId,
                    AutorId = libroDto.AutorId,
                    Paginas = libroDto.Paginas,
                    PortadaUrl = libroDto.PortadaUrl,
                    Puntuacion = libroDto.Puntuacion,
                    FechaRegistro = DateTime.UtcNow, // Establece la fecha actual
                    Sinopsis = libroDto.Sinopsis
                };

                _context.Libros.Add(libro); // Agregar el libro a la base de datos
                await _context.SaveChangesAsync(); // Guardar cambios

                // Retornar el libro creado con su ID
                return CreatedAtAction(nameof(GetLibro), new { id = libro.LibroId }, libro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        // Método GET para obtener un libro (usado por CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }

        //se añade este codigo

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLibro(int id, [FromBody] LibroCreateDto libroDto)
        {
            if (libroDto == null || string.IsNullOrWhiteSpace(libroDto.Titulo) || string.IsNullOrWhiteSpace(libroDto.Isbn))
            {
                return BadRequest("El título y el ISBN del libro son obligatorios.");
            }

            // Buscar el libro por ID
            var libroExistente = await _context.Libros.FindAsync(id);

            if (libroExistente == null)
            {
                return NotFound("El libro no existe.");
            }

            try
            {
                // Actualizar los campos del libro existente con los nuevos valores
                libroExistente.Titulo = libroDto.Titulo;
                libroExistente.Isbn = libroDto.Isbn;
                libroExistente.AnhoPublicacion = libroDto.AnhoPublicacion;
                libroExistente.GeneroId = libroDto.GeneroId;
                libroExistente.AutorId = libroDto.AutorId;
                libroExistente.Paginas = libroDto.Paginas;
                libroExistente.PortadaUrl = libroDto.PortadaUrl;
                libroExistente.Puntuacion = libroDto.Puntuacion;
                libroExistente.Sinopsis = libroDto.Sinopsis;
                libroExistente.Autor = null;
                libroExistente.Genero = null;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok(libroExistente); // Retornar el libro actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        //codigo añadido eliminar

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
            {
                return NotFound("El libro no existe.");
            }

            try
            {
                // Eliminar el libro de la base de datos
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();

                return NoContent(); // Retorna un 204 No Content cuando se elimina correctamente
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }

        //codigo añadido get all books

        // GET: api/Libros
        [HttpGet]
        public async Task<IActionResult> GetAllLibros()
        {
            var libros = await _context.Libros.ToListAsync();

            if (libros == null || libros.Count == 0)
            {
                return NotFound("No se encontraron libros.");
            }

            return Ok(libros); // Retorna una lista de libros
        }



        //codigo añadido buscar por nombre

        //ver si funciona het

        [HttpGet("buscar")]
        public async Task<IActionResult> BuscarLibros([FromQuery] string titulo)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest("Debe proporcionar un título para la búsqueda.");
            }

            try
            {
                // Convertir a minúsculas para realizar la búsqueda insensible a mayúsculas/minúsculas
                var libros = await _context.Libros
                    .Where(l => l.Titulo.ToLower().Contains(titulo.ToLower()))
                    .ToListAsync();

                if (libros == null || !libros.Any())
                {
                    return NotFound("No se encontraron libros que coincidan con el término de búsqueda.");
                }

                return Ok(libros); // Devolver los libros encontrados
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al buscar libros: {ex.Message}");
            }
        }




    }
}
