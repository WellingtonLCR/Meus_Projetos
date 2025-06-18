using MauiAppPrevisaodoTempo.Data;

namespace MauiAppPrevisaodoTempo
{
    public partial class App : Application
    {
        private static PrevisaoTempoDatabase _database;
        public static PrevisaoTempoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new PrevisaoTempoDatabase();
                }
                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }
    }
}