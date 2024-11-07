using CantoneseLearning.Business;
using CantoneseLearning.Business.Model;
using CantoneseLearning.Model;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace CantoneseLearning.App
{
    public partial class Translation : ContentPage
    {
        public Translation()
        {
            InitializeComponent();
        }

        private async void OnTranslateButtonClicked(object sender, EventArgs e)
        {
            string content = this.txtContent.Text;

            if (string.IsNullOrEmpty(content))
            {
                await DisplayAlert("提示", "请输入要翻译的内容！", "确定");
                return;
            }

            TranslateType translateType = this.rbMandarin2Cantonese.IsChecked ? TranslateType.Mandarin2Cantonese : TranslateType.Cantonese2Mandarin;

            var result = await DataProcessor.Translate(translateType, content.Trim());

            if (result.Contents.Count == 0)
            {
                result.Contents.Add(content);
            }

            resultContent.Clear();

            int rowIndex = 0;

            Grid grid = new Grid();

            foreach (var con in result.Contents)
            {
                grid.Margin = new Thickness(0, 5);
                grid.RowDefinitions = [new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) }, new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) }];
                grid.ColumnDefinitions = [new ColumnDefinition() { Width = new GridLength(0.9, GridUnitType.Star) }, new ColumnDefinition() { Width = new GridLength(0.1, GridUnitType.Star) }];
                grid.BackgroundColor = Color.Parse("AliceBlue");

                string text = result.Contents.Count > 1 ? $"{rowIndex + 1}. {con}" : con;

                Label label = new Label();
                label.Padding = new Thickness(10);
                label.Text = text;

                ImageButton image = new ImageButton();
                image.Source = "pronounce.png";
                image.HeightRequest = 20;
                image.WidthRequest = 20;
                image.CommandParameter = con;
                image.Clicked += OnCantonesePronounceButtonClicked;

                grid.Add(label, 0, rowIndex);
                grid.Add(CreatePronounceImageButton(con), 1, rowIndex);

                rowIndex++;
            }

            if (result.Examples != null)
            {
                foreach (var example in result.Examples)
                {
                    Label label = new Label();
                    label.Padding = new Thickness(10);
                    label.Text = "示例：" + example;
                    label.FontAttributes = FontAttributes.Italic;

                    grid.Add(label, 0, rowIndex);
                    grid.Add(CreatePronounceImageButton(example), 1, rowIndex);

                    rowIndex++;
                }
            }

            if (result.PatternNotes != null)
            {
                Label label = new Label();
                label.Padding = new Thickness(10);
                label.Text = "句式：" + result.PatternNotes;
                label.FontAttributes = FontAttributes.Italic;

                grid.Add(label, 0, rowIndex);
                grid.SetColumnSpan(label, 2);

                rowIndex++;
            }

            resultContent.Add(grid);
        }

        private ImageButton CreatePronounceImageButton(string text)
        {
            ImageButton imageButton = new ImageButton();
            imageButton.Source = "pronounce.png";
            imageButton.HeightRequest = 20;
            imageButton.WidthRequest = 20;
            imageButton.CommandParameter = text;
            imageButton.Clicked += OnCantonesePronounceButtonClicked;

            return imageButton;
        }


        private async void OnCantonesePronounceButtonClicked(object sender, EventArgs e)
        {
            this.PlayAudios(sender, this.rbMandarin2Cantonese.IsChecked? LanguageType.Cantonese: LanguageType.Mandarin);
        }

        private async void PlayAudios(object sender, LanguageType type)
        {
            var btn = (sender as ImageButton);

            if (btn != null)
            {
                string content = btn.CommandParameter?.ToString();

                var syllables = await DataProcessor.GetSyllables(content, true);

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
        }
    }
}
