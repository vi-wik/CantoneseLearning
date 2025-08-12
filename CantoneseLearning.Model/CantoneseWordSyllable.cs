namespace CantoneseLearning.Model
{
    public class CantoneseWordSyllable
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int ConsonantId { get; set; }
        public int MedialId { get; set; }
        public int VowelId { get; set; }
        public int Tone { get; set; }      
        public bool IsDefault { get; set; }
        public int Priority { get; set; }
        public bool Hidden { get; set; }
        public bool Dialectal { get; set; }
        public bool Fixed { get; set; }
        public bool UseThisIfMultiple { get; set; } 
    }
}
