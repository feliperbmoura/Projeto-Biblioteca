using Microsoft.EntityFrameworkCore;
using Sistema_Biblioteca_02.Models;
using System.Configuration;

namespace Sistema_Biblioteca_02.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Livro> Livros { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            );
        }
    }
}
