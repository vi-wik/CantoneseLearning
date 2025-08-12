using System.Net.NetworkInformation;

namespace viwik.CantoneseLearning.Utility
{
    public class StringHelper
    {
        public static string[] SpecialVowels =  { "i", "ik", "im", "in", "ing", "ip", "it", "iu", "yu", "yut", "yun" };

        
        public static string GetDisplayVowel_GP(string syllable)
        {
            if (!string.IsNullOrEmpty(syllable))
            {
                return syllable.Replace("v", "ü");
            }

            return syllable;
        }      

        public static bool IsChineseChar(char ch)
        {
            if (ch >= 0x4E00 && ch <= 0x9FFF)
            {
                return true;
            }

            return false;
        }
    }
}
