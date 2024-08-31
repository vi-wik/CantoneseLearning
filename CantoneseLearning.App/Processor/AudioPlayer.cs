using Plugin.Maui.Audio;

namespace CantoneseLearning.App
{
    public class AudioPlayer
    {
        public static async void PlayPackageFile(string file)
        {
            if(await FileSystem.AppPackageFileExistsAsync(file))
            {
                var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(file));

                audioPlayer.Play();
            }           
        }
    }
}
