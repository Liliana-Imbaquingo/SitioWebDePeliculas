using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SitioWebDePeliculas.Models
{
    public class Actor
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Biografia { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [ValidateNever]
        public ICollection<PeliculaActor> PeliculaActores { get; set; }
    }
}
