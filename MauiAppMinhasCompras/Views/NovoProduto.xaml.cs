using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Model;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
        DatePickerCadastro.Date = DateTime.Today;
    }

    async void OnSalvarClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryDescricao.Text))
        {
            await DisplayAlertAsync("Atenção", "Informe a descrição.", "OK");
            return;
        }
        if (!double.TryParse(EntryQuantidade.Text?.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var qtd) || qtd <= 0)
        {
            await DisplayAlertAsync("Atenção", "Quantidade inválida.", "OK");
            return;
        }
        if (!double.TryParse(EntryPreco.Text?.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var preco) || preco < 0)
        {
            await DisplayAlertAsync("Atenção", "Preço inválido.", "OK");
            return;
        }
        try
        {
            var p = new Produto
            {
                Descricao = EntryDescricao.Text!.Trim(),
                Quantidade = qtd,
                Preco = preco,
                DataCadastro = (DateTime)DatePickerCadastro.Date
            };
            await SQLiteDatabaseHelper.Instance.Insert(p);
            await DisplayAlertAsync("Sucesso", "Produto adicionado.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
}
