using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SitioWebDePeliculas.Data;
using SitioWebDePeliculas.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

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
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre");
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre");
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create( Pelicula pelicula)

        {
            if (ModelState.IsValid)
            {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.DirectorId);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.GeneroId);
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            //Recuperar la película original para obtener la ruta de la imagen actual
            var peliculaExistente = await _context.Peliculas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if(peliculaExistente == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (pelicula.ImagenArchivo != null && pelicula.ImagenArchivo.Length > 0)
                    {

                        //Eliminar la imagen anterior si existe
                        if(!string.IsNullOrEmpty(peliculaExistente.ImagenRuta))
                        {
                            var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", peliculaExistente.ImagenRuta.TrimStart('/'));
                            if (System.IO.File.Exists(rutaAnterior))
                            {
                                System.IO.File.Delete(rutaAnterior);
                            }
                        }

                        //Guardar la nueva imagen
                        var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(pelicula.ImagenArchivo.FileName);
                        var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await pelicula.ImagenArchivo.CopyToAsync(stream);
                        }
                        pelicula.ImagenRuta = "/imagenes/" + nombreArchivo;
                     
                    }
                    else
                    {
                        pelicula.ImagenRuta = peliculaExistente.ImagenRuta;
                    }
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directores, "Id", "Nombre", pelicula.Director);
            ViewData["GeneroId"] = new SelectList(_context.Generos, "Id", "Nombre", pelicula.Genero);
            return View(pelicula);
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
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                if(!string.IsNullOrEmpty(pelicula.ImagenRuta))
                {
                    var rutaCompleta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pelicula.ImagenRuta.TrimStart('/'));
                    if(System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
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
