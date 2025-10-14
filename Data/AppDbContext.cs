using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SitioWebDePeliculas.Models;

namespace SitioWebDePeliculas.Data
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero>    Generos { get; set; }
        public DbSet<Director> Directores { get; set; }
        public DbSet<Actor> Actores { get; set; }

        public DbSet<PeliculaActor> PeliculaActores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   modelBuilder.Entity<PeliculaActor>().HasKey(x => new {x.PeliculaId, x.ActorId});


            modelBuilder.Entity<PeliculaActor>()
           .HasOne(pa => pa.Pelicula)
           .WithMany(p => p.PeliculaActores)
           .HasForeignKey(pa => pa.PeliculaId);

            modelBuilder.Entity<PeliculaActor>()
            .HasOne(pa => pa.Actor)
            .WithMany(a => a.PeliculaActores)
            .HasForeignKey(pa => pa.ActorId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
