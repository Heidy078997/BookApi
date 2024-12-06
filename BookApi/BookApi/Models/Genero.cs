using System;
using System.Collections.Generic;

namespace BookApi.Models
{
    public partial class Genero
    {
        public int GeneroId { get; set; }
        public string Descripcion { get; set; } = null!;

        public virtual ICollection<Libro> Libros { get; set; }
    }
}
