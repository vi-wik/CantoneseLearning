namespace viwik.CantoneseLearning.BLL.Core.Model
{
    public class SettingInfo
    {
        public bool EnableLog { get; set; }
        public PinYinMode PinYinMode { get; set; } = PinYinMode.GP;
    }

    public enum PinYinMode
    {
        /// <summary>
        /// 广拼
        /// </summary>
        GP=1,
        /// <summary>
        /// 粤拼
        /// </summary>
        YP=2
    }
}
