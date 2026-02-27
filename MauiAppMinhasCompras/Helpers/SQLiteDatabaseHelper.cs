using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn; //propriedade que armazena a "conexão"
        
        public SQLiteDatabaseHelper(string path) //construtor: é chamado quando o objeto é instanciado
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p) //Declaração de um método p = parâmetro do tipo produto = classe. Só consegue fazer insert a partir de um model produto preenchido.
        {
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?"; //Emula SQL; poderia fazer como o Insert acima.

            return _conn.QueryAsync<Produto>(
                sql, p.Descricao, p.Quantidade, p.Preco, p.Id
            );
        }

        public Task<int> Delete(int id) //Para deletar basta passar a ID, não precisa passar o objeto todo.
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id); //Expressão lambda. "Para cada item, tal que item.Id (Id do item conforme model Produto) seja igual ao id fornecido" 
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