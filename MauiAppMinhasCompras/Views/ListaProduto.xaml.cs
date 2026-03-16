using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Model;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    List<Produto> _listaOriginal = new List<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarProdutos();
    }

    async void CarregarProdutos()
    {
        try
        {
            _listaOriginal = await SQLiteDatabaseHelper.Instance.GetAll();
            ListaProdutos.ItemsSource = new ObservableCollection<Produto>(_listaOriginal);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        string texto = e.NewTextValue;

        if (string.IsNullOrWhiteSpace(texto))
        {
            ListaProdutos.ItemsSource = new ObservableCollection<Produto>(_listaOriginal);
        }
        else
        {
            var filtrado = _listaOriginal
                .Where(p => p.Descricao.ToLower().Contains(texto.ToLower()))
                .ToList();
            ListaProdutos.ItemsSource = new ObservableCollection<Produto>(filtrado);
        }
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
