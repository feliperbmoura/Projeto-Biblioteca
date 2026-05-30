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
        private Label lblBusca = new() { Text = "Buscar:", Top = 25, Left = 430 };

        private TextBox txtNome = new() { PlaceholderText = "Nome do autor", Width = 200, Top = 20, Left = 110 };
        private TextBox txtNacionalidade = new() { PlaceholderText = "Ex: Brasileiro", Width = 200, Top = 60, Left = 110 };
        private TextBox txtBusca = new() { PlaceholderText = "Buscar por nome...", Width = 150, Top = 20, Left = 480 };

        private Button btnSalvar = new() { Text = "Salvar", Top = 100, Left = 10, Width = 100, Height = 35 };
        private Button btnExcluir = new() { Text = "Excluir", Top = 100, Left = 120, Width = 100, Height = 35 };
        private Button btnLimpar = new() { Text = "Limpar", Top = 100, Left = 230, Width = 100, Height = 35 };
        private Button btnBuscar = new() { Text = "Buscar", Top = 18, Left = 640, Width = 80, Height = 28 };

        private DataGridView grid = new()
        {
            Top = 150,
            Left = 10,
            Width = 720,
            Height = 280,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        private Button btnAnterior = new() { Text = "< Anterior", Top = 440, Left = 10, Width = 100, Height = 30 };
        private Button btnProximo = new() { Text = "Proximo >", Top = 440, Left = 120, Width = 100, Height = 30 };
        private Label lblPagina = new() { Text = "Pagina 1", Top = 445, Left = 240, Width = 200 };

        private int _idSelecionado = 0;
        private int _paginaAtual = 1;
        private const int _itensPorPagina = 10;

        public FormAutores()
        {
            InitializeComponent();
            Text = "Cadastro de Autores";
            Width = 760;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.AddRange(new Control[]
            {
                lblNome, lblNacionalidade, lblBusca,
                txtNome, txtNacionalidade, txtBusca,
                btnSalvar, btnExcluir, btnLimpar, btnBuscar,
                grid,
                btnAnterior, btnProximo, lblPagina
            });

            btnSalvar.Click += BtnSalvar_Click;
            btnExcluir.Click += BtnExcluir_Click;
            btnLimpar.Click += (s, e) => Limpar();
            grid.CellClick += Grid_CellClick;
            btnBuscar.Click += (s, e) => { _paginaAtual = 1; CarregarGrid(); };
            btnAnterior.Click += (s, e) => { if (_paginaAtual > 1) { _paginaAtual--; CarregarGrid(); } };
            btnProximo.Click += (s, e) => { _paginaAtual++; CarregarGrid(); };

            CarregarGrid();
        }

        private void CarregarGrid()
        {
            var query = _context.Autores
                .Include(a => a.Livros)
                .Where(a => string.IsNullOrEmpty(txtBusca.Text) ||
                            a.Nome.Contains(txtBusca.Text))
                .Select(a => new
                {
                    a.Id,
                    a.Nome,
                    a.Nacionalidade,
                    TotalLivros = a.Livros.Count
                });

            int total = query.Count();
            int totalPaginas = (int)Math.Ceiling(total / (double)_itensPorPagina);

            if (_paginaAtual > totalPaginas && totalPaginas > 0)
                _paginaAtual = totalPaginas;

            grid.DataSource = query
                .Skip((_paginaAtual - 1) * _itensPorPagina)
                .Take(_itensPorPagina)
                .ToList();

            lblPagina.Text = $"Pagina {_paginaAtual} de {(totalPaginas == 0 ? 1 : totalPaginas)}";
            btnAnterior.Enabled = _paginaAtual > 1;
            btnProximo.Enabled = _paginaAtual < totalPaginas;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome e Obrigatorio!", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_idSelecionado == 0)
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
                var autor = _context.Autores.Find(new object[] { _idSelecionado });
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
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um autor para excluir!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "Tem certeza que deseja excluir este autor?\nOs livros vinculados tambem serao excluidos!",
                "Confirmar exclusao",
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
                    MessageBox.Show("Autor excluido com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Limpar();
                    CarregarGrid();
                }
            }
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = grid.Rows[e.RowIndex];
            _idSelecionado = (int)row.Cells["Id"].Value;
            txtNome.Text = row.Cells["Nome"].Value?.ToString();
            txtNacionalidade.Text = row.Cells["Nacionalidade"].Value?.ToString();
        }

        private void Limpar()
        {
            txtNome.Clear();
            txtNacionalidade.Clear();
            _idSelecionado = 0;
            grid.ClearSelection();
        }
    }
}