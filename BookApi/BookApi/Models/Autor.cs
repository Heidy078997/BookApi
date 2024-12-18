using System;
using System.Collections.Generic;

namespace BookApi.Models
{
    public partial class Autor
    {
        public int AutorId { get; set; }
        public string Nombre { get; set; } = null!;
        public virtual ICollection<Libro> Libros { get; set; }
    }
}
