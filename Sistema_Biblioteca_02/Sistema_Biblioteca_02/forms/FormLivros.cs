using Microsoft.EntityFrameworkCore;
using Sistema_Biblioteca_02.Data;
using Sistema_Biblioteca_02.Models;

namespace Sistema_Biblioteca_02.Forms
{
    public partial class FormLivros : Form
    {
        private readonly AppDbContext _context = new();

        private Label lblTitulo = new() { Text = "Titulo:", Top = 25, Left = 10 };
        private Label lblGenero = new() { Text = "Genero:", Top = 65, Left = 10 };
        private Label lblAno = new() { Text = "Ano:", Top = 65, Left = 260 };
        private Label lblAutor = new() { Text = "Autor:", Top = 105, Left = 10 };

        private TextBox txtTitulo = new() { PlaceholderText = "Titulo do livro", Width = 380, Top = 20, Left = 110 };
        private TextBox txtGenero = new() { PlaceholderText = "Ex: Romance", Width = 130, Top = 60, Left = 110 };
        private TextBox txtAno = new() { PlaceholderText = "Ex: 2023", Width = 100, Top = 60, Left = 300 };
        private ComboBox cmbAutor = new() { Width = 380, Top = 100, Left = 110, DropDownStyle = ComboBoxStyle.DropDownList };

        private Button btnSalvar = new() { Text = "Salvar", Top = 145, Left = 10, Width = 100, Height = 35 };
        private Button btnExcluir = new() { Text = "Excluir", Top = 145, Left = 120, Width = 100, Height = 35 };
        private Button btnLimpar = new() { Text = "Limpar", Top = 145, Left = 230, Width = 100, Height = 35 };

        private DataGridView grid = new()
        {
            Top = 195,
            Left = 10,
            Width = 660,
            Height = 300,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        private int _idSelecionado = 0;

        public FormLivros()
        {
            InitializeComponent();
            Text = "Cadastro de Livros";
            Width = 700;
            Height = 560;
            StartPosition = FormStartPosition.CenterScreen;

            Controls.AddRange(new Control[]
            {
                lblTitulo, lblGenero, lblAno, lblAutor,
                txtTitulo, txtGenero, txtAno, cmbAutor,
                btnSalvar, btnExcluir, btnLimpar,
                grid
            });

            btnSalvar.Click += BtnSalvar_Click;
            btnExcluir.Click += BtnExcluir_Click;
            btnLimpar.Click += (s, e) => Limpar();
            grid.CellClick += Grid_CellClick;

            CarregarAutores();
            CarregarGrid();
        }

        private void CarregarAutores()
        {
            var autores = _context.Autores.ToList();
            cmbAutor.DataSource = autores;
            cmbAutor.DisplayMember = "Nome";
            cmbAutor.ValueMember = "Id";
        }

        private void CarregarGrid()
        {
            grid.DataSource = _context.Livros
                .Include(l => l.Autor)
                .Select(l => new
                {
                    l.Id,
                    l.Titulo,
                    l.Genero,
                    Ano = l.AnoPublicacao,
                    Autor = l.Autor!.Nome
                }).ToList();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("Titulo e obrigatorio!", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtAno.Text, out int ano) || ano < 1000 || ano > 2100)
            {
                MessageBox.Show("Ano invalido! Digite um ano entre 1000 e 2100.", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbAutor.SelectedValue == null)
            {
                MessageBox.Show("Selecione um autor!", "Validacao",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_idSelecionado == 0)
            {
                var livro = new Livro
                {
                    Titulo = txtTitulo.Text.Trim(),
                    Genero = txtGenero.Text.Trim(),
                    AnoPublicacao = ano,
                    AutorId = (int)cmbAutor.SelectedValue
                };
                _context.Livros.Add(livro);
            }
            else
            {
                var livro = _context.Livros.Find(new object[] { _idSelecionado });
                if (livro != null)
                {
                    livro.Titulo = txtTitulo.Text.Trim();
                    livro.Genero = txtGenero.Text.Trim();
                    livro.AnoPublicacao = ano;
                    livro.AutorId = (int)cmbAutor.SelectedValue;
                    _context.Livros.Update(livro);
                }
            }

            _context.SaveChanges();
            MessageBox.Show("Livro salvo com sucesso!", "Sucesso",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Limpar();
            CarregarGrid();
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if (_idSelecionado == 0)
            {
                MessageBox.Show("Selecione um livro para excluir!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Tem certeza que deseja excluir este livro?",
                "Confirmar exclusao", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var livro = _context.Livros.Find(new object[] { _idSelecionado });
                if (livro != null)
                {
                    _context.Livros.Remove(livro);
                    _context.SaveChanges();
                    MessageBox.Show("Livro excluido com sucesso!", "Sucesso",
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
            txtTitulo.Text = row.Cells["Titulo"].Value?.ToString();
            txtGenero.Text = row.Cells["Genero"].Value?.ToString();
            txtAno.Text = row.Cells["Ano"].Value?.ToString();

            var autorNome = row.Cells["Autor"].Value?.ToString();
            foreach (var item in cmbAutor.Items)
            {
                if (item.ToString() == autorNome)
                {
                    cmbAutor.SelectedItem = item;
                    break;
                }
            }
        }

        private void Limpar()
        {
            txtTitulo.Clear();
            txtGenero.Clear();
            txtAno.Clear();
            _idSelecionado = 0;
            if (cmbAutor.Items.Count > 0)
                cmbAutor.SelectedIndex = 0;
            grid.ClearSelection();
        }
    }
}