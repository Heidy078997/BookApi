﻿using BookApi.Dtos;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (autorDto == null || string.IsNullOrWhiteSpace(autorDto.Nombre) )
            {
                return BadRequest("El nombre del autor es obligatorio.");
            }

            try
            {
                // Crear una nueva instancia del modelo Autor
                var autor = new Autor
                {
                    Nombre = autorDto.Nombre,
              
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

        //get all

        // GET: api/Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            try
            {
                var autores = await _context.Autors.ToListAsync();
                return Ok(autores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error: {ex.Message}");
            }
        }
    }
}
