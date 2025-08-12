using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App;

public partial class Consonants : ContentPage
{

	public Consonants()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            string consonant = btn.Text.ToString();            

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Consonant/{consonant}.mp3");
        }
    }
}