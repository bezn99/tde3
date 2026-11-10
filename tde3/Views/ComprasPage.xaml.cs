using System.Collections.ObjectModel;
using tde3.Models;
using tde3.Data;

namespace tde3.Views
{
    public partial class ComprasPage : ContentPage
    {
        private readonly DatabaseService _dbService;
        private ObservableCollection<ItemCompra> _carrinho; // Lista temporária para a interface
        private decimal _valorTotalDaCompra = 0;

        public ComprasPage()
        {
            InitializeComponent();
            _dbService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<DatabaseService>();

            _carrinho = new ObservableCollection<ItemCompra>();
            ItensListView.ItemsSource = _carrinho;

            CarregarDados();
        }

        // Carrega os clientes e produtos do banco de dados para os Pickers (menus suspensos)
        private async void CarregarDados()
        {
            var clientes = await _dbService.GetClientesAsync();
            var produtos = await _dbService.GetProdutosAsync();

            ClientePicker.ItemsSource = clientes;
            ProdutoPicker.ItemsSource = produtos;
        }

        // Valida e adiciona um item à lista temporária (Carrinho)
        private async void OnAdicionarItemClicked(object sender, EventArgs e)
        {
            var produtoSelecionado = ProdutoPicker.SelectedItem as Produto;

            if (produtoSelecionado == null)
            {
                await DisplayAlert("Erro", "Selecione um produto.", "OK");
                return;
            }

            if (!int.TryParse(QuantidadeEntry.Text, out int qtd) || qtd <= 0)
            {
                await DisplayAlert("Erro", "Insira uma quantidade válida.", "OK");
                return;
            }

            // Exigência bónus: Validação de stock
            if (qtd > produtoSelecionado.QtdEstoque)
            {
                await DisplayAlert("Erro", $"Estoque insuficiente. Temos apenas {produtoSelecionado.QtdEstoque} unidades.", "OK");
                return;
            }

            // Cria o item da compra
            var item = new ItemCompra
            {
                Produto = produtoSelecionado,
                CodigoProduto = produtoSelecionado.CodigoProduto,
                Quantidade = qtd,
                ValorParcial = produtoSelecionado.Preco * qtd
            };

            _carrinho.Add(item);

            // Atualiza o total
            _valorTotalDaCompra += item.ValorParcial;
            AtualizarLabelTotal();

            // Limpa os campos de inserção
            ProdutoPicker.SelectedItem = null;
            QuantidadeEntry.Text = string.Empty;
        }

        // Guarda a compra definitiva na Base de Dados
        private async void OnFinalizarCompraClicked(object sender, EventArgs e)
        {
            var clienteSelecionado = ClientePicker.SelectedItem as Cliente;

            if (clienteSelecionado == null)
            {
                await DisplayAlert("Erro", "Selecione um cliente para a compra.", "OK");
                return;
            }

            if (_carrinho.Count == 0)
            {
                await DisplayAlert("Erro", "Adicione pelo menos um produto ao carrinho.", "OK");
                return;
            }

            // Cria a Compra final
            var novaCompra = new Compra
            {
                Cliente = clienteSelecionado,
                CodigoCliente = clienteSelecionado.CodigoCliente,
                DataCompra = DateTime.Now,
                ValorTotal = _valorTotalDaCompra,
                ItensCompra = _carrinho.ToList() // Associa os itens à compra
            };

            // GUARDA NO BANCO: O SQLiteNetExtensions (InsertWithChildrenAsync) guarda a Compra e os Itens ao mesmo tempo
            await _dbService.InsertCompraAsync(novaCompra);

            // Atualiza o estoque dos produtos após a venda
            foreach (var item in _carrinho)
            {
                item.Produto.QtdEstoque -= item.Quantidade;
                await _dbService.UpdateProdutoAsync(item.Produto);
            }

            await DisplayAlert("Sucesso", "Compra finalizada e registada com sucesso!", "OK");

            LimparTela();
        }

        private void AtualizarLabelTotal()
        {
            ValorTotalLabel.Text = $"Total: R$ {_valorTotalDaCompra:F2}";
        }

        private void LimparTela()
        {
            ClientePicker.SelectedItem = null;
            ProdutoPicker.SelectedItem = null;
            QuantidadeEntry.Text = string.Empty;
            _carrinho.Clear();
            _valorTotalDaCompra = 0;
            AtualizarLabelTotal();

            // Recarrega os dados para atualizar os Pickers e o estoque visualmente
            CarregarDados();
        }
    }
}