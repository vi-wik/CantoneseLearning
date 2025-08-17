namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseVocabularyMediaPlayTime : CantoneseMediaPlayTime
    {
        public int Id { get; set; }
        public string VocabularyMediaId { get; set; }
        public string VocabularyId { get; set; }
        public int MediaId { get; set; }
    }
}
