using CantoneseLearning.Model;

namespace CantoneseLearning.Model
{
    public class Syllable: V_SyllableBase
    {
        public string Syllable_YP { get { return base.Syllable; } }
        public string Consonant_GP { get; set; }
        public string Vowel_GP { get; set; }
        public string Syllable_GP { get; set; }
        public string Syllable_M { get; set; }
        public int? Tone_M { get; set; }

        public bool HasSyllable { get { return !string.IsNullOrEmpty(this.Syllable_YP); } }
        public bool HasSyllable_M { get { return !string.IsNullOrEmpty(this.Syllable_M); } }

        public string SyllableFull
        {
            get
            {
                if (string.IsNullOrEmpty(this.Syllable_YP))
                {
                    return null;
                }

                return string.Concat(this.Syllable_YP, this.Tone == 0 ? "" : this.Tone.ToString() );
            }
        }

        public string SyllableFull_GP
        {
            get
            {
                if (string.IsNullOrEmpty(this.Syllable_GP))
                {
                    return null;
                }

                return string.Concat(this.Syllable_GP, this.Tone == 0 ? "" : this.Tone.ToString());
            }
        }

        public string Syllable_M_Full
        {
            get
            {
                if (string.IsNullOrEmpty(this.Syllable_M))
                {
                    return null;
                }

                return string.Concat(this.Syllable_M, this.Tone_M == 0 ? "" : this.Tone_M.ToString());
            }
        }       
    }
}
