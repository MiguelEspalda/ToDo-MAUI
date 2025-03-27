using MAUIApp.Pages;

namespace MAUIApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNavigateToHome(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HomePage));
        }
    }

}
