using CantoneseLearning.Business;
using CantoneseLearning.Business.Model;
using CantoneseLearning.Model;
using CommunityToolkit.Maui.Views;
using System;

namespace CantoneseLearning.App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            string content = this.txtWord.Text;

            if (string.IsNullOrEmpty(content))
            {
                await DisplayAlert("提示", "请输入查询内容！", "确定");
                return;
            }

            var syllables = await DataProcessor.GetSyllables(content);

            int count = syllables.Count();

            this.BatchPronounceGrid.IsVisible = count > 1;

            if (count == 0)
            {
                await DisplayAlert("提示", "未找到任何匹配记录！", "确定");
            }

            this.lstSyllables.ItemsSource = syllables;
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

        private async void OnPronounceMandarinButtonClicked(object sender, EventArgs e)
        {
            this.PlayAudios(LanguageType.Mandarin);
        }

        private async void OnPronounceCantoneseButtonClicked(object sender, EventArgs e)
        {
            this.PlayAudios(LanguageType.Cantonese);
        }

        private async void PlayAudios(LanguageType type)
        {
            var syllables = this.lstSyllables.ItemsSource as IEnumerable<Model.Syllable>;

            if (syllables != null)
            {
                foreach (var s in syllables)
                {
                    string syllable = type == LanguageType.Mandarin ? s.Syllable_M_Full : s.SyllableFull;

                    AudioPlayer.PlayPackageFile($"Audios/{type.ToString()}/{syllable}.mp3");

                    Thread.Sleep(1000);
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var syllable = (sender as Label).BindingContext as SyllableDisplay;

            if (syllable != null && syllable.HasAlt)
            {
                this.ShowSyllableAlts(syllable);
            }           
        }

        private async void ShowSyllableAlts(SyllableDisplay syllable)
        {
            if (syllable != null)
            {
                var popup = new SyllableAlternatives(syllable);

                this.ShowPopup(popup);
            }
        }
    }
}
