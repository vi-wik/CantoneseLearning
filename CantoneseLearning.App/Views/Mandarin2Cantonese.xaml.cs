using CantoneseLearning.Business;

namespace CantoneseLearning.App;

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
            await DisplayAlert("��ʾ", "�������ѯ���ݣ�", "ȷ��");
            return;
        }

        bool matched = false;

        foreach (var item in this.lstMandarin2Cantoneses.ItemsSource)
        {
            var mc = item as CantoneseLearning.Model.Mandarin2Cantonese;

            string mandarin = mc.Mandarin;
            string cantonese = mc.Cantonese;

            if(isMandarin)
            {
                if(this.IsMatched(word, mandarin))
                {
                    matched = true;
                }
            }    
            else
            {
                if(this.IsMatched(word, cantonese))
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
            await DisplayAlert("��ʾ", "δ�ҵ��κ�ƥ���", "ȷ��");            
        }
    }

    private bool IsMatched(string search, string content)
    {
        string[] items = content.Split(',','��');

        if(items.Contains(search))
        {
            return true;
        }

        return false;
    }
}