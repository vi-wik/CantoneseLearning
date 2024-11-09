using Plugin.Maui.Audio;
using System.Text;

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

        public static async void SpeechMandarin(string content)
        {
            IEnumerable<Locale> locales = await TextToSpeech.Default.GetLocalesAsync();

            SpeechOptions options = new SpeechOptions()
            {
                Pitch = 1.5f,   // 0.0 - 2.0
                Volume = 1f, // 0.0 - 1.0
                Locale = locales.FirstOrDefault(item => item.Name == "zh")
            };

            await TextToSpeech.Default.SpeakAsync(content, options);
        }
    }
}
