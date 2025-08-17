namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseVowelMediaPlayTime: CantoneseMediaPlayTime
    {
        public int Id { get; set; }
        public int VowelMediaId { get; set; }
        public int WordId { get; set; }
        public int MediaId { get; set; }
    }
}
