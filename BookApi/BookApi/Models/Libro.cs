using System;
using System.Collections.Generic;

namespace BookApi.Models
{
    public partial class Libro
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public int? AnhoPublicacion { get; set; }
        public int? GeneroId { get; set; }
        public int? AutorId { get; set; }
        public int? Paginas { get; set; }
        public string? PortadaUrl { get; set; }
        public double? Puntuacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string? Sinopsis { get; set; }

        public virtual Autor? Autor { get; set; }
        public virtual Genero? Genero { get; set; }
    }
}
