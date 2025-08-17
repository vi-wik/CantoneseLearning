using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.Model;
using viwik.CantoneseLearning.BLL.MAUI.Manager;

namespace viwik.CantoneseLearning.App.Views;

public partial class Consonants : ContentPage
{
    private SettingInfo setting;

    public Consonants()
	{
		InitializeComponent();

        this.setting = SettingManager.GetSetting();

        this.LoadData();
	}

    private async void LoadData()
    {
        int itemsCountOfEachRow = 4;

        var consonants = (await DataProcessor.GetCantoneseConsonant_YPGPs()).ToList();

        int consonantCount = consonants.Count();
        int consonantRowCount = consonantCount % itemsCountOfEachRow == 0 ? consonantCount / itemsCountOfEachRow : consonantCount / itemsCountOfEachRow + 1;

        RowDefinition[] consonantRowDefinations = new RowDefinition[consonantRowCount];

        for (int i = 0; i < consonantRowDefinations.Length; i++)
        {
            consonantRowDefinations[i] = new RowDefinition(new GridLength(0, GridUnitType.Star));
        }

        this.gvConsonant.RowDefinitions = new RowDefinitionCollection(consonantRowDefinations);

        int index = 0;

        for (int i = 0; i < consonantRowCount; i++)
        {
            for (int j = 0; j < itemsCountOfEachRow; j++)
            {
                var consonant = consonants[index];

                Button btn = new Button() { Text = (setting.PinYinMode == PinYinMode.GP ? consonant.Consonant_GP : consonant.Consonant_YP), CommandParameter = consonant };
                btn.BindingContext = consonant;
                btn.Clicked += this.Button_Clicked;              

                btn.Style = (Style)Application.Current.Resources["ConsonantButton"];

                this.gvConsonant.Add(btn, j, i);

                index++;

                if (index >= consonantCount)
                {
                    break;
                }
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            var consonant = btn.BindingContext as V_CantoneseConsonant_YPGP;

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Consonant/{consonant.Consonant_YP}.mp3");
        }
    }
}