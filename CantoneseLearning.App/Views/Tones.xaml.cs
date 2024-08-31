using CantoneseLearning.Model;

namespace CantoneseLearning.App;

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

            AudioPlayer.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Tones/{tone}.mp3");
        }
    }
}