using CantoneseLearning.Business;
using CantoneseLearning.Model;

namespace CantoneseLearning.App;

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
		var consonants = await DataProcessor.GetCantoneseConsonant_GPYPs();

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
			var consonant = (selectedItem as CantoneseConsonant_GPYP).Consonant_YP;

            AudioPlayer.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Consonant/{consonant}.mp3");
        }
	}

    private void lstVowels_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem;

        if (selectedItem != null)
        {
            var vowel = (selectedItem as CantoneseVowel_GPYP).Vowel_YP;

            AudioPlayer.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Vowel/{vowel}.mp3");
        }
    }
}