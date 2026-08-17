using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using MauiAppMinhasCompras2026.Models;

namespace MauiAppMinhasCompras2026.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Corrigido: adicionada a vírgula após Descricao=? e alterado para ExecuteAsync
        public Task<int> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.ExecuteAsync(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        public Task<int> Delete(int id)
        {
            string sql = "DELETE FROM Produto WHERE Id=?";
            return _conn.ExecuteAsync(sql, id);
        }

        public Task<List<Produto>> GetAll()
        {
            string sql = "SELECT * FROM Produto";
            return _conn.QueryAsync<Produto>(sql);
        }

        // Corrigido: SQL clássico mantido, porém parametrizado para evitar erros de busca
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, "%" + q + "%");
        }
    }
}