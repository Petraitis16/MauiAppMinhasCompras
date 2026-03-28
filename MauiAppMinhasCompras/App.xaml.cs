using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    //Agenda 03
    public partial class App : Application
    {
        /*É como “avisar” o app que trabalhará com SQLite, 
         * tornando a classe sqlitedatabasehelper disponível em todo o aplicativo.
         * Campo: onde o dado está gravado. Aqui: campo implicitamente privado. Underline indica campo.
         */
        static SQLiteDatabaseHelper _db;

        //Propriedade: forma de acesso ao campo.
        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    /*Busca do caminho onde está o arquivo de banco de dados, pois cada plataforma
                     * armazena em um caminho diferente.
                     */
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            //Agenda 06
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            //MainPage = new AppShell();
            //Agenda 01: nova página abre nova ListaProduto
            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
