namespace CantoneseLearning.Model
{
    public class V_Mandarin2Cantonese
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
        public string Example { get; set; }
        public string MandarinExample { get; set; }
        public string Pattern { get; set; }
        public bool IsStart { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsEnd { get; set; }
        public string PatternNotes { get; set; }
    }
}
