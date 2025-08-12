namespace viwik.CantoneseLearning.Model
{
    public class Mandarin2Cantonese
    {
        public int Id { get; set; }
        public string Mandarin { get; set; }
        public string Cantonese { get; set; }
        public string MandarinSynonym { get; set; }
        public string CantoneseSynonym { get; set; }
        public string Exclusion { get; set; }
        public string MandarinRegex { get; set; }
        public string CantoneseRegex { get; set; }
        public int PatternId { get; set; }
    }
}
