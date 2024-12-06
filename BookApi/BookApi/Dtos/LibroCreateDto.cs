namespace BookApi.Dtos
{
    public class LibroCreateDto
    {
        public string Titulo { get; set; } = null!;
        public string Isbn { get; set; } = null!;
        public int? AnhoPublicacion { get; set; }
        public int? GeneroId { get; set; }
        public int? AutorId { get; set; }
        public int? Paginas { get; set; }
        public string? PortadaUrl { get; set; }
        public double? Puntuacion { get; set; }
        public string? Sinopsis { get; set; }
    }
}
