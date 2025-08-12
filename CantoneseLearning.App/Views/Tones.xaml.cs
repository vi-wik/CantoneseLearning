using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App;

public partial class Tones : ContentPage
{
	public Tones()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            string tone = btn.Text.ToString();

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Tones/{tone}.mp3");
        }
    }
}