using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SitioWebDePeliculas.Models
{
    public class Genero
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(500, ErrorMessage = "El Nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La Descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La Descripción no puede tener más de 200 caracteres.")]
        public string Descripcion { get; set; }

        //Relacion 1-N
        [ValidateNever]
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
