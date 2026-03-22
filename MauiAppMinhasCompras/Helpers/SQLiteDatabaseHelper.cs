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
        bool _initialized = false;

        SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            // removi o .wait pra evitar o deadlock, mas isso significa que a tabela só será criada quando o primeiro método for chamado
        }

        async Task EnsureInitializedAsync()
        {
            if (_initialized) return;
            await _conn.CreateTableAsync<Produto>();
            _initialized = true;
        }

        public async Task<int> Insert(Produto p)
        {
            await EnsureInitializedAsync();
            return await _conn.InsertAsync(p);
        }

        public async Task<int> Update(Produto p)
        {
            await EnsureInitializedAsync();
            return await _conn.UpdateAsync(p);
        }

        public async Task<int> Delete(int id)
        {
            await EnsureInitializedAsync();
            return await _conn.DeleteAsync<Produto>(id);
        }

        public async Task<Produto?> GetById(int id)
        {
            await EnsureInitializedAsync();
            return await _conn.Table<Produto>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Produto>> GetAll()
        {
            await EnsureInitializedAsync();
            return await _conn.Table<Produto>().ToListAsync();
        }

        // Usando LINQ para evitar SQL injection
        public async Task<List<Produto>> Search(string q)
        {
            await EnsureInitializedAsync();
            return await _conn.Table<Produto>()
                .Where(p => p.Descricao != null && p.Descricao.Contains(q))
                .ToListAsync();
        }
    }
}
