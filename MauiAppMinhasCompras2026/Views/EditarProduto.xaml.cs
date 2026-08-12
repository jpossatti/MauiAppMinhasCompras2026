using MauiAppMinhasCompras2026.Helpers;
using MauiAppMinhasCompras2026.Models;

namespace MauiAppMinhasCompras2026.Views;

public partial class EditarProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _dbHelper;
    private Produto _produtoOriginal;

    public EditarProduto(Produto produto)
    {
        InitializeComponent();

        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "compras.db3");
        _dbHelper = new SQLiteDatabaseHelper(dbPath);

        _produtoOriginal = produto;

        // Carrega os dados do produto nos campos
        txtDescricao.Text = produto.Descricao;
        txtQuantidade.Text = produto.Quantidade.ToString();
        txtPreco.Text = produto.Preco.ToString();
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            // Valida os campos
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe a descrição do produto.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe a quantidade.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe o preço.", "OK");
                return;
            }

            // Atualiza os dados do produto
            _produtoOriginal.Descricao = txtDescricao.Text.Trim();
            _produtoOriginal.Quantidade = Convert.ToDouble(txtQuantidade.Text);
            _produtoOriginal.Preco = Convert.ToDouble(txtPreco.Text);

            // Salva no banco de dados (usando Update)
            await _dbHelper.Update(_produtoOriginal);

            // Mensagem de sucesso
            await DisplayAlert("Sucesso", "Produto atualizado com sucesso!", "OK");

            // Volta para a lista
            await Navigation.PopAsync();
        }
        catch (FormatException)
        {
            await DisplayAlert("Erro", "Por favor, insira valores numéricos válidos para quantidade e preço.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao salvar o produto: {ex.Message}", "OK");
        }
    }

    private async void OnExcluirClicked(object sender, EventArgs e)
    {
        try
        {
            // Confirma a exclusão
            bool confirmacao = await DisplayAlert("Confirmar", $"Deseja realmente excluir o produto '{_produtoOriginal.Descricao}'?", "Sim", "Não");

            if (confirmacao)
            {
                // Exclui o produto
                await _dbHelper.Delete(_produtoOriginal.Id);

                await DisplayAlert("Sucesso", "Produto excluído com sucesso!", "OK");

                // Volta para a lista
                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao excluir o produto: {ex.Message}", "OK");
        }
    }
}