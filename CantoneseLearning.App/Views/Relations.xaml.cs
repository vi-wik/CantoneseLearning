using CantoneseLearning.Business;
using CantoneseLearning.Model;

namespace CantoneseLearning.App
{
    public partial class Relations : ContentPage
    {
        private SyllableType syllableType = SyllableType.Mandarin;

        public Relations()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string syllable = this.txtSyllable.Text;

            if (string.IsNullOrEmpty(syllable))
            {
                await DisplayAlert("��ʾ", "�������ѯ���ݣ�", "ȷ��");
                return;
            }

            var syllables = await DataProcessor.GetSyllableRelations(this.syllableType, syllable);

            this.lstSyllables.ItemsSource = syllables;

            if (syllables.Count() == 0)
            {
                await DisplayAlert("��ʾ", "δ�ҵ��κ�ƥ���¼��", "ȷ��");
            }
        }

        private async void OnMandarinPronounceButtonClicked(object sender, EventArgs e)
        {
            this.PlayAudio(sender, LanguageType.Mandarin);
        }

        private async void OnCantonesePronounceButtonClicked(object sender, EventArgs e)
        {
            this.PlayAudio(sender, LanguageType.Cantonese);
        }

        private async void PlayAudio(object sender, LanguageType type)
        {
            var btn = (sender as ImageButton);

            if (btn != null)
            {
                string syllable = btn.CommandParameter?.ToString();

                if (string.IsNullOrEmpty(syllable))
                {
                    return;
                }

                AudioPlayer.PlayPackageFile($"Audios/{type.ToString()}/{syllable}.mp3");
            }
        }

        private void OnSyllableTypeRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this.syllableType = (SyllableType)Enum.Parse(typeof(SyllableType), (sender as RadioButton).Value.ToString());

            this.PanelSpecialCharacter.IsVisible = this.syllableType == SyllableType.Cantonese_GP;
        }

        private void SpecialCharacterButton_Clicked(object sender, EventArgs e)
        {
            this.txtSyllable.Text += (sender as Button).Text;            
            this.txtSyllable.Focus();
        }
    }
}