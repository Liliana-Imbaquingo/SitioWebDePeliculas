using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SitioWebDePeliculas.Models
{
    public class Genero
    {

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        //Relacion 1-N
        [ValidateNever]
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
