using CantoneseLearning.Business;

namespace CantoneseLearning.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DataFileManager.Init();

            MainPage = new AppShell();
        }
    }
}
