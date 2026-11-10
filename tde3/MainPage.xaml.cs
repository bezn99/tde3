using tde3.Views;

namespace tde3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnClientesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientesPage());
        }

        private async void OnProdutosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProdutosPage());
        }

        private async void OnComprasClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ComprasPage());
        }
    }
}