using MauiAppMinhasCompras2026.Helpers;
using MauiAppMinhasCompras2026.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras2026.Views;

public partial class ListaProduto : ContentPage
{
    private ObservableCollection<Produto> _produtos = new ObservableCollection<Produto>();
    private List<Produto> _listaCompleta = new List<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarProdutos();
    }

    private async void CarregarProdutos()
    {
        try
        {
            var produtos = await App.Db.GetAll();
            _listaCompleta = produtos;

            _produtos.Clear();
            foreach (var produto in _listaCompleta)
            {
                _produtos.Add(produto);
            }

            ProdutosCollectionView.ItemsSource = null;
            ProdutosCollectionView.ItemsSource = _produtos;

            CalcularTotalGeral();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao carregar produtos: {ex.Message}", "OK");
        }
    }

    private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string textoBusca = e.NewTextValue?.ToLower() ?? "";

            var filtrados = _listaCompleta
                .Where(p => p.Descricao != null && p.Descricao.ToLower().Contains(textoBusca))
                .ToList();

            _produtos.Clear();
            foreach (var prod in filtrados)
            {
                _produtos.Add(prod);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private void CalcularTotalGeral()
    {
        try
        {
            double totalGeral = 0;
            foreach (var produto in _produtos)
            {
                totalGeral += produto.Total;
            }
            lblTotalGeral.Text = $"R$ {totalGeral:F2}";
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Erro ao calcular total: {ex.Message}", "OK");
        }
    }

    private async void ToolbarItem_Adicionar_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new NovoProduto());
        }
        catch (Exception ex)
        {
            await DisplayAlert("OPS", ex.Message, "OK");
        }
    }

    private async void ToolbarItem_Somar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (_produtos.Count == 0)
            {
                await DisplayAlert("Atenção", "Não há produtos para somar.", "OK");
                return;
            }

            double totalGeral = 0;
            foreach (var produto in _produtos)
            {
                totalGeral += produto.Total;
            }

            await DisplayAlert("Total da Compra",
                $"O valor total da sua compra é: R$ {totalGeral:F2}",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var produtoSelecionado = e.CurrentSelection[0] as Produto;
                if (produtoSelecionado != null)
                {
                    // Abre a tela de edição passando o objeto selecionado
                    await Navigation.PushAsync(new EditarProduto(produtoSelecionado));
                }

                // Desmarca o item selecionado para poder clicar novamente se necessário
                ProdutosCollectionView.SelectedItem = null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}