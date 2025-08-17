using viwik.CantoneseLearning.Utility;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.BLL.Core.Model
{
    public class SyllableDisplay: Syllable
    {
        public int OriginalWordIndex { get; set; }
        public bool HasAlt { get; set; }
        public bool IsOral { get; set; }      

        public string Syllable_Display
        {
            get
            {
                return this.Syllable_YP;
            }
        }

        public string Syllable_GP_Display
        {
            get
            {
                return this.GetDisplayCantoneseSyllable_GP();
            }
        }
      
        public string Syllable_M_Full_ToneMark { get; set; }

        public string SyllableFull_Display
        {
            get
            {
                if (string.IsNullOrEmpty(this.Syllable_Display))
                {
                    return null;
                }

                return string.Concat(this.Syllable_Display, this.Tone == 0 ? "" : this.Tone.ToString());
            }
        }

        public string SyllableFull_GP_Display
        {
            get
            {
                if (string.IsNullOrEmpty(this.Syllable_GP_Display))
                {
                    return null;
                }

                return string.Concat(this.Syllable_GP_Display, this.Tone == 0 ? "" : this.Tone.ToString());
            }
        }

        public string Dynamic_Display { get; set; }
        

        public string GetDisplayCantoneseSyllable_GP()
        {
            if (this == null)
            {
                return string.Empty;
            }

            string consonant = this.Consonant;
            string vowel = this.Vowel;
            string consonantGP = this.Consonant_GP;
            string vowelGP = this.Vowel_GP;

            if (vowel != null)
            {
                if (StringHelper.SpecialVowels.Contains(vowel))
                {
                    switch (consonant)
                    {
                        case "c":
                            consonantGP = "q";
                            break;
                        case "s":
                            consonantGP = "x";
                            break;
                        case "z":
                            consonantGP = "j";
                            break;
                    }

                    if (vowel == "yu" || vowel == "yut" || vowel == "yun")
                    {
                        if (consonant == "z")
                        {
                            consonantGP = "j";
                        }

                        switch (consonant)
                        {
                            case "c":
                            case "j":
                            case "s":
                            case "z":
                                vowelGP = vowelGP.Replace("v", "u");
                                break;
                        }
                    }
                }                         
            }

            string syllableGP = string.Concat(consonantGP?? "", this.Medial?? "", StringHelper.GetDisplayVowel_GP(vowelGP));

            return syllableGP;
        }
    }
}
