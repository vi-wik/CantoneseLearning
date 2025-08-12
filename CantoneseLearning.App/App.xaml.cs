using viwik.CantoneseLearning.BLL.MAUI.Manager;
using viwik.CantoneseLearning.DataAccess;

namespace viwik.CantoneseLearning.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DbUtitlity.DataFilePath = DataFileManager.DataFilePath;

            DataFileManager.Init();

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Exception exception = e?.ExceptionObject as Exception;

                if (exception != null)
                {
                    LogManager.LogException(exception);
                }
            };

            MainPage = new AppShell();
        }
    }
}
