using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SitioWebDePeliculas.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace SitioWebDePeliculas.Models
{
    public class Actor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El Nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Biografía es obligatoria.")]
        [StringLength(800, ErrorMessage = "La Biografía no puede superar los 800 caracteres.")]
        [Display(Name = "Biografía")]
        public string Biografia { get; set; }
        [Required(ErrorMessage = "La Fecha de Nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        [NoFuture(ErrorMessage = "La Fecha de Nacimiento no puede ser mayor a la fecha actual.")]
        public DateTime FechaNacimiento { get; set; }

        [ValidateNever]
        public ICollection<PeliculaActor> PeliculaActores { get; set; }
    }
}
