namespace CantoneseLearning.Business.Model
{
    public class TranslationResult
    {
        public List<string> Contents { get; set; } = new List<string>();
        public List<string> Examples { get; set; } = new List<string>();
        public string PatternNotes { get; set; }
    }
}
