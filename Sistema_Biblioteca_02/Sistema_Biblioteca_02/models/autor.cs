using System.ComponentModel.DataAnnotations;

namespace Sistema_Biblioteca_02.Models
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é Obrigatório")]
        [StringLength(100, ErrorMessage = "Máximo 100 Caracteres")]

        public string Nome { get; set; } = string.Empty;

        [StringLength(50)]
        public string Nacionalidade { get; set; } = string.Empty;

        public List<Livro> Livros { get; set; } = new();

        public override string ToString() => Nome;
    }
}