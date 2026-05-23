namespace Sistema_Biblioteca_02
{
    public partial class Form1 : Form
    {
        private Label lblTitulo = new()
        {
            Text = "Sistema de Biblioteca",
            Font = new Font("Arial", 16, FontStyle.Bold),
            Top = 20,
            Left = 30,
            Width = 320,
            Height = 40
        };

        private Button btnAutores = new()
        {
            Text = "Gerenciar Autores",
            Width = 220,
            Height = 55,
            Top = 80,
            Left = 40,
            Font = new Font("Arial", 11)
        };

        private Button btnLivros = new()
        {
            Text = "Gerenciar Livros",
            Width = 220,
            Height = 55,
            Top = 155,
            Left = 40,
            Font = new Font("Arial", 11)
        };

        public Form1()
        {
            InitializeComponent();
            Text = "Biblioteca";
            Width = 320;
            Height = 280;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Controls.AddRange(new Control[] { lblTitulo, btnAutores, btnLivros });

            btnAutores.Click += (s, e) => new Forms.FormAutores().ShowDialog();
            btnLivros.Click += (s, e) => new Forms.FormLivros().ShowDialog();
        }
    }
}