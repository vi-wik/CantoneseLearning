using CantoneseLearning.Business;
using CantoneseLearning.Business.Model;
using CantoneseLearning.Model;
using CommunityToolkit.Maui.Views;

namespace CantoneseLearning.App;

public partial class SyllableAlternatives : Popup
{
    private SyllableDisplay syllable;

    public SyllableAlternatives()
    {
        InitializeComponent();
    }

    public SyllableAlternatives(SyllableDisplay syllable)
    {
        InitializeComponent();

        this.syllable = syllable;

        this.LoadData();
    }

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
       
    }

    private async void LoadData()
    {
        if (this.syllable != null)
        {
            this.lblWord.Text = syllable.Word;
            this.lblDescription.Text = $"({syllable.SyllableFull_Display})µÄÒì¶ÁÒôÓÐ£º";

            var syllables = await DataProcessor.GetCantoneseSyllableAlts(this.syllable.Id);

            this.lstSyllables.ItemsSource = syllables;
            this.BatchPronounceGrid.IsVisible = syllables.Count() > 1;
        }
    }

    private async void OnPronounceCantoneseButtonClicked(object sender, EventArgs e)
    {
        this.PlayAudios(LanguageType.Cantonese);
    }

    private async void OnCantonesePronounceButtonClicked(object sender, EventArgs e)
    {
        this.PlayAudio(sender, LanguageType.Cantonese);
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

    private async void OnCloseImageClicked(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}