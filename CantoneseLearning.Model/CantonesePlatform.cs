namespace viwik.CantoneseLearning.Model
{
    public class CantonesePlatform
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
    }

    public enum CantonesePlatformType
    {
        bilibili = 1,
        ximalaya = 2
    }
}
