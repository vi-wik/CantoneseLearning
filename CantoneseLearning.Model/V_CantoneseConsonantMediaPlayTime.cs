namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseConsonantMediaPlayTime: CantoneseMediaPlayTime
    {
        public int Id { get; set; }
        public int ConsonantMediaId { get; set; }
        public int WordId { get; set; }
        public int MediaId { get; set; }
    }
}
