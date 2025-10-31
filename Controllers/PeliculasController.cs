using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SitioWebDePeliculas.Data;
using SitioWebDePeliculas.Models;
using SitioWebDePeliculas.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SitioWebDePeliculas.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly AppDbContext _context;


        public PeliculasController(AppDbContext context)
        {
            _context = context;

        }

        // GET: Peliculas
        public async Task<IActionResult> Index(string searchString, int? generoId)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentGenero"] = generoId;
            ViewData["Generos"] = new SelectList(_context.Generos, "Id", "Nombre");

            var peliculas = _context.Peliculas.Include(p => p.Director).Include(p => p.Genero).Include(p => p.PeliculaActores).ThenInclude(pa => pa.Actor).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                peliculas = peliculas.Where(p => p.Titulo.Contains(searchString));
            }
            if (generoId.HasValue && generoId.Value > 0)
            {
                peliculas = peliculas.Where(p => p.GeneroId == generoId.Value);
            }

            return View(await peliculas.ToListAsync());
        }

        // GET: Peliculas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.Director)
                .Include(p => p.PeliculaActores)
                 .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            var viewModel = new PeliculaCreacionViewModel
            {
                Pelicula = new Pelicula(),
                ActoresDisponibles = _context.Actores
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre
                })
                .ToList()
            };

            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre");
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");

            return View(viewModel);
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PeliculaCreacionViewModel model)

        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"Campo: {error.Key} - Error: {err.ErrorMessage}");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                var pelicula = model.Pelicula;
                if (pelicula.ImagenArchivo != null && pelicula.ImagenArchivo.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(pelicula.ImagenArchivo.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await pelicula.ImagenArchivo.CopyToAsync(stream);
                    }

                    pelicula.ImagenRuta = "/imagenes/" + nombreArchivo;
                }
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                // Asociar actores seleccionados
                if (model.ActoresSeleccionadosIds.Any())
                {
                    foreach (var actorId in model.ActoresSeleccionadosIds)
                    {
                        _context.PeliculaActores.Add(new PeliculaActor
                        {
                            PeliculaId = pelicula.Id,
                            ActorId = actorId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            //Si hay error, volver a llenar combos
            model.ActoresDisponibles = _context.Actores
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Nombre
            })
            .ToList();

            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", model.Pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", model.Pelicula.GeneroId);

            return View(model);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
            .Include(p => p.PeliculaActores)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            var viewModel = new PeliculaCreacionViewModel
            {
                Pelicula = pelicula,
                ActoresSeleccionadosIds = pelicula.PeliculaActores.Select(pa => pa.ActorId).ToList(),
                ActoresDisponibles = _context.Actores
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre
                })
                .ToList()
            };

            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(viewModel);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PeliculaCreacionViewModel model)
        {
            if (id != model.Pelicula.Id)
                return NotFound();

            // 🔹 Recuperar película con su relación
            var peliculaExistente = await _context.Peliculas
                .Include(p => p.PeliculaActores)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (peliculaExistente == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.ActoresDisponibles = _context.Actores
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre
                    }).ToList();

                ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", model.Pelicula.DirectorId);
                ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", model.Pelicula.GeneroId);

                return View(model);
            }

            try
            {
                // 🔹 Actualizar campos básicos
                peliculaExistente.Titulo = model.Pelicula.Titulo;
                peliculaExistente.Sinopsis = model.Pelicula.Sinopsis;
                peliculaExistente.Duracion = model.Pelicula.Duracion;
                peliculaExistente.FechaEstreno = model.Pelicula.FechaEstreno;
                peliculaExistente.DirectorId = model.Pelicula.DirectorId;
                peliculaExistente.GeneroId = model.Pelicula.GeneroId;

                // 🔹 Manejo de imagen
                if (model.Pelicula.ImagenArchivo != null && model.Pelicula.ImagenArchivo.Length > 0)
                {
                    // Eliminar la imagen anterior si existe
                    if (!string.IsNullOrEmpty(peliculaExistente.ImagenRuta))
                    {
                        var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", peliculaExistente.ImagenRuta.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                            System.IO.File.Delete(rutaAnterior);
                    }

                    // Guardar la nueva imagen
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(model.Pelicula.ImagenArchivo.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await model.Pelicula.ImagenArchivo.CopyToAsync(stream);
                    }
                    peliculaExistente.ImagenRuta = "/imagenes/" + nombreArchivo;
                }

                // 🔹 Actualizar relación muchos a muchos (siempre)
                peliculaExistente.PeliculaActores.Clear();
                if (model.ActoresSeleccionadosIds != null)
                {
                    foreach (var actorId in model.ActoresSeleccionadosIds)
                    {
                        peliculaExistente.PeliculaActores.Add(new PeliculaActor
                        {
                            PeliculaId = peliculaExistente.Id,
                            ActorId = actorId
                        });
                    }
                }

                _context.Update(peliculaExistente);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeliculaExists(model.Pelicula.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .Include(p => p.Director)
                .Include(p => p.Genero)
                .Include(p => p.PeliculaActores)
                    .ThenInclude(pa => pa.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas
            .Include(p => p.PeliculaActores)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula != null)
            {
                if (!string.IsNullOrEmpty(pelicula.ImagenRuta))
                {
                    var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pelicula.ImagenRuta.TrimStart('/'));
                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }
                //Eliminar relaciones peliculas-actores
                if (pelicula.PeliculaActores != null && pelicula.PeliculaActores.Any())
                {
                    _context.PeliculaActores.RemoveRange(pelicula.PeliculaActores);
                }
                _context.Peliculas.Remove(pelicula);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }

    }
}

