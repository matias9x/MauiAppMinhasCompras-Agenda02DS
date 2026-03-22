using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Model;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
    readonly int _id;

    public EditarProduto(int id)
    {
        _id = id;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var p = await SQLiteDatabaseHelper.Instance.GetById(_id);
        if (p == null)
        {
            await DisplayAlertAsync("Erro", "Produto não encontrado.", "OK");
            await Navigation.PopAsync();
            return;
        }
        EntryDescricao.Text = p.Descricao;
        EntryQuantidade.Text = p.Quantidade.ToString(System.Globalization.CultureInfo.InvariantCulture);
        EntryPreco.Text = p.Preco.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
    }

    async void OnSalvarClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(EntryDescricao.Text))
        {
            await DisplayAlertAsync("Atenção", "Informe a descrição.", "OK");
            return;
        }
        if (!double.TryParse(EntryQuantidade.Text?.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var qtd) || qtd <= 0)
        {
            await DisplayAlertAsync("Atenção", "Quantidade inválida.", "OK");
            return;
        }
        if (!double.TryParse(EntryPreco.Text?.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var preco) || preco < 0)
        {
            await DisplayAlertAsync("Atenção", "Preço inválido.", "OK");
            return;
        }
        try
        {
            var p = new Produto
            {
                Id = _id,
                Descricao = EntryDescricao.Text!.Trim(),
                Quantidade = qtd,
                Preco = preco
            };
            await SQLiteDatabaseHelper.Instance.Update(p);
            await DisplayAlertAsync("Sucesso", "Produto atualizado.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    async void OnExcluirClicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlertAsync("Confirmar", "Deseja excluir este produto?", "Sim", "Não");
        if (!confirmar) return;
        try
        {
            await SQLiteDatabaseHelper.Instance.Delete(_id);
            await DisplayAlertAsync("Sucesso", "Produto excluído.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
}
