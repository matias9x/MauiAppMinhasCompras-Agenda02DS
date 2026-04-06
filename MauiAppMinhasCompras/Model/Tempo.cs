using Newtonsoft.Json;

namespace MauiAppMinhasCompras.Model
{
    // Mapeamento do JSON retornado pela OpenWeatherMap API
    public class Tempo
    {
        // "name": "São Paulo"
        [JsonProperty("name")]
        public string? Cidade { get; set; }

        // "main": { "temp": 25.3, "humidity": 80 }
        [JsonProperty("main")]
        public Main? Main { get; set; }

        // "wind": { "speed": 3.5 }
        [JsonProperty("wind")]
        public Wind? Wind { get; set; }

        // "visibility": 10000
        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        // "weather": [ { "description": "céu limpo" } ]
        [JsonProperty("weather")]
        public List<Weather>? Weather { get; set; }

        // Propriedades auxiliares para exibição
        public double Temperatura => Main?.Temp ?? 0;
        public int Umidade => Main?.Humidity ?? 0;
        public double VelocidadeVento => Wind?.Speed ?? 0;

        // Visibilidade em km (API retorna em metros)
        public string VisibilidadeFormatada =>
            Visibility >= 1000
                ? $"{Visibility / 1000.0:N0} km"
                : $"{Visibility} m";

        public string Descricao =>
            Weather != null && Weather.Count > 0
                ? Weather[0].Description ?? "-"
                : "-";
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
    }

    public class Weather
    {
        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}
