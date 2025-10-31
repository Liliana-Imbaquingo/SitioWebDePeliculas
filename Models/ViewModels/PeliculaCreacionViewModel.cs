using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SitioWebDePeliculas.Models.ViewModels
{
    public class PeliculaCreacionViewModel
    {
        public Pelicula Pelicula { get; set; }

        public List<int> ActoresSeleccionadosIds { get; set; } = new();
        [ValidateNever]
        public IEnumerable<SelectListItem> ActoresDisponibles { get; set; }

    }
}
