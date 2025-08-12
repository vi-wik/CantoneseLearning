using viwik.CantoneseLearning.Model;
using Microsoft.Maui.Controls.Shapes;
using viwik.CantoneseLearning.App.Helper;

namespace viwik.CantoneseLearning.App;

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

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/{syllable}.mp3");
        }
    }
}