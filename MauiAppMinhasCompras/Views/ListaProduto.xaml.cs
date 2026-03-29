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
            ListaProdutos.ItemsSource = new ObservableCollection<Produto>(_listaOriginal);
        else
        {
            var filtrado = _listaOriginal
                .Where(p => p.Descricao != null && p.Descricao.Contains(texto, StringComparison.OrdinalIgnoreCase))
                .ToList();
            ListaProdutos.ItemsSource = new ObservableCollection<Produto>(filtrado);
        }
    }

    async void OnNovoClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    async void OnRelatorioClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCompras());
    }

    async void OnEditarClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Produto p)
        {
            await Navigation.PushAsync(new EditarProduto(p.Id));
        }
    }

    async void OnExcluirClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Produto p)
        {
            bool confirmar = await DisplayAlertAsync("Confirmar", $"Excluir \"{p.Descricao}\"?", "Sim", "Não");
            if (!confirmar) return;

            try
            {
                await SQLiteDatabaseHelper.Instance.Delete(p.Id);
                CarregarProdutos();
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Erro", ex.Message, "OK");
            }
        }
    }
}
