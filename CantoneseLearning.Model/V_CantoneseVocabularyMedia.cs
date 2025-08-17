namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseVocabularyMedia : V_CantoneseMedia
    {
        public int Id { get; set; }
        public int Vocabulary { get; set; }
        public string Cantonese { get; set; }
        public int Priority { get; set; }
    }
}
