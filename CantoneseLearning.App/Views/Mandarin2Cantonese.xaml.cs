using viwik.CantoneseLearning.BLL.Core;
using viwik.CantoneseLearning.BLL.Core.Helper;

namespace viwik.CantoneseLearning.App;

public partial class Mandarin2Cantonese : ContentPage
{
	public Mandarin2Cantonese()
	{
		InitializeComponent();

        this.LoadDataAsync();
    }

	private async Task LoadDataAsync()
	{
        var records = await DataProcessor.GetMandarin2Cantoneses();

        this.lstMandarin2Cantoneses.ItemsSource = records;
    }

    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        string word = this.txtWord.Text.Trim();
        bool isMandarin = this.chkSearchMandarin.IsChecked;

        if(string.IsNullOrEmpty(word))
        {
            await DisplayAlert("提示", "请输入查询内容！", "确定");
            return;
        }

        bool matched = false;

        foreach (var item in this.lstMandarin2Cantoneses.ItemsSource)
        {
            var mc = item as viwik.CantoneseLearning.Model.Mandarin2Cantonese;

            string mandarin = mc.Mandarin;
            string cantonese = mc.Cantonese;

            if(isMandarin)
            {
                if(this.IsMatched(word, mandarin, mc.MandarinSynonym))
                {
                    matched = true;
                }
            }    
            else
            {
                if(this.IsMatched(word, cantonese, mc.CantoneseSynonym))
                {
                    matched = true;
                }
            }

            if(matched)
            {
                this.lstMandarin2Cantoneses.SelectedItem = item;
                this.lstMandarin2Cantoneses.ScrollTo(item, ScrollToPosition.Start, false);
                break;
            }
        }

        if(!matched)
        {
            this.lstMandarin2Cantoneses.SelectedItem = null;
            await DisplayAlert("提示", "未找到任何匹配项！", "确定");            
        }
    }

    private bool IsMatched(string search, string content, string synonym)
    {
        var items = TranslateHelper.GetItemsBySynonym(content, synonym);

        if(items.Contains(search))
        {
            return true;
        }

        return false;
    }
}