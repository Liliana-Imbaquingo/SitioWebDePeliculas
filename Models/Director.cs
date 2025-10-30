using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SitioWebDePeliculas.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace SitioWebDePeliculas.Models
{
    public class Director
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El Nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La Nacionalidad es obligatoria.")]
        [StringLength(50, ErrorMessage = "La Nacionalidad no puede tener más de 50 caracteres.")]
        public string Nacionalidad { get; set; }
        [Required(ErrorMessage = "La Fecha de Nacimiento es obligatoria.")]
        [Display(Name = "Fecha de Nacimiento")]
        [NoFuture(ErrorMessage = "La Fecha de Nacimiento no puede ser mayor a la fecha actual.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        //Relacion 1-N
        [ValidateNever] 
        public ICollection<Pelicula> Peliculas{ get; set; }
    }
}
