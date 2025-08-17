using Microsoft.Maui.Controls;
using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.BLL.MAUI.Manager;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App.Views;

public partial class PhoneticAlphabet : ContentPage
{
    private SettingInfo setting;

    public PhoneticAlphabet()
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

                Button btn = new Button() { Text = (setting.PinYinMode == PinYinMode.GP? consonant.Consonant_GP: consonant.Consonant_YP), CommandParameter = consonant };
                btn.Clicked += this.Button_Clicked;

                btn.Style = (Style)Application.Current.Resources["ConsonantButton"];

                this.gvConsonant.Add(btn, j, i);

                index++;

                if(index>= consonantCount)
                {
                    break;
                }
            }
        }        

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

        index = 0;     

        for (int i = 0; i < vowelRowCount; i++)
        {
            for (int j = 0; j < itemsCountOfEachRow; j++)
            {
                var vowel = vowels[index];

                string v = this.setting.PinYinMode == PinYinMode.GP ? vowel.Vowel_GP : vowel.Vowel_YP;              

                Button btn = new Button() { Text = v, CommandParameter = vowel };
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
            this.ShowDetail(btn.CommandParameter);
        }
    }
  

    private async void ShowDetail(object content)
    {
        PhoneticAlphabetDetail detail = (PhoneticAlphabetDetail)Activator.CreateInstance(typeof(PhoneticAlphabetDetail), content);

        await Navigation.PushAsync(detail);
    }
}