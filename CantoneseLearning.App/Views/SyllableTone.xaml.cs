using CantoneseLearning.Model;
using Microsoft.Maui.Controls.Shapes;

namespace CantoneseLearning.App;

public partial class SyllableTone : ContentPage
{
    public SyllableTone()
    {
        InitializeComponent();
    }

    private void ToneDemoButton_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            string syllable = btn.CommandParameter?.ToString();

            if (string.IsNullOrEmpty(syllable))
            {
                return;
            }

            AudioPlayer.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/{syllable}.mp3");
        }
    }
}