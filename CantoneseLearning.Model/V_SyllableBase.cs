namespace CantoneseLearning.Model
{
    public class V_SyllableBase
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Consonant { get; set; }
        public string Medial { get; set; }
        public string Vowel { get; set; }
        public int Tone { get; set; }
        public string Syllable { get; set; }
        public bool IsDefault { get; set; }
        public int Priority { get; set; }
        public bool Hidden { get; set; }
        public string Examples { get; set; }
        public string Description { get; set; }
    }
}
