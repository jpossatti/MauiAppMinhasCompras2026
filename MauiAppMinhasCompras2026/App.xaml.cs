using __XamlGeneratedCode__;
using MauiAppMinhasCompras2026.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace MauiAppMinhasCompras2026
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get 
            {
                if(_db == null) 
                {
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
            MainPage = new NavigationPage(new Views.ListaProduto());
        }

        //protected override Window CreateWindow(IActivationState? activationState)
       // {
            //return new Window(new AppShell());
       // }
    }
}     