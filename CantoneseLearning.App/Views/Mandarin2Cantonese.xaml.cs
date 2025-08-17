using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.BLL.Core.Helper;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.App.Views;

public partial class Mandarin2Cantonese : ContentPage
{
    IEnumerable<V_Mandarin2Cantonese> records = null;

    public Mandarin2Cantonese()
    {
        InitializeComponent();

        this.LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        this.records = await DataProcessor.GetVMandarin2Cantoneses();

        this.lstMandarin2Cantoneses.ItemsSource = records;
    }

    private void OnSearchButtonClicked(object sender, EventArgs e)
    {
        this.Search();
    }

    private void txtWord_Completed(object sender, EventArgs e)
    {
        this.Search();
    }

    private async void Search()
    {
        string word = this.txtWord.Text.Trim();
        //bool isMandarin = this.chkSearchMandarin.IsChecked;

        if (string.IsNullOrEmpty(word))
        {
            await DisplayAlert("提示", "请输入查询内容！", "确定");
            return;
        }

        List<Mandarin2CantoneseResult> list = new List<Mandarin2CantoneseResult>();

        foreach (var item in this.records)
        {
            var mc = item as V_Mandarin2Cantonese;

            ResultMatchType matchType = ResultMatchType.None;

            string mandarin = mc.Mandarin;
            string cantonese = mc.Cantonese;

            matchType = this.Match(word, mandarin, mc.MandarinSynonym);

            if (matchType == ResultMatchType.None)
            {
                matchType = this.Match(word, cantonese, mc.CantoneseSynonym);
            }           

            if (matchType != ResultMatchType.None)
            {
                list.Add(new Mandarin2CantoneseResult() { Content = mc, MatchType = matchType });
            }
        }

        this.lstMandarin2Cantoneses.ItemsSource = list.OrderBy(item => item.MatchType).Select(item => item.Content);
    }

    private ResultMatchType Match(string search, string content, string synonym)
    {
        var items = TranslateHelper.GetItemsBySynonym(content, synonym);

        if (items.Contains(search))
        {
            return ResultMatchType.FullMatch;
        }
        else if (items.Any(item => item.Contains(search)))
        {
            return ResultMatchType.FuzzyMatch;
        }

        return ResultMatchType.None;
    }

    private async void OnShowMediaButtonClicked(object sender, EventArgs e)
    {
        ImageButton btn = sender as ImageButton;

        V_Mandarin2Cantonese mandarin2Cantonese = btn.BindingContext as V_Mandarin2Cantonese;

        if (mandarin2Cantonese != null)
        {
            var medias = await DataProcessor.GetVVocabularyMedias(mandarin2Cantonese.Id);      

            MediaList mediaList = (MediaList)Activator.CreateInstance(typeof(MediaList), medias);

            mediaList.Title = mandarin2Cantonese.Cantonese;

            await Navigation.PushAsync(mediaList);
        }       
    }

    private void txtWord_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(this.txtWord.Text?.Trim()))
        {
            this.lstMandarin2Cantoneses.ItemsSource = this.records;
        }
    }

    internal class Mandarin2CantoneseResult
    {
        public V_Mandarin2Cantonese Content { get; set; }
        public ResultMatchType MatchType { get; set; }
    }

    internal enum ResultMatchType
    {
        None = 0,
        FullMatch = 1,
        FuzzyMatch = 2
    }   
}