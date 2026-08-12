using MauiAppMinhasCompras2026.Helpers;
using MauiAppMinhasCompras2026.Models;

namespace MauiAppMinhasCompras2026.Views;

public partial class NovoProduto : ContentPage
{
    private readonly SQLiteDatabaseHelper _dbHelper;

    public NovoProduto()
    {
        InitializeComponent();

        // Inicializa o banco de dados
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "compras.db3");
        _dbHelper = new SQLiteDatabaseHelper(dbPath);

        // Garante que os campos estão habilitados
        txtDescricao.IsEnabled = true;
        txtQuantidade.IsEnabled = true;
        txtPreco.IsEnabled = true;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            // Valida os campos
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe a descrição do produto.", "OK");
                txtDescricao.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe a quantidade.", "OK");
                txtQuantidade.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                await DisplayAlert("Erro", "Por favor, informe o preço.", "OK");
                txtPreco.Focus();
                return;
            }

            // Converte e valida os valores numéricos
            if (!double.TryParse(txtQuantidade.Text, out double quantidade))
            {
                await DisplayAlert("Erro", "Por favor, insira um valor numérico válido para quantidade.", "OK");
                txtQuantidade.Focus();
                txtQuantidade.Text = string.Empty;
                return;
            }

            if (!double.TryParse(txtPreco.Text, out double preco))
            {
                await DisplayAlert("Erro", "Por favor, insira um valor numérico válido para preço.", "OK");
                txtPreco.Focus();
                txtPreco.Text = string.Empty;
                return;
            }

            // Valida se os valores são positivos
            if (quantidade <= 0)
            {
                await DisplayAlert("Erro", "A quantidade deve ser maior que zero.", "OK");
                txtQuantidade.Focus();
                txtQuantidade.Text = string.Empty;
                return;
            }

            if (preco <= 0)
            {
                await DisplayAlert("Erro", "O preço deve ser maior que zero.", "OK");
                txtPreco.Focus();
                txtPreco.Text = string.Empty;
                return;
            }

            // Cria o novo produto
            var produto = new Produto
            {
                Descricao = txtDescricao.Text.Trim(),
                Quantidade = quantidade,
                Preco = preco
            };

            // Salva no banco de dados
            int resultado = await _dbHelper.Insert(produto);

            if (resultado > 0)
            {
                // Mensagem de sucesso
                await DisplayAlert("Sucesso", "Produto cadastrado com sucesso!", "OK");

                // Limpa os campos
                txtDescricao.Text = string.Empty;
                txtQuantidade.Text = string.Empty;
                txtPreco.Text = string.Empty;

                // Foca no primeiro campo
                txtDescricao.Focus();
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível salvar o produto. Tente novamente.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao salvar o produto: {ex.Message}", "OK");
        }
    }

    // Método para limpar os campos manualmente (opcional)
    private void LimparCampos()
    {
        txtDescricao.Text = string.Empty;
        txtQuantidade.Text = string.Empty;
        txtPreco.Text = string.Empty;
        txtDescricao.Focus();
    }
}