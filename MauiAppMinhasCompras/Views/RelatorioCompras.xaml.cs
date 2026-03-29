using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Model;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class RelatorioCompras : ContentPage
{
    public RelatorioCompras()
    {
        InitializeComponent();
        DatePickerInicio.Date = DateTime.Today.AddDays(-30);
        DatePickerFim.Date = DateTime.Today;
    }

    async void OnBuscarClicked(object? sender, EventArgs e)
    {
        DateTime inicio = (DateTime)DatePickerInicio.Date;
        DateTime fim = (DateTime)DatePickerFim.Date;

        if (inicio > fim)
        {
            await DisplayAlertAsync("Atenção", "A data inicial não pode ser maior que a data final.", "OK");
            return;
        }
        try
        {
            var lista = await SQLiteDatabaseHelper.Instance.GetByPeriodo(inicio, fim);

            ListaRelatorio.ItemsSource = new ObservableCollection<Produto>(lista);

            double total = lista.Sum(p => p.Total);
            LabelTotal.Text = $"Total do período: {total:C}";
            PainelTotal.IsVisible = lista.Count > 0;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
}
