using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App.Views;

public partial class CompareYPGP : ContentPage
{
	public CompareYPGP()
	{
		InitializeComponent();

		this.LoadData();
	}

	private void LoadData()
	{
		this.LoadConsonants();
		this.LoadVowels();
	}


	private async void LoadConsonants()
	{
		var consonants = await DataProcessor.GetCantoneseConsonant_YPGPs();

		this.lstConsonants.ItemsSource = consonants;
	}

    private async void LoadVowels()
    {
        var vowels = await DataProcessor.GetCantoneseVowel_GPYPs();

        this.lstVowels.ItemsSource = vowels;
    }

    private void lstConsonants_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		var selectedItem = e.SelectedItem;

		if (selectedItem != null)
		{
			var consonant = (selectedItem as V_CantoneseConsonant_YPGP).Consonant_YP;

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Consonant/{consonant}.mp3");
        }
	}

    private void lstVowels_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem;

        if (selectedItem != null)
        {
            var vowel = (selectedItem as V_CantoneseVowel_YPGP).Vowel_YP;

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Vowel/{vowel}.mp3");
        }
    }
}