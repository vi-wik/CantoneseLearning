namespace CantoneseLearning.Model
{
    public class MandarinSyllable
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Consonant { get; set; }
        public string Medial { get; set; }
        public string Vowel { get; set; }
        public int Tone { get; set; }
        public string Syllable { get; set; }
        public string SyllableFull
        {
            get
            {
                return string.Concat(this.Syllable, this.Tone);
            }
        }

        public bool IsDefault { get; set; }
        public bool Hidden { get; set; }
    }
}
