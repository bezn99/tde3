using tde3.Models;
using tde3.Data;

namespace tde3.Views
{
    public partial class ProdutosPage : ContentPage
    {
        private readonly DatabaseService _dbService;
        private Produto _produtoSelecionado;

        public ProdutosPage()
        {
            InitializeComponent();
            _dbService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<DatabaseService>();
            CarregarProdutos();
        }

        private async void CarregarProdutos()
        {
            var produtos = await _dbService.GetProdutosAsync();
            ProdutosListView.ItemsSource = produtos;
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(TipoEntry.Text) ||
                string.IsNullOrWhiteSpace(PrecoEntry.Text) || string.IsNullOrWhiteSpace(EstoqueEntry.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            if (!decimal.TryParse(PrecoEntry.Text, out decimal preco))
            {
                await DisplayAlert("Erro", "O preço deve ser numérico.", "OK");
                return;
            }

            if (!int.TryParse(EstoqueEntry.Text, out int estoque))
            {
                await DisplayAlert("Erro", "O estoque deve ser um número inteiro.", "OK");
                return;
            }

            var novoProduto = new Produto
            {
                NomeProduto = NomeEntry.Text,
                Tipo = TipoEntry.Text,
                Preco = preco,
                QtdEstoque = estoque
            };

            await _dbService.InsertProdutoAsync(novoProduto);
            await DisplayAlert("Sucesso", "Produto registado com sucesso!", "OK");

            LimparCampos();
            CarregarProdutos();
        }

        private void OnProdutoSelecionado(object sender, SelectionChangedEventArgs e)
        {
            _produtoSelecionado = e.CurrentSelection.FirstOrDefault() as Produto;
            if (_produtoSelecionado != null)
            {
                NomeEntry.Text = _produtoSelecionado.NomeProduto;
                TipoEntry.Text = _produtoSelecionado.Tipo;
                PrecoEntry.Text = _produtoSelecionado.Preco.ToString();
                EstoqueEntry.Text = _produtoSelecionado.QtdEstoque.ToString();
            }
        }

        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            if (_produtoSelecionado == null)
            {
                await DisplayAlert("Aviso", "Selecione um produto na lista para atualizar.", "OK");
                return;
            }

            if (!decimal.TryParse(PrecoEntry.Text, out decimal preco) || !int.TryParse(EstoqueEntry.Text, out int estoque))
            {
                await DisplayAlert("Erro", "Verifique se o preço e o estoque contêm números válidos.", "OK");
                return;
            }

            _produtoSelecionado.NomeProduto = NomeEntry.Text;
            _produtoSelecionado.Tipo = TipoEntry.Text;
            _produtoSelecionado.Preco = preco;
            _produtoSelecionado.QtdEstoque = estoque;

            await _dbService.UpdateProdutoAsync(_produtoSelecionado);
            await DisplayAlert("Sucesso", "Produto atualizado!", "OK");

            LimparCampos();
            CarregarProdutos();
        }

        private async void OnApagarClicked(object sender, EventArgs e)
        {
            if (_produtoSelecionado == null)
            {
                await DisplayAlert("Aviso", "Selecione um produto para apagar.", "OK");
                return;
            }

            bool confirmar = await DisplayAlert("Confirmar", $"Tem a certeza que deseja apagar {_produtoSelecionado.NomeProduto}?", "Sim", "Não");
            if (confirmar)
            {
                await _dbService.DeleteProdutoAsync(_produtoSelecionado);
                await DisplayAlert("Sucesso", "Produto apagado!", "OK");

                LimparCampos();
                CarregarProdutos();
            }
        }

        private void OnLimparClicked(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            _produtoSelecionado = null;
            NomeEntry.Text = string.Empty;
            TipoEntry.Text = string.Empty;
            PrecoEntry.Text = string.Empty;
            EstoqueEntry.Text = string.Empty;
            ProdutosListView.SelectedItem = null;
        }
    }
}