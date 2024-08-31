using Microsoft.Maui.Storage;

namespace CantoneseLearning.Business
{
    public class DataFileManager
    {
        private readonly static string dataFileName = "language.db3";
        private readonly static string dbVersionFileName = "DbVersion.txt";
        internal static string DataFolder => "data";
        internal static string DataFilePath 
        { 
            get
            {
#if IOS
                throw new NotImplementedException();
#endif
                return Path.Combine(FileSystem.Current.AppDataDirectory, DataFolder, dataFileName); 
            }
        }

        static DataFileManager()
        {
            
        }

        public static async void Init()
        {
            var appFolder = FileSystem.Current.AppDataDirectory;

            bool existsDbFile = await FileSystem.Current.AppPackageFileExistsAsync(dataFileName);
            bool existsDbVersionFile = await FileSystem.Current.AppPackageFileExistsAsync(dbVersionFileName);

            if(!existsDbFile)
            {
                throw new FileNotFoundException($@"File ""{dataFileName}"" is not found.");
            }

            if(!existsDbVersionFile)
            {
                throw new FileNotFoundException($@"File ""{dbVersionFileName}"" is not found.");
            }

            string dbFolder = Path.Combine(appFolder, DataFolder);

            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            string dbFilePath = Path.Combine(dbFolder, dataFileName);
            string dbVersionFilePath = Path.Combine(dbFolder, dbVersionFileName);

            bool hasDbFile = File.Exists(dbFilePath);
            bool hasDbVersionFile = File.Exists(dbVersionFilePath);

            bool dbCopied = false;
#if DEBUG
            await CopyPackageFileAsync(dataFileName, dbFilePath);
            dbCopied = true;
#endif

            if (!hasDbFile && !dbCopied)
            {
                await CopyPackageFileAsync(dataFileName, dbFilePath);
                dbCopied = true;
            }

            if (!hasDbVersionFile)
            {
                await CopyPackageFileAsync(dbVersionFileName, dbVersionFilePath);
            }

            if(hasDbVersionFile)
            {
                using (var dbVersionFs = await FileSystem.Current.OpenAppPackageFileAsync(dbVersionFileName))
                {
                    string dbVersion = new StreamReader(dbVersionFs).ReadToEnd();

                    string oldDbVersion = File.ReadAllText(dbVersionFilePath);

                    if(dbVersion!= oldDbVersion)
                    {
                        if(!dbCopied)
                        {
                            await CopyPackageFileAsync(dataFileName, dbFilePath);
                        }
                        
                        await CopyPackageFileAsync(dbVersionFileName, dbVersionFilePath);
                    }
                }
            }            
        }

        private static async Task CopyPackageFileAsync(string fileName, string targetFilePath)
        {
            using (Stream fs = await FileSystem.Current.OpenAppPackageFileAsync(fileName))
            {
                if (fs != null)
                {
                    FileStream target = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    StreamWriter writer = new StreamWriter(target);

                    fs.CopyTo(target);

                    writer.Flush();
                }              
            }
        }  
    }
}
