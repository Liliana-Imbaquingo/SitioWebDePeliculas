
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SitioWebDePeliculas.Models
{
    public class Genero
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        //Relacion 1-N
        [ValidateNever]
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
