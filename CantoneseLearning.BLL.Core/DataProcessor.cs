using Microsoft.Data.Sqlite;
using System.Text.RegularExpressions;
using viwik.CantoneseLearning.BLL.Core.Helper;
using viwik.CantoneseLearning.BLL.Core.Model;
using viwik.CantoneseLearning.DataAccess;
using viwik.CantoneseLearning.Model;
using viwik.CantoneseLearning.Utility;

namespace viwik.CantoneseLearning.BLL.Core
{
    public class DataProcessor
    {
        internal static IEnumerable<MandarinVowel> MandarinVowels { get; private set; }     

        static DataProcessor()
        {
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

            Action<string, V_CantoneseWordSyllable, IEnumerable<V_MandarinWordSyllable>, int> addSyllable = (matchedExample, cantoneseSyllable, mandarinWordSyllables, originalWordIndex) =>
            {
                V_MandarinWordSyllable mandarinSyllable = null;

                int? mandarinSyllableId = cantoneseSyllable.MandarinWordSyllableId;
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
                V_CantoneseWordSyllable cantoneseSyllable = null;
                int? mandarinSyllableId = null;

                string matchedExample = GetMatchedExample(content, i, cantoneseWordSyllables, out cantoneseSyllableId);

                bool noRelation = false;

                if (string.IsNullOrEmpty(matchedExample))
                {
                    matchedExample = GetMatchedExample(content, i, mandarinWordSyllables, out mandarinSyllableId);

                    if (mandarinSyllableId.HasValue && !cantoneseSyllableId.HasValue)
                    {
                        cantoneseSyllableId = cantoneseWordSyllables.FirstOrDefault(item => item.MandarinWordSyllableId == mandarinSyllableId)?.Id;

                        if (!cantoneseSyllableId.HasValue)
                        {
                            int noRelationCount = cantoneseWordSyllables.Count(item => item.MandarinWordSyllableId == null);

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
                        V_MandarinWordSyllable mandarinSyllable = mandarinWordSyllables.FirstOrDefault(item => item.Id == mandarinSyllableId);

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

        private static Model.SyllableDisplay GetSyllableDisplay(V_CantoneseWordSyllable cantoneseSyllable, V_MandarinWordSyllable mandarinSyllable)
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

        private static string GetMandarinSyllableToneMarkDisplay(V_MandarinWordSyllable mandarinSyllable)
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

        public static async Task<IEnumerable<V_CantoneseConsonant_YPGP>> GetCantoneseConsonant_YPGPs()
        {
            return await DbObjectsFetcher.GetCantoneseConsonant_YPGPs();
        }

        public static async Task<IEnumerable<V_CantoneseVowel_YPGP>> GetCantoneseVowel_GPYPs()
        {
            var results = await DbObjectsFetcher.GetCantoneseVowel_YPGPs();

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

        public static async Task<IEnumerable<V_Mandarin2Cantonese>> GetVMandarin2Cantoneses(bool excludeIgnored = false)
        {
            return await DbObjectsFetcher.GetVMandarin2Cantoneses(excludeIgnored);
        }

        public static async Task<IEnumerable<V_CantoneseExample>> GetVCantoneseExamples()
        {
            return await DbObjectsFetcher.GetVCantoneseExamples();
        }

        public static async Task<IEnumerable<CantoneseSynonym>> GetCantoneseSynonyms()
        {
            return await DbObjectsFetcher.GetCantoneseSynonyms();
        }

        public static async Task<IEnumerable<CantoneseSentencePattern>> GetCantoneseSentencePatterns()
        {
            return await DbObjectsFetcher.GetCantoneseSentencePatterns();
        }

        public static async Task<TranslationResult> Translate(TranslateType translateType, string content, IEnumerable<string> reservedWords = null)
        {
            return await TranslateHelper.Translate(translateType, content, reservedWords);
        }

        public static async Task<bool> HasUserDataTable(string dbFilePath)
        {
            return await DbObjectsFetcher.HasUserDataTable(dbFilePath, "MediaAccessHistory");
        }

        public static async Task<UserData> GetUserData(string dbFilePath)
        {
            UserData userData = new UserData();

            userData.MediaFavoriteCategories = await DbObjectsFetcher.GetMediaFavoriteCategories(dbFilePath);
            userData.MediaFavorites = await DbObjectsFetcher.GetMediaFavorites(dbFilePath);
            userData.MediaAccessHistories = await DbObjectsFetcher.GetMediaAccessHistories(dbFilePath);         

            return userData;
        }

        public static async Task<CantoneseMediaExtraInfo> GetCantoneseMediaExtraInfo(int medialId)
        {
            return await DbObjectsFetcher.GetCantoneseMediaExtraInfo(medialId);
        }

        public static async Task<int> UpdateMediaSource(int id, string source)
        {
            return await DbExecuter.UpdateMediaSource(id, source);
        }

        public static async Task<int> RecordMediaAccessHistory(MediaAccessHistory mediaAccessHistory)
        {
            return await DbExecuter.RecordMediaAccessHistory(mediaAccessHistory);
        }

        public static async Task<MediaFavorite> GetMediaFavoriteByMediaId(int mediaId)
        {
            return await DbObjectsFetcher.GetMediaFavoriteByMediaId(mediaId);
        }

        public static async Task<IEnumerable<MediaFavoriteCategory>> GetMediaFavoriteCategories()
        {
            return await DbObjectsFetcher.GetMediaFavoriteCategories();
        }

        public static async Task<bool> AddMediaFavorite(int mediaId, int categoryId)
        {
            return await DbExecuter.AddMediaFavorite(mediaId, categoryId);
        }

        public static async Task<bool> DeleteMediaFavorite(int id)
        {
            return await DbExecuter.DeleteMediaFavorite(id);
        }

        public static async Task<CantoneseSubject> GetCantoneseSubject(int subjectId)
        {
            return await DbObjectsFetcher.GetCantoneseSubject(subjectId);
        }

        public static async Task<CantoneseSubject> GetCantoneseSubjectByEnName(string enName)
        {
            return await DbObjectsFetcher.GetCantoneseSubjectByEnName(enName);
        }

        public static async Task<IEnumerable<V_CantoneseSubjectMedia>> GetVCantoneseSubjectMedias(int subjectId)
        {
            return await DbObjectsFetcher.GetVCantoneseSubjectMedias(subjectId);
        }

        public static async Task<IEnumerable<CantoneseTopic>> GetCantoneseTopics(int subjectId)
        {
            return await DbObjectsFetcher.GetCantoneseTopics(subjectId);
        }

        public static async Task<IEnumerable<CantoneseTopicDetail>> GetCantoneseTopicDetails(int topicId, string keyword = null)
        {
            return await DbObjectsFetcher.GetCantoneseTopicDetails(topicId, keyword);
        }

        public static async Task<IEnumerable<V_CantoneseTopicDetailMedia>> GetVCantoneseTopicDetailMedias(int topicId, string keyword = null)
        {
            return await DbObjectsFetcher.GetVCantoneseTopicDetailMedias(topicId, keyword);
        }
      

        public static async Task<IEnumerable<V_CantoneseTopicDetailMediaPlayTime>> GetVCantoneseTopicDetailMediaPlayTimes(int topicDetailMediaId)
        {
            return await DbObjectsFetcher.GetVCantoneseTopicDetailMediaPlayTimes(topicDetailMediaId);
        }

        public static async Task<IEnumerable<V_CantoneseTopicDetailMediaPlayTime>> GetVCantoneseSubjectMediaPlayTimes(int subjectMediaId)
        {
            return await DbObjectsFetcher.GetVCantoneseSubjectMediaPlayTimes(subjectMediaId);
        }

        public static async Task<IEnumerable<V_CantoneseVocabularyMedia>> GetVVocabularyMedias(int vocabularyId)
        {
            return await DbObjectsFetcher.GetVVocabularyMedias(vocabularyId);
        }

        public static async Task<IEnumerable<V_CantoneseVocabularyMediaPlayTime>> GetVCantoneseVocabularyMediaPlayTimes(int vocabularyMediaId)
        {
            return await DbObjectsFetcher.GetVCantoneseVocabularyMediaPlayTimes(vocabularyMediaId);
        }

        public static async Task<IEnumerable<CantoneseConsonant>> GetCantoneseConsonants()
        {
            return await DbObjectsFetcher.GetCantoneseConsonants();
        }

        public static async Task<IEnumerable<CantoneseVowel>> GetCantoneseVowels()
        {
            return await DbObjectsFetcher.GetCantoneseVowels();
        }

        public static async Task<IEnumerable<V_CantoneseConsonantMedia>> GetVCantoneseConsonantMedias(int consonantId)
        {
            return await DbObjectsFetcher.GetVCantoneseConsonantMedias(consonantId);
        }

        public static async Task<IEnumerable<V_CantoneseVowelMedia>> GetVCantoneseVowelMedias(int vowelId)
        {
            return await DbObjectsFetcher.GetVCantoneseVowelMedias(vowelId);
        }

        public static async Task<IEnumerable<V_CantoneseConsonantMediaPlayTime>> GetVCantoneseConsonantMediaPlayTimes(int consonantMediaId)
        {
            return await DbObjectsFetcher.GetVCantoneseConsonantMediaPlayTimes(consonantMediaId);
        }

        public static async Task<IEnumerable<V_CantoneseVowelMediaPlayTime>> GetVCantoneseVowelMediaPlayTimes(int vowelMediaId)
        {
            return await DbObjectsFetcher.GetVCantoneseVowelMediaPlayTimes(vowelMediaId);
        }

        public static void ClearSqliteAllPools()
        {
            SqliteConnection.ClearAllPools();
        }

        public static async Task<int> ImportUserData(string sourceDbFilePath, string targetDbFilePath)
        {
            if (!File.Exists(targetDbFilePath))
            {
                return 0;
            }

            if (await DataProcessor.HasUserDataTable(targetDbFilePath))
            {
                UserData userData = await GetUserData(sourceDbFilePath);

                ClearSqliteAllPools();

                return await DbExecuter.KeepUserData(userData, targetDbFilePath);
            }

            return 0;
        }

        public static async Task<int> ClearMediaAccessHistories()
        {
            return await DbExecuter.ClearMediaAccessHistories();
        }

        public static async Task<int> DeleteMediaAccessHistoriesByMediaIds(List<int> mediaIds)
        {
            return await DbExecuter.DeleteMediaAccessHistoriesByMediaIds(mediaIds);
        }

        public static async Task<IEnumerable<V_MediaAccessHistory>> GetVMediaAccessHistories()
        {
            return await DbObjectsFetcher.GetVMediaAccessHistories();
        }

        public static async Task<IEnumerable<V_MediaFavorite>> GetVMediaFavorites()
        {
            return await DbObjectsFetcher.GetVMediaFavorites();
        }

        public static async Task<bool> AddMediaFavoriteCategory(MediaFavoriteCategory category)
        {
            return await DbExecuter.AddMediaFavoriteCategory(category);
        }

        public static async Task<bool> IsMediaFavoriteCategoryNameExisting(bool isAdd, string name, int? id)
        {
            return await DbObjectsFetcher.IsMediaFavoriteCategoryNameExisting(isAdd, name, id);
        }

        public static async Task<bool> RenameMediaFavoriteCategory(int id, string name)
        {
            return await DbExecuter.RenameMediaFavoriteCategory(id, name);
        }

        public static async Task<int> DeleteMediaFavoriteCategoriesByIds(List<int> ids)
        {
            return await DbExecuter.DeleteMediaFavoriteCategoriesByIds(ids);
        }

        public static async Task<bool> IsMediaFavoriteCategoryBeRefered(List<int> ids)
        {
            return await DbObjectsFetcher.IsMediaFavoriteCategoryBeRefering(ids);
        }

        public static async Task<IEnumerable<CantoneseEqualsMandarin>> GetCantoneseEqualsMandarins()
        {
            return await DbObjectsFetcher.GetCantoneseEqualsMandarins();
        }
    }
}