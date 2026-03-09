using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Model;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    public ListaProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarProdutos();
    }

    async void CarregarProdutos(string? busca = null)
    {
        try
        {
            var db = SQLiteDatabaseHelper.Instance;
            var lista = string.IsNullOrWhiteSpace(busca)
                ? await db.GetAll()
                : await db.Search(busca);
            ListaProdutos.ItemsSource = lista;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        CarregarProdutos(e.NewTextValue);
    }

    async void OnNovoClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    async void OnItemSelecionado(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Model.Produto p)
            return;
        ListaProdutos.SelectedItem = null;
        await Navigation.PushAsync(new EditarProduto(p.Id));
    }
}
