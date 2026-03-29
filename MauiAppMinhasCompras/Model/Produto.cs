using System;
using System.Collections.Generic;
using System.Text;
using SQLite;   

namespace MauiAppMinhasCompras.Model
{
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Ignore]
        public double Total => Quantidade * Preco;

        [Ignore]
        public string Resumo => $"{Quantidade:N0} un. × R$ {Preco:N2}";

        [Ignore]
        public string PrecoFormatado => $"R$ {Total:N2}";

        [Ignore]
        public string DataFormatada => DataCadastro.ToString("dd/MM/yyyy");
    }
}
