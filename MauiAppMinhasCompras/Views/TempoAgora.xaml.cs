using MauiAppMinhasCompras.Model;
using Newtonsoft.Json;
using System.Net;

namespace MauiAppMinhasCompras.Views;

public partial class TempoAgora : ContentPage
{
    // ⚠️ Substitua pela sua chave da OpenWeatherMap (gratuita em openweathermap.org)
    const string ApiKey = "710a677cece91efd6f393ec3fd479152";
    const string ApiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=pt_br";

    static readonly HttpClient _http = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(10)
    };

    public TempoAgora()
    {
        InitializeComponent();
    }

    async void OnBuscarClicked(object? sender, EventArgs e)
    {
        string cidade = EntryCity.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(cidade))
        {
            await DisplayAlertAsync("Atenção", "Informe o nome de uma cidade.", "OK");
            return;
        }

        // Parte 2: verificar conectividade antes de fazer a requisição
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlertAsync(
                "Sem conexão",
                "Você está sem acesso à internet. Verifique sua conexão e tente novamente.",
                "OK");
            return;
        }

        await GetPrevisao(cidade);
    }

    async Task GetPrevisao(string cidade)
    {
        // Mostra loading, esconde resultado anterior
        Loading.IsVisible = true;
        Loading.IsRunning = true;
        PainelResultado.IsVisible = false;

        try
        {
            string url = string.Format(ApiUrl, Uri.EscapeDataString(cidade), ApiKey);

            using HttpResponseMessage response = await _http.GetAsync(url);

            // Parte 2: tratamento específico por código HTTP
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                await DisplayAlertAsync(
                    "Cidade não encontrada",
                    $"Não foi possível encontrar dados para \"{cidade}\". Verifique o nome e tente novamente.",
                    "OK");
                return;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlertAsync(
                    "Chave inválida",
                    "A chave da API é inválida ou expirou. Verifique sua ApiKey.",
                    "OK");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                await DisplayAlertAsync(
                    "Erro na requisição",
                    $"O servidor retornou um erro inesperado: {(int)response.StatusCode} {response.ReasonPhrase}.",
                    "OK");
                return;
            }

            string json = await response.Content.ReadAsStringAsync();
            Tempo? tempo = JsonConvert.DeserializeObject<Tempo>(json);

            if (tempo == null)
            {
                await DisplayAlertAsync("Erro", "Não foi possível processar os dados do clima.", "OK");
                return;
            }

            ExibirResultado(tempo);
        }
        catch (TaskCanceledException)
        {
            // Timeout
            await DisplayAlertAsync(
                "Tempo esgotado",
                "A requisição demorou muito. Verifique sua conexão e tente novamente.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro inesperado", ex.Message, "OK");
        }
        finally
        {
            Loading.IsRunning = false;
            Loading.IsVisible = false;
        }
    }

    void ExibirResultado(Tempo tempo)
    {
        // Parte 1: exibir todos os campos incluindo os novos
        LabelCidade.Text = tempo.Cidade ?? "-";
        LabelTemperatura.Text = $"{tempo.Temperatura:N1} °C";
        LabelDescricao.Text = char.ToUpper(tempo.Descricao[0]) + tempo.Descricao[1..]; // capitaliza
        LabelUmidade.Text = $"{tempo.Umidade}%";
        LabelVento.Text = $"{tempo.VelocidadeVento:N1} m/s";
        LabelVisibilidade.Text = tempo.VisibilidadeFormatada;

        PainelResultado.IsVisible = true;
    }
}
