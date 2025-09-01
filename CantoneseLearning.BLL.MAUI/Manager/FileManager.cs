using viwik.CantoneseLearning.BLL.MAUI.Helper;

namespace viwik.CantoneseLearning.BLL.MAUI.Manager
{
    public class FileManager
    {
        internal static string RootFolder
        {
            get
            {
                return FileSystem.Current.AppDataDirectory;
            }
        }
    }
}
