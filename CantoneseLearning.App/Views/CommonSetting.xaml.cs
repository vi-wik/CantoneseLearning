using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.BLL.MAUI.Manager;

namespace viwik.CantoneseLearning.App.Views;

public partial class CommonSetting : ContentPage
{
    private SettingInfo setting;

    public CommonSetting()
    {
        InitializeComponent();

        this.setting = SettingManager.GetSetting();      

        PinYinMode pinyinMode = setting.PinYinMode;

        if (pinyinMode == PinYinMode.GP)
        {
            this.rbPinYinMode_GP.IsChecked = true;
        }
        else
        {
            this.rbPinYinMode_YP.IsChecked = true;
        }
    }

    private void Save()
    {
        SettingManager.SaveSetting(this.setting);
    }

    private void chkEnableLog_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        this.setting.EnableLog = this.chkEnableLog.IsChecked;

        this.Save();
    }

    private void rbPinYinMode_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (this.setting == null)
        {
            return;
        }

        RadioButton radioButton = sender as RadioButton;

        if (radioButton.IsChecked)
        {
            this.setting.PinYinMode =(PinYinMode) Convert.ToInt32(radioButton.Value);

            this.Save();
        }
    }      
}