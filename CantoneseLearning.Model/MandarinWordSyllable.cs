namespace CantoneseLearning.Model
{
    public class MandarinWordSyllable
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int ConsonantId { get; set; }
        public int MedialId { get; set; }
        public int VowelId { get; set; }
        public int Tone { get; set; }
        public bool IsDefault { get; set; }
        public bool Hidden { get; set; }
    }
}
