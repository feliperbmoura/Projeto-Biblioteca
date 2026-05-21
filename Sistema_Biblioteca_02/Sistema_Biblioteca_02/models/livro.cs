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

        public int AutorId { get; set; } 
        public Autor? Autor { get; set; } // ? poder ser nulo ou autor não carregado no código
    }
}