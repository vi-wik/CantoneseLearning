using CommunityToolkit.Maui.Storage;
using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.BLL.MAUI.Helper;

namespace viwik.CantoneseLearning.App.Views;

public partial class TextViewer : ContentPage
{
    private string text;
    private FileInfo file;

    public TextViewer(string text)
    {
        InitializeComponent();

        this.text = text;

        this.Init();
    }

    public TextViewer(FileInfo file)
    {
        InitializeComponent();

        this.file = file;

        this.Init();
    }

    private void Init()
    {
        if (this.file != null)
        {
            this.txtContent.Text = File.ReadAllText(this.file.FullName);
        }
        else if (this.text != null)
        {
            this.txtContent.Text = this.text;
        }

        var window = Application.Current.MainPage.Window;
        var width = window.Width-10;       

        this.txtContent.WidthRequest = width;       
    }

    private async void tbiCopy_Clicked(object sender, EventArgs e)
    {
        string text = this.txtContent.Text.Trim();

        if (text.Length > 0)
        {
            await Clipboard.SetTextAsync(text);

            MessageHelper.ShowToastMessage("�Ѹ��Ƶ����а�");
        }
    }

    private async void tbiSaveAs_Clicked(object sender, EventArgs e)
    {
        string text = this.txtContent.Text.Trim();

        if (text.Length > 0)
        {
            if (!(await PermissionHelper.CheckReadWritePermission(PermissionType.Write)))
            {
                return;
            }

            string fileName = this.file != null ? this.file.Name : "�½��ı��ĵ�.txt";

            using (MemoryStream ms = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(ms);

                sw.WriteLine(this.txtContent.Text);

                sw.Flush();

                var res = await FileSaver.SaveAsync(fileName, ms, new CancellationToken());

                if (res.IsSuccessful)
                {
                    await DisplayAlert("��Ϣ", "����ɹ�", "ȷ��");
                }
            }
        }
    }
}