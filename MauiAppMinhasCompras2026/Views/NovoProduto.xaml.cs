using MauiAppMinhasCompras2026.Helpers;
using MauiAppMinhasCompras2026.Models;

namespace MauiAppMinhasCompras2026.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // 1. Validação de campos vazios
            if (string.IsNullOrWhiteSpace(txt_descricao.Text))
            {
                await DisplayAlert("Atenção", "Por favor, preencha a descrição do produto.", "OK");
                txt_descricao.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_quantidade.Text))
            {
                await DisplayAlert("Atenção", "Por favor, preencha a quantidade.", "OK");
                txt_quantidade.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_preco.Text))
            {
                await DisplayAlert("Atenção", "Por favor, preencha o preço unitário.", "OK");
                txt_preco.Focus();
                return;
            }

            // 2. Tratamento para aceitar tanto ponto quanto vírgula decimal
            string qteTexto = txt_quantidade.Text.Replace(',', '.');
            string precoTexto = txt_preco.Text.Replace(',', '.');

            if (!double.TryParse(qteTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double quantidade))
            {
                await DisplayAlert("Atenção", "Quantidade inválida. Digite apenas números.", "OK");
                txt_quantidade.Focus();
                return;
            }

            if (!double.TryParse(precoTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double preco))
            {
                await DisplayAlert("Atenção", "Preço inválido. Digite apenas números.", "OK");
                txt_preco.Focus();
                return;
            }

            // 3. Criação do objeto e Inserção no Banco
            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = quantidade,
                Preco = preco
            };

            await App.Db.Insert(p);

            await DisplayAlert("Sucesso", "Produto cadastrado com sucesso!", "OK");

            // 4. Retorna para a tela de listagem
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao Salvar", ex.Message, "OK");
        }
    }
}