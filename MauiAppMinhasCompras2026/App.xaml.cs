using Microsoft.Extensions.DependencyInjection;

namespace MauiAppMinhasCompras2026
{
    public partial class App : Application
    {
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