using System;
using SQLite;

namespace PrevisaoDoTempoApp.Models
{
    public class PrevisaoTempo
    {
        public int Id { get; set; }
        public string Cidade { get; set; }
        public double Temperatura { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
    }
}