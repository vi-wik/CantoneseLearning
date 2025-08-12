namespace CantoneseLearning.Model
{
    public class V_CantoneseWordSyllable : V_SyllableBase
    {
        public string Consonant_GP { get; set; }
        public string Vowel_GP { get; set; }
        public string Syllable_GP { get; set; }
        public bool UseThisIfMultiple { get; set; }
        public bool Inclusive { get; set; }
        public bool Fixed { get; set; }
        public int? MandarinWordSyllableId { get; set; }
        public bool? HasSpecial { get; set; }
        public bool? HasAlt { get; set; }
    }
}
