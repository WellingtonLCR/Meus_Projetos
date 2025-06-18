using SQLite;

namespace MauiAppFit.Models
{
    public class Atividade
    {
        public int Id { get; set; }

        public string? Descricao { get; set; }

        public DateTime Data { get; set; }

        public Double? Peso { get; set; }

        public string? Observacoes { get; set; }
    }
}
