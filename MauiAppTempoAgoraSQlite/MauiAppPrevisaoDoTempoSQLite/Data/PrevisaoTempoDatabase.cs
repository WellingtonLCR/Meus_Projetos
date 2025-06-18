using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrevisaoDoTempoApp.Models;
using SQLite;

namespace MauiAppPrevisaodoTempo.Data
{
    public class PrevisaoTempoDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public PrevisaoTempoDatabase()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PrevisaoTempo.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PesquisaPrevisaoTempo>().Wait();
        }

        public Task<List<PesquisaPrevisaoTempo>> GetConsultasAsync()
        {
            return _database.Table<PesquisaPrevisaoTempo>().ToListAsync();
        }

        public Task<int> SaveConsultaAsync(PesquisaPrevisaoTempo consulta)
        {
            return _database.InsertAsync(consulta);
        }

        public Task<List<PesquisaPrevisaoTempo>> GetConsultasByDateAsync(DateTime data)
        {
            return _database.Table<PesquisaPrevisaoTempo>()
                            .Where(p => p.DataPesquisa.Date == data.Date)
                            .ToListAsync();
        }
    }
}