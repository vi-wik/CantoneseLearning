using CantoneseLearning.Model;

namespace CantoneseLearning.App;

public partial class Vowels : ContentPage
{	
	public Vowels()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            string vowel = btn.Text.ToString();

            AudioPlayer.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Vowel/{vowel}.mp3");
        }
    }
}