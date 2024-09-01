using CantoneseLearning.Business.Model;
using CantoneseLearning.DataAccess;
using CantoneseLearning.Model;
using CantoneseLearning.Participle;
using CantoneseLearning.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;

namespace CantoneseLearning.Business
{
    public class DataProcessor
    {
        internal static IEnumerable<MandarinVowel> MandarinVowels { get; private set; }
        internal static IEnumerable<V_Mandarin2Cantonese> Mandarin2Cantoneses { get; private set; }

        static DataProcessor()
        {
            DbUtitlity.DataFilePath = DataFileManager.DataFilePath;

            Init();
        }

        static async void Init()
        {
            if (MandarinVowels == null)
            {
                MandarinVowels = await GetMandarinVowels();
            }
        }

        public static async Task<IEnumerable<SyllableDisplay>> GetSyllables(string content, bool onlyFetchFirstIfMultiple = false)
        {
            var words = content.Select(item => item.ToString()).ToArray();
            int wordsCount = words.Length;
            bool isMutiple = wordsCount > 1;

            List<SyllableDisplay> syllables = new List<SyllableDisplay>();

            Action<string, V_CantoneseSyllable, IEnumerable<V_MandarinSyllable>, int> addSyllable = (matchedExample, cantoneseSyllable, mandarinWordSyllables, originalWordIndex) =>
            {
                V_MandarinSyllable mandarinSyllable = null;

                int? mandarinSyllableId = cantoneseSyllable.MandarinSyllableId;
                bool? hasSpecial = cantoneseSyllable.HasSpecial;

                if (hasSpecial.HasValue && !string.IsNullOrEmpty(matchedExample))
                {
                    var dictExamples = GetExamplesDictionary(mandarinWordSyllables);

                    foreach (var dictExample in dictExamples)
                    {
                        if (dictExample.Value.Contains(matchedExample))
                        {
                            mandarinSyllableId = dictExample.Key;
                            break;
                        }
                    }
                }

                if (mandarinSyllableId.HasValue)
                {
                    mandarinSyllable = mandarinWordSyllables.FirstOrDefault(item => item.Id == mandarinSyllableId.Value);
                }

                Model.SyllableDisplay syllable = GetSyllableDisplay(cantoneseSyllable, mandarinSyllable);
                syllable.OriginalWordIndex = originalWordIndex;

                syllables.Add(syllable);
            };

            var result = await DbObjectsFetcher.GetSyllables(content);
            var cantoneseSyllables = result.CantoneseSyllables;
            var mandarinSyllables = result.MandarinSyllables;

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                var cantoneseWordSyllables = cantoneseSyllables.Where(item => item.Word == word);
                var mandarinWordSyllables = mandarinSyllables.Where(item => item.Word == word);

                int cantoneseSyllableCount = cantoneseWordSyllables.Count();

                int? cantoneseSyllableId = null;
                V_CantoneseSyllable cantoneseSyllable = null;
                int? mandarinSyllableId = null;

                string matchedExample = GetMatchedExample(content, i, cantoneseWordSyllables, out cantoneseSyllableId);

                bool noRelation = false;

                if (string.IsNullOrEmpty(matchedExample))
                {
                    matchedExample = GetMatchedExample(content, i, mandarinWordSyllables, out mandarinSyllableId);

                    if (mandarinSyllableId.HasValue && !cantoneseSyllableId.HasValue)
                    {
                        cantoneseSyllableId = cantoneseWordSyllables.FirstOrDefault(item => item.MandarinSyllableId == mandarinSyllableId)?.Id;

                        if (!cantoneseSyllableId.HasValue)
                        {
                            int noRelationCount = cantoneseWordSyllables.Count(item => item.MandarinSyllableId == null);

                            if (noRelationCount == 0)
                            {
                                if (!cantoneseWordSyllables.Any(item => item.UseThisIfMultiple))
                                {
                                    noRelation = true;
                                }
                            }
                        }
                    }
                }

                if (cantoneseSyllableCount == 1)
                {
                    var firstCantoneseSyllable = cantoneseWordSyllables.First();

                    if (firstCantoneseSyllable.Inclusive)
                    {
                        cantoneseSyllableId = firstCantoneseSyllable.Id;
                        noRelation = false;
                    }
                }

                if (!noRelation)
                {
                    if (!cantoneseSyllableId.HasValue)
                    {
                        if (cantoneseSyllableCount == 1)
                        {
                            cantoneseSyllableId = cantoneseWordSyllables.FirstOrDefault()?.Id;
                        }
                    }

                    if (cantoneseSyllableId.HasValue)
                    {
                        cantoneseSyllable = cantoneseWordSyllables.FirstOrDefault(item => item.Id == cantoneseSyllableId);
                    }
                    else
                    {
                        if (isMutiple && cantoneseWordSyllables.Any(item => item.UseThisIfMultiple))
                        {
                            cantoneseSyllable = cantoneseWordSyllables.FirstOrDefault(item => item.UseThisIfMultiple);
                        }
                    }

                    if (cantoneseSyllable == null)
                    {
                        var notFixedSyllables = cantoneseWordSyllables.Where(item => item.Fixed == false);
                        var defaultSyllable = cantoneseWordSyllables.FirstOrDefault(item => item.IsDefault == true);

                        if (isMutiple && notFixedSyllables.Count() == 1)
                        {
                            cantoneseSyllable = notFixedSyllables.FirstOrDefault();
                        }
                        else if (isMutiple && defaultSyllable != null && string.IsNullOrEmpty(defaultSyllable.Examples))
                        {
                            cantoneseSyllable = defaultSyllable;
                        }
                        else
                        {
                            foreach (var cs in cantoneseWordSyllables)
                            {
                                addSyllable(matchedExample, cs, mandarinWordSyllables, i);

                                if (onlyFetchFirstIfMultiple)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (cantoneseSyllable != null)
                    {
                        addSyllable(matchedExample, cantoneseSyllable, mandarinWordSyllables, i);
                    }
                }
                else
                {
                    if (mandarinSyllableId.HasValue)
                    {
                        V_MandarinSyllable mandarinSyllable = mandarinWordSyllables.FirstOrDefault(item => item.Id == mandarinSyllableId);

                        Model.SyllableDisplay syllable = GetSyllableDisplay(null, mandarinSyllable);
                        syllable.OriginalWordIndex = i;

                        syllables.Add(syllable);
                    }
                }
            }

            return syllables;
        }

        private static string GetMatchedExample(string content, int wordIndex,
                              IEnumerable<V_SyllableBase> wordSyllables, out int? syllableId)
        {
            syllableId = null;
            string matchedEample = null;

            var dictExamples = GetExamplesDictionary(wordSyllables.OrderBy(item => item.Priority));

            //完全匹配
            foreach (var dictExample in dictExamples)
            {
                var examples = dictExample.Value;

                foreach (string example in examples)
                {
                    if (content.Trim() == example)
                    {
                        syllableId = dictExample.Key;
                        matchedEample = example;
                        break;
                    }
                }
            }

            //模糊匹配
            if (!syllableId.HasValue)
            {
                foreach (var dictExample in dictExamples)
                {
                    var examples = dictExample.Value;

                    foreach (string example in examples)
                    {
                        Regex regex = new Regex(example);

                        var matches = regex.Matches(content);

                        foreach (Match match in matches)
                        {
                            int startIndex = match.Index;
                            int stopIndex = startIndex + match.Length;

                            if (wordIndex >= startIndex && wordIndex <= stopIndex)
                            {
                                syllableId = dictExample.Key;
                                matchedEample = match.Value;
                                break;
                            }
                        }

                        if (syllableId.HasValue)
                        {
                            break;
                        }
                    }
                }
            }

            return matchedEample;
        }

        private static Model.SyllableDisplay GetSyllableDisplay(V_CantoneseSyllable cantoneseSyllable, V_MandarinSyllable mandarinSyllable)
        {
            Model.SyllableDisplay syllableDisplay = new Model.SyllableDisplay()
            {
                Id = cantoneseSyllable?.Id ?? 0,
                Word = cantoneseSyllable?.Word ?? mandarinSyllable?.Word,
                Consonant = cantoneseSyllable?.Consonant,
                Medial = cantoneseSyllable?.Medial,
                Vowel = cantoneseSyllable?.Vowel,
                Tone = cantoneseSyllable == null ? 0 : cantoneseSyllable.Tone,
                Consonant_GP = cantoneseSyllable?.Consonant_GP,
                Vowel_GP = cantoneseSyllable?.Vowel_GP,
                Syllable = cantoneseSyllable?.Syllable,
                Syllable_GP = cantoneseSyllable?.Syllable_GP,
                Syllable_M = mandarinSyllable?.Syllable,
                Tone_M = mandarinSyllable?.Tone,
                Priority = cantoneseSyllable?.Priority ?? 0,
                IsDefault = cantoneseSyllable?.IsDefault ?? false,
                Syllable_M_Full_ToneMark = GetMandarinSyllableToneMarkDisplay(mandarinSyllable),
                HasAlt = cantoneseSyllable?.HasAlt ?? false
            };

            return syllableDisplay;
        }

        private static string GetMandarinSyllableToneMarkDisplay(V_MandarinSyllable mandarinSyllable)
        {
            if (mandarinSyllable == null)
            {
                return string.Empty;
            }

            return GetMandarinSyllableToneMarkDisplay(mandarinSyllable.Consonant, mandarinSyllable.Medial, mandarinSyllable.Vowel, mandarinSyllable.Tone);
        }

        private static string GetMandarinSyllableToneMarkDisplay(string consonant, string medial, string vowel, int tone)
        {
            string prefix = string.Concat(consonant ?? "", medial ?? "");
            var toneMarkRecord = MandarinVowels.Where(item => item.Vowel == vowel).FirstOrDefault();
            string toneMark = string.Empty;

            switch (tone)
            {
                case 1:
                    toneMark = toneMarkRecord?.ToneMark1;
                    break;
                case 2:
                    toneMark = toneMarkRecord?.ToneMark2;
                    break;
                case 3:
                    toneMark = toneMarkRecord?.ToneMark3;
                    break;
                case 4:
                    toneMark = toneMarkRecord?.ToneMark4;
                    break;
                default:
                    toneMark = vowel ?? "";
                    break;
            }

            return string.Concat(prefix, toneMark ?? "");
        }

        private static Dictionary<int, List<string>> GetExamplesDictionary(IEnumerable<V_SyllableBase> syllables)
        {
            Dictionary<int, List<string>> examples = new Dictionary<int, List<string>>();

            foreach (var syllable in syllables)
            {
                if (!string.IsNullOrEmpty(syllable.Examples))
                {
                    examples.Add(syllable.Id, GetExampleList(syllable.Examples));
                }
            }

            return examples;
        }

        private static List<string> GetExampleList(string examples)
        {
            if (!string.IsNullOrEmpty(examples))
            {
                return examples.Split(new char[] { ',', '，' }).Select(item => item.Trim()).ToList();
            }

            return new List<string>();
        }

        public static async Task<List<SyllableDisplay>> GetSyllableRelations(SyllableType type, string syllable)
        {
            var syllableRelations = await DbObjectsFetcher.GetSyllableRelations(type, syllable);

            List<SyllableDisplay> syllableDisplays = new List<SyllableDisplay>();

            foreach (var sr in syllableRelations)
            {
                SyllableDisplay sd = ObjectHelper.CloneObject<SyllableDisplay>(sr);
                sd.Syllable_M_Full_ToneMark = GetMandarinSyllableToneMarkDisplay(sr.Consonant_M, sr.Medial_M, sr.Vowel_M, sr.Tone_M);

                syllableDisplays.Add(sd);
            }

            return syllableDisplays;
        }

        public static async Task<IEnumerable<MandarinVowel>> GetMandarinVowels()
        {
            return await DbObjectsFetcher.GetMandarinVowels();
        }

        public static async Task<IEnumerable<CantoneseConsonant_GPYP>> GetCantoneseConsonant_GPYPs()
        {
            return await DbObjectsFetcher.GetCantoneseConsonant_GPYPs();
        }

        public static async Task<IEnumerable<CantoneseVowel_GPYP>> GetCantoneseVowel_GPYPs()
        {
            var results = await DbObjectsFetcher.GetCantoneseVowel_GPYPs();

            foreach (var result in results)
            {
                result.Vowel_GP = StringHelper.GetDisplayVowel_GP(result.Vowel_GP);
            }

            return results;
        }

        public static async Task<IEnumerable<SyllableDisplay>> GetCantoneseSyllableAlts(int syllableId)
        {
            var syllableAlts = await DbObjectsFetcher.GetCantoneseSyllableAlts(syllableId);

            List<SyllableDisplay> syllables = new List<SyllableDisplay>();

            foreach (var alt in syllableAlts)
            {
                SyllableDisplay syllable = new SyllableDisplay()
                {
                    Word = alt.Word,
                    Consonant = alt.Consonant,
                    Consonant_GP = alt.Consonant_GP,
                    Medial = alt.Medial,
                    Vowel = alt.Vowel,
                    Vowel_GP = alt.Vowel_GP,
                    Tone = alt.Tone,
                    Syllable = alt.Syllable,
                    Syllable_GP = alt.Syllable_GP,
                    Priority = alt.Priority,
                    IsOral = alt.IsOral
                };

                syllables.Add(syllable);
            }

            return syllables;
        }

        public static async Task<IEnumerable<Mandarin2Cantonese>> GetMandarin2Cantoneses()
        {
            return await DbObjectsFetcher.GetMandarin2Cantoneses();
        }

        public static async Task<IEnumerable<V_Mandarin2Cantonese>> GetVMandarin2Cantoneses()
        {
            var results = await DbObjectsFetcher.GetVMandarin2Cantoneses();

            return results;
        }

        public static async Task<TranslationResult> Translate(string content)
        {
            TranslationResult result = new TranslationResult();

            if (Mandarin2Cantoneses == null)
            {
                Mandarin2Cantoneses = await GetVMandarin2Cantoneses();
            }

            char[] splitors = [',', '，'];

            Func<string, V_Mandarin2Cantonese> find = (word) =>
            {
                V_Mandarin2Cantonese target = null;

                foreach (var mc in Mandarin2Cantoneses)
                {
                    if (mc.Mandarin == word)
                    {
                        target = mc;
                    }
                    else if (mc.Mandarin.Split(splitors).Contains(word))
                    {
                        target = mc;
                    }

                    if (target != null)
                    {
                        break;
                    }
                }

                return target;
            };

            Action<V_Mandarin2Cantonese> setResult = (res) =>
            {
                var cantoneses = res.Cantonese.Split(splitors);

                result.Contents.AddRange(cantoneses);

                if (res.Example != null)
                {
                    result.Examples.Add(res.Example);
                }

                result.PatternNotes = res.PatternNotes;
            };

            V_Mandarin2Cantonese target = find(content);

            if (target != null)
            {
                setResult(target);
            }
            else
            {
                if (target == null)
                {
                    var cuts = ParticipleHelper.Cut(content);

                    List<CutPiece> pieces = new List<CutPiece>();

                    bool previousIsOrderIncreased = false;

                    for (int i = 0; i < cuts.Count; i++)
                    {
                        var cut = cuts[i];

                        CutPiece piece = new CutPiece() { Word = cut.Word, Flag = cut.Flag, Order = i };

                        if (previousIsOrderIncreased)
                        {
                            piece.Order--;
                            previousIsOrderIncreased = false;
                        }

                        target = find(cut.Word);

                        if (target != null)
                        {
                            piece.Word = target.Cantonese.Split(splitors).FirstOrDefault();
                        }
                        else
                        {
                            foreach (var mc in Mandarin2Cantoneses)
                            {
                                var words = mc.Mandarin.Split(splitors);

                                foreach (var word in words)
                                {
                                    if (!string.IsNullOrEmpty(word))
                                    {
                                        Regex regex = new Regex(word);

                                        var matches = regex.Matches(content);

                                        foreach (Match match in matches)
                                        {
                                            piece.Word = piece.Word.Replace(match.Value, mc.Cantonese.Split(splitors).FirstOrDefault());
                                        }
                                    }
                                }
                            }
                        }

                        pieces.Add(piece);

                        if (target != null && target.PatternId == 2) //先
                        {
                            if (i + 1 < cuts.Count && cuts[i + 1].Flag.StartsWith("v")) //动词
                            {
                                piece.Order++;
                                previousIsOrderIncreased = true;
                            }
                        }
                    }

                    result.Contents.Add(string.Join("", pieces.OrderBy(i => i.Order).Select(item => item.Word)));
                }
            }

            return result;
        }
    }
}