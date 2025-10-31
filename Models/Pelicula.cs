using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SitioWebDePeliculas.Models.Validations;

namespace SitioWebDePeliculas.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "La Sinopsis es obligatoria.")]
        [StringLength(800, ErrorMessage = "El nombre no puede tener más de 800 caracteres.")]
        public string Sinopsis { get; set; }
        [Required(ErrorMessage = "La Duración(minutos) es obligatoria.")]
        [Range(1, 500,ErrorMessage = "El tiempo de duración debe estar entre 1 y 500 mminutos.")]
        [Display(Name = "Duración(min)")]
        public int Duracion { get; set; }
        [Required(ErrorMessage = "La Fecha de Estreno es obligatoria.")]
        [Display(Name = "Fecha de Estreno")]
        [NoFuture(ErrorMessage = "La Fecha de Estreno no puede ser mayor a la fecha actual.")]
        [DataType(DataType.Date)]

        public DateTime FechaEstreno { get; set; }
        [ValidateNever]

        [Required(ErrorMessage = "El Género es obligatorio.")]
        [Display(Name = "Género")]
        public int GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        
        public Genero? Genero { get; set; }
        [Required(ErrorMessage = "El Director es obligatorio.")]
        [Display(Name = "Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director? Director { get; set; }

        [Display(Name = "Póster")]
        public string? ImagenRuta { get; set; }
        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }
        [ValidateNever]
        public ICollection<PeliculaActor> PeliculaActores { get; set; } = new List<PeliculaActor>();

    }
}
