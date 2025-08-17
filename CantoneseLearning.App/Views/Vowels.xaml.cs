using System;
using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.Model;
using viwik.CantoneseLearning.BLL.MAUI.Manager;

namespace viwik.CantoneseLearning.App.Views;

public partial class Vowels : ContentPage
{
    private SettingInfo setting;

    public Vowels()
	{
		InitializeComponent();

        this.setting = SettingManager.GetSetting();

        this.LoadData();
	}

    private async void LoadData()
    {
        int itemsCountOfEachRow = 4;

        var vowels = (await DataProcessor.GetCantoneseVowel_GPYPs()).ToList();

        List<V_CantoneseVowel_YPGP> vowelList = new List<V_CantoneseVowel_YPGP>();
        List<string> vowelNames = new List<string>();

        foreach (var vowel in vowels)
        {
            string v = this.setting.PinYinMode == PinYinMode.GP ? vowel.Vowel_GP : vowel.Vowel_YP;         

            if (vowelNames.Contains(v))
            {
                continue;
            }

            vowelList.Add(vowel);
        }

        int vowelCount = vowelList.Count();
        int vowelRowCount = vowelCount % itemsCountOfEachRow == 0 ? vowelCount / itemsCountOfEachRow : vowelCount / itemsCountOfEachRow + 1;

        RowDefinition[] vowelRowDefinations = new RowDefinition[vowelRowCount];

        for (int i = 0; i < vowelRowDefinations.Length; i++)
        {
            vowelRowDefinations[i] = new RowDefinition(new GridLength(0, GridUnitType.Star));
        }

        this.gvVowel.RowDefinitions = new RowDefinitionCollection(vowelRowDefinations);

        int index = 0;      

        for (int i = 0; i < vowelRowCount; i++)
        {
            for (int j = 0; j < itemsCountOfEachRow; j++)
            {
                var vowel = vowelList[index];

                string v = this.setting.PinYinMode == PinYinMode.GP ? vowel.Vowel_GP : vowel.Vowel_YP;                

                Button btn = new Button() { Text = v, CommandParameter = vowel };
                btn.BindingContext = vowel;
                btn.Clicked += this.Button_Clicked;

                btn.Style = (Style)Resources["VowelButtonStyle"];

                this.gvVowel.Add(btn, j, i);

                index++;

                if (index >= vowelCount)
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
            var vowel = btn.BindingContext as V_CantoneseVowel_YPGP;

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/Vowel/{vowel.Vowel_YP}.mp3");
        }
    }
}