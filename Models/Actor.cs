using System.ComponentModel.DataAnnotations;

namespace SitioWebDePeliculas.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Biografia { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        //[ValidateNever]
        public ICollection<PeliculaActor> PeliculaActores { get; set; }
    }
}
