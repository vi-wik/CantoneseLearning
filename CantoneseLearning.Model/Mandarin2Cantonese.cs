namespace CantoneseLearning.Model
{
    public class Mandarin2Cantonese
    {
        public int Id { get; set; }
        public string Mandarin { get; set; }
        public string Cantonese { get; set; }
        public bool Reversed { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool IsEndOfMandarin { get; set; }
        public bool IsEndOfCantonese { get; set; }
        public int PatternId { get; set; }
    }
}
