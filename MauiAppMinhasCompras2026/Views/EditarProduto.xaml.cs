using MauiAppMinhasCompras2026.Models;
using System.Globalization;

namespace MauiAppMinhasCompras2026.Views;

public partial class EditarProduto : ContentPage
{
    private Produto _produtoOriginal;

    public EditarProduto(Produto produto)
    {
        InitializeComponent();

        _produtoOriginal = produto;

        // Carrega os dados do produto nos campos
        txtDescricao.Text = produto.Descricao;
        txtQuantidade.Text = produto.Quantidade.ToString(CultureInfo.InvariantCulture);
        txtPreco.Text = produto.Preco.ToString(CultureInfo.InvariantCulture);
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            // Valida os campos vazios
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                await DisplayAlert("Atenção", "Por favor, informe a descrição do produto.", "OK");
                txtDescricao.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantidade.Text))
            {
                await DisplayAlert("Atenção", "Por favor, informe a quantidade.", "OK");
                txtQuantidade.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPreco.Text))
            {
                await DisplayAlert("Atenção", "Por favor, informe o preço.", "OK");
                txtPreco.Focus();
                return;
            }

            // Tratamento de ponto/vírgula decimal
            string qteTexto = txtQuantidade.Text.Replace(',', '.');
            string precoTexto = txtPreco.Text.Replace(',', '.');

            if (!double.TryParse(qteTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out double quantidade))
            {
                await DisplayAlert("Atenção", "Quantidade inválida. Digite apenas números.", "OK");
                txtQuantidade.Focus();
                return;
            }

            if (!double.TryParse(precoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out double preco))
            {
                await DisplayAlert("Atenção", "Preço inválido. Digite apenas números.", "OK");
                txtPreco.Focus();
                return;
            }

            // Atualiza o objeto original
            _produtoOriginal.Descricao = txtDescricao.Text.Trim();
            _produtoOriginal.Quantidade = quantidade;
            _produtoOriginal.Preco = preco;

            // Executa o Update no Banco de Dados
            await App.Db.Update(_produtoOriginal);

            await DisplayAlert("Sucesso", "Produto atualizado com sucesso!", "OK");

            // Retorna para a lista de produtos
            await Navigation.PopAsync();
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
            bool confirmacao = await DisplayAlert("Confirmar", $"Deseja realmente excluir o produto '{_produtoOriginal.Descricao}'?", "Sim", "Não");

            if (confirmacao)
            {
                await App.Db.Delete(_produtoOriginal.Id);

                await DisplayAlert("Sucesso", "Produto excluído com sucesso!", "OK");

                await Navigation.PopAsync();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao excluir o produto: {ex.Message}", "OK");
        }
    }
}