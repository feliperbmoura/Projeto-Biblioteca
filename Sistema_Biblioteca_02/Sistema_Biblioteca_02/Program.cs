using Sistema_Biblioteca_02.Data;

namespace Sistema_Biblioteca_02
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Vai garantir que o banco e as tabelas sejam criadas
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            Application.Run(new Form1());
        }
    }
}