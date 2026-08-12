using MauiAppMinhasCompras2026.Helpers;
using MauiAppMinhasCompras2026.Models;

namespace MauiAppMinhasCompras2026.Views;

public partial class ListaProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _dbHelper;
    private List<Produto> _produtos;
    private bool _isLoading;

    public ListaProduto()
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "compras.db3");
        _dbHelper = new SQLiteDatabaseHelper(dbPath);

        // Carrega a lista quando a página aparecer
        Appearing += OnPageAppearing;
    }

    private async void OnPageAppearing(object sender, EventArgs e)
    {
        await CarregarProdutos();
    }

    private async Task CarregarProdutos(string busca = "")
    {
        try
        {
            _isLoading = true;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            if (string.IsNullOrWhiteSpace(busca))
            {
                _produtos = await _dbHelper.GetAll();
            }
            else
            {
                _produtos = await _dbHelper.Search(busca);
            }

            // Calcula o total de cada produto
            foreach (var produto in _produtos)
            {
                // Propriedade Total será calculada na view
                // Usando uma classe auxiliar ou propriedade calculada
            }

            listViewProdutos.ItemsSource = _produtos;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar produtos: {ex.Message}", "OK");
        }
        finally
        {
            _isLoading = false;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
    }

    private async void OnBuscaTextChanged(object sender, TextChangedEventArgs e)
    {
        // Se o texto da busca for vazio, recarrega todos os produtos
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            await CarregarProdutos();
        }
    }

    private async void OnBuscarClicked(object sender, EventArgs e)
    {
        await CarregarProdutos(txtBusca.Text);
    }

    private async void OnSomarClicked(object sender, EventArgs e)
    {
        try
        {
            if (_produtos == null || !_produtos.Any())
            {
                await DisplayAlert("Informação", "Nenhum produto cadastrado.", "OK");
                return;
            }

            decimal total = (decimal)_produtos.Sum(p => p.Quantidade * p.Preco);
            await DisplayAlert("Total da Compra", $"O valor total dos produtos é: R$ {total:F2}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao calcular total: {ex.Message}", "OK");
        }
    }

    private async void OnAdicionarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    private async void OnItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Produto produtoSelecionado)
        {
            // Desmarca o item selecionado
            ((ListView)sender).SelectedItem = null;

            // Navega para a tela de edição
            await Navigation.PushAsync(new EditarProduto(produtoSelecionado));
        }
    }

    public async Task AtualizarLista()
    {
        await CarregarProdutos();
    }
}