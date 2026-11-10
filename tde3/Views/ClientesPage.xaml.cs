using tde3.Models;
using tde3.Data;

namespace tde3.Views
{
    public partial class ClientesPage : ContentPage
    {
        private readonly DatabaseService _dbService;
        private Cliente _clienteSelecionado;

        public ClientesPage()
        {
            InitializeComponent();

            // Vai buscar o serviço de base de dados que registámos no MauiProgram
            _dbService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<DatabaseService>();

            CarregarClientes();
        }

        // Lê do Banco de Dados (READ)
        private async void CarregarClientes()
        {
            var clientes = await _dbService.GetClientesAsync();
            ClientesListView.ItemsSource = clientes;
        }

        // Insere no Banco de Dados (CREATE)
        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            // EXIGÊNCIA 2: Validações de Back-end
            if (string.IsNullOrWhiteSpace(NomeEntry.Text) || string.IsNullOrWhiteSpace(RgEntry.Text) || string.IsNullOrWhiteSpace(TelefoneEntry.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
                return;
            }

            if (!int.TryParse(RgEntry.Text, out int rg))
            {
                await DisplayAlert("Erro", "O RG deve conter apenas números válidos.", "OK");
                return;
            }

            var novoCliente = new Cliente
            {
                Nome = NomeEntry.Text,
                RG = rg,
                Telefone = TelefoneEntry.Text
            };

            await _dbService.InsertClienteAsync(novoCliente);
            await DisplayAlert("Sucesso", "Cliente registado com sucesso!", "OK");

            LimparCampos();
            CarregarClientes();
        }

        // Ao clicar num cliente da lista, preenche os campos para podermos editar ou apagar
        private void OnClienteSelecionado(object sender, SelectionChangedEventArgs e)
        {
            _clienteSelecionado = e.CurrentSelection.FirstOrDefault() as Cliente;
            if (_clienteSelecionado != null)
            {
                NomeEntry.Text = _clienteSelecionado.Nome;
                RgEntry.Text = _clienteSelecionado.RG.ToString();
                TelefoneEntry.Text = _clienteSelecionado.Telefone;
            }
        }

        // Atualiza no Banco de Dados (UPDATE)
        private async void OnAtualizarClicked(object sender, EventArgs e)
        {
            if (_clienteSelecionado == null)
            {
                await DisplayAlert("Aviso", "Selecione um cliente na lista abaixo para o atualizar.", "OK");
                return;
            }

            if (!int.TryParse(RgEntry.Text, out int rg))
            {
                await DisplayAlert("Erro", "O RG deve conter apenas números.", "OK");
                return;
            }

            _clienteSelecionado.Nome = NomeEntry.Text;
            _clienteSelecionado.RG = rg;
            _clienteSelecionado.Telefone = TelefoneEntry.Text;

            await _dbService.UpdateClienteAsync(_clienteSelecionado);
            await DisplayAlert("Sucesso", "Dados do cliente atualizados!", "OK");

            LimparCampos();
            CarregarClientes();
        }

        // Apaga do Banco de Dados (DELETE)
        private async void OnApagarClicked(object sender, EventArgs e)
        {
            if (_clienteSelecionado == null)
            {
                await DisplayAlert("Aviso", "Selecione um cliente na lista abaixo para apagar.", "OK");
                return;
            }

            bool confirmar = await DisplayAlert("Confirmar", $"Tem a certeza que deseja apagar o cliente {_clienteSelecionado.Nome}?", "Sim", "Não");
            if (confirmar)
            {
                await _dbService.DeleteClienteAsync(_clienteSelecionado);
                await DisplayAlert("Sucesso", "Cliente apagado!", "OK");

                LimparCampos();
                CarregarClientes();
            }
        }

        private void OnLimparClicked(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            _clienteSelecionado = null;
            NomeEntry.Text = string.Empty;
            RgEntry.Text = string.Empty;
            TelefoneEntry.Text = string.Empty;
            ClientesListView.SelectedItem = null;
        }
    }
}