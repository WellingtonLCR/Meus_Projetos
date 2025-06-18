using System;
using SQLite;

namespace PrevisaoDoTempoApp.Models
{
    public class PesquisaPrevisaoTempo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Cidade { get; set; }
        public DateTime DataPesquisa { get; set; }
    }
}