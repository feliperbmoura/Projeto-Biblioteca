using Microsoft.EntityFrameworkCore;
using Sistema_Biblioteca_02.Data;
using Sistema_Biblioteca_02.Models;

namespace Sistema_Biblioteca_02.Forms
{
    public partial class FormAutores : Form
    {
        private readonly AppDbContext _context = new();

        private Label lblNome = new() { Text = "Nome:", Top = 25, Left = 10 };
        private Label lblNacionalidade = new() { Text = "Nacionalidade:", Top = 65, Left = 10 };

        private TextBox txtNome = new() { PlaceholderText = "Nome do autor", Width = 300, Top = 20, Left = 110 };
        private TextBox txtNacionalidade = new() { PlaceholderText = "Ex: Brasileiro", Width = 300, Top = 60, Left = 110 };

        private Button btnSalvar = new() { Text = "💾 Salvar", Top = 100, Left = 10, Width = 100, Height = 35 };
        private Button btnExcluir = new() { Text = "🗑️ Excluir", Top = 100, Left = 120, Width = 100, Height = 35 };
        private Button btnLimpar = new() { Text = "🔄 Limpar", Top = 100, Left = 230, Width = 100, Height = 35 };

        private DataGridView grid = new()
        {
            Top = 150,
            Left = 10,
            Width = 560,
            Height = 300,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        private int _idSelecionado = 0;

        public FormAutores()
        {
            InitializeComponent();
            Text = "Cadastro de Autores";
            Width = 600;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.AddRange(new Control[]
            {
                lblNome, lblNacionalidade,
                txtNome, txtNacionalidade,
                btnSalvar, btnExcluir, btnLimpar,
                grid
            });

            btnSalvar.Click += BtnSalvar_Click;
            btnExcluir.Click += BtnExcluir_Click;
            btnLimpar.Click += (s, e) => Limpar();
            grid.CellClick += Grid_CellClick;

            CarregarGrid();
        }

        private void CarregarGrid()
        {
            grid.DataSource = _context.Autores
                .Include(a => a.Livros)
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Nacionalidade,
                    TotalLivros = a.Livros.Count
                }).ToList();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome é Obrigatório!", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if(_idSelecionado == 0)
            {
                var autor = new Autor
                {
                    Nome = txtNome.Text.Trim(),
                    Nacionalidade = txtNacionalidade.Text.Trim()
                };
                _context.Autores.Add(autor);
            }
            else
            {
                var autor = _context.Autores.Find.Trim(_idSelecionado);
               if (autor != null)
                {
                    autor.Nome = txtNome.Text.Trim();
                    autor.Nacionalidade = txtNacionalidade.Text.Trim();
                    _context.Autores.Update(autor);
                }
            }

            _context.SaveChanges();
            MessageBox.Show("Autor salvo com sucesso!", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Limpar();
            CarregarGrid();
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if(_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um autor para excluir!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                 "Tem certeza que deseja excluir este autor?\nOs livros vinculados também serão excluídos!",
                "Confirmar exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var autor = _context.Autores
                    .Include(a => a.Livros)
                    .FirstOrDefault(a => a.Id == _idSelecionado);

                if (autor != null)
                {
                    _context.Livros.RemoveRange(autor.Livros);
                    _context.Autores.Remove(autor);
                    _context.SaveChanges();
                    MessageBox.Show("Autor excluído com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpar();
                    CarregarGrid();
                }
            }
        }


    }
}