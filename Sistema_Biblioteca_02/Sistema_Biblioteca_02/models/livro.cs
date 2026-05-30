using System.ComponentModel.DataAnnotations;

namespace Sistema_Biblioteca_02.Models
{
    public class Livro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(50)]
        public string Genero { get; set; } = string.Empty;

        [Range(1000, 2100, ErrorMessage = "Ano Inválido")]
        public int AnoPublicacao { get; set; }

        [StringLength(500)]
        public string Sinopse { get; set; } = string.Empty;
        public int AutorId { get; set; } 
        public Autor? Autor { get; set; } // (?) esse sinal significa que pode ser Nulo ou não />
    }
}