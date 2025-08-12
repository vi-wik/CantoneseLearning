namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseSyllableAlt
    {
        public int Id { get; set; }
        public int SyllableId { get; set; }
        public string Word { get; set; }
        public string Consonant { get; set; }
        public string Consonant_GP { get; set; }
        public string Medial { get; set; }
        public string Vowel { get; set; }
        public string Vowel_GP { get; set; }
        public int Tone { get; set; }
        public string Syllable { get; set; }    
        public string Syllable_GP { get; set; }
        public int Priority { get; set; }    
        public bool IsOral { get; set; }
    }
}
