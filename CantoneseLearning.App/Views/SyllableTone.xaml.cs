using viwik.CantoneseLearning.Model;
using Microsoft.Maui.Controls.Shapes;
using viwik.CantoneseLearning.App.Helper;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.BLL.MAUI.Manager;

namespace viwik.CantoneseLearning.App.Views;

public partial class SyllableTone : ContentPage
{
    private SettingInfo setting;

    public SyllableTone()
    {
        InitializeComponent();

        this.setting = SettingManager.GetSetting();

        this.Init();
    }

    private void Init()
    {
        bool isGP = this.setting.PinYinMode == PinYinMode.GP;

        this.lblSample1.Text = isGP ? "sam1" : "saam1";
        this.lblSample2.Text = isGP ? "geo2" : "gau2";
        this.lblSample3.Text = isGP ? "s¨¦i3" : "sei3";
        this.lblSample4.Text = isGP ? "ling4" : "ling4";
        this.lblSample5.Text = isGP ? "ng5" : "ng5";
        this.lblSample6.Text = isGP ? "yi6" : "ji6";
        this.lblSample7.Text = isGP ? "cad7" : "cat7";
        this.lblSample8.Text = isGP ? "bad8" : "baat8";
        this.lblSample9.Text = isGP ? "lug9" : "luk9";
    }

    private void ToneDemoButton_Clicked(object sender, EventArgs e)
    {
        var btn = (sender as Button);

        if (btn != null)
        {
            string syllable = btn.CommandParameter?.ToString();

            if (string.IsNullOrEmpty(syllable))
            {
                return;
            }

            AudioPlayHelper.PlayPackageFile($"Audios/{LanguageType.Cantonese.ToString()}/{syllable}.mp3");
        }
    }
}