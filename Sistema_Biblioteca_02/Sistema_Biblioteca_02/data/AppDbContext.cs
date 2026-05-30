using Microsoft.EntityFrameworkCore;
using Sistema_Biblioteca_02.Models;

namespace Sistema_Biblioteca_02.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Livro> Livros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "biblioteca.db");
            options.UseSqlite($"Data Source={caminho}");
        }
    }
}
