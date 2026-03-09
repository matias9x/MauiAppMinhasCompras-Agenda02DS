using System;
using System.Collections.Generic;
using System.IO;
using MauiAppMinhasCompras.Model;
using Microsoft.Maui.Storage;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        static readonly string DbPath = Path.Combine(FileSystem.AppDataDirectory, "minhascompras.db");
        static SQLiteDatabaseHelper? _instance;
        public static SQLiteDatabaseHelper Instance => _instance ??= new SQLiteDatabaseHelper(DbPath);

        readonly SQLiteAsyncConnection _conn;
        SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }
        public Task<int> Update(Produto p)
        {
            return _conn.UpdateAsync(p);
        }
        public Task<int> Delete(int id)
        {
            return _conn.DeleteAsync<Produto>(id);
        }
        public async Task<Produto?> GetById(int id)
        {
            Produto? item = await _conn.Table<Produto>().Where(x => x.Id == id).FirstOrDefaultAsync().ConfigureAwait(false);
            return item;
        }
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }
    }
}
        