﻿using viwik.CantoneseLearning.BLL.MAUI.Helper;

namespace viwik.CantoneseLearning.BLL.MAUI.Manager
{
    public class FileManager
    {
        internal static string DataRootFolder
        {
            get
            {
                return FileSystem.Current.AppDataDirectory;
            }
        }

        internal static string CacheRootFolder
        {
            get
            {
                return FileSystem.Current.CacheDirectory;
            }
        }
    }
}
