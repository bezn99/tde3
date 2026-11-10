using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using tde3.Models;

namespace tde3.Data
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public DatabaseService()
        {
            // Define o caminho local onde a base de dados será guardada no telemóvel/computador
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "loja.db3");
            _db = new SQLiteAsyncConnection(dbPath);

            // Criação das 4 tabelas
            _db.CreateTableAsync<Cliente>().Wait();
            _db.CreateTableAsync<Produto>().Wait();
            _db.CreateTableAsync<Compra>().Wait();
            _db.CreateTableAsync<ItemCompra>().Wait();
        }

        // --- CRUD CLIENTE ---
        public Task<List<Cliente>> GetClientesAsync() => _db.GetAllWithChildrenAsync<Cliente>();
        public Task InsertClienteAsync(Cliente cliente) => _db.InsertWithChildrenAsync(cliente);
        public Task UpdateClienteAsync(Cliente cliente) => _db.UpdateWithChildrenAsync(cliente);
        public Task DeleteClienteAsync(Cliente cliente) => _db.DeleteAsync(cliente);

        // --- CRUD PRODUTO ---
        public Task<List<Produto>> GetProdutosAsync() => _db.GetAllWithChildrenAsync<Produto>();
        public Task InsertProdutoAsync(Produto produto) => _db.InsertWithChildrenAsync(produto);
        public Task UpdateProdutoAsync(Produto produto) => _db.UpdateWithChildrenAsync(produto);
        public Task DeleteProdutoAsync(Produto produto) => _db.DeleteAsync(produto);

        // --- CRUD COMPRA ---
        public Task<List<Compra>> GetComprasAsync() => _db.GetAllWithChildrenAsync<Compra>();
        public Task InsertCompraAsync(Compra compra) => _db.InsertWithChildrenAsync(compra);
        public Task UpdateCompraAsync(Compra compra) => _db.UpdateWithChildrenAsync(compra);
        public Task DeleteCompraAsync(Compra compra) => _db.DeleteAsync(compra);

        // --- CRUD ITEMCOMPRA ---
        public Task<List<ItemCompra>> GetItensCompraAsync() => _db.GetAllWithChildrenAsync<ItemCompra>();
        public Task InsertItemCompraAsync(ItemCompra item) => _db.InsertWithChildrenAsync(item);
        public Task UpdateItemCompraAsync(ItemCompra item) => _db.UpdateWithChildrenAsync(item);
        public Task DeleteItemCompraAsync(ItemCompra item) => _db.DeleteAsync(item);
    }
}