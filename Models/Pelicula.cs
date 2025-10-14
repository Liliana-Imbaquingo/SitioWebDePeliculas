using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SitioWebDePeliculas.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }

        [Display(Name = "Duración")]
        public int Duracion {  get; set; }

      
        [Display(Name = "Fecha de Estreno")]
        public DateTime FechaEstreno { get; set; }

        [ValidateNever]
        public string ImagenRuta { get; set; }

        [Display(Name = "Género")]
        public int GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        public Genero Genero { get; set; }


        [Display(Name = "Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director Director { get; set; }

        [ValidateNever]
        public ICollection<PeliculaActor> PeliculaActores { get; set; }

    }
}
