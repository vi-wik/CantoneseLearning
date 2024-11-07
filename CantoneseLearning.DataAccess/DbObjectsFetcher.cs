using CantoneseLearning.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CantoneseLearning.DataAccess
{
    public class DbObjectsFetcher
    {
        public static async Task<IEnumerable<MandarinWord>> GetMandarinWords()
        {
            string sql = "select * from MandarinWord";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<MandarinWord>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseWord>> GetCantoneseWords()
        {
            string sql = "select * from CantoneseWord";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseWord>(sql));
            }
        }

        public static async Task<(IEnumerable<V_CantoneseSyllable> CantoneseSyllables, IEnumerable<V_MandarinSyllable> MandarinSyllables)> GetSyllables(string content)
        {
            var words = content.Select(item => item.ToString()).ToArray();      
            string wordsIn = string.Join(',', words.Select(item => $"'{DbUtitlity.GetSafeValue(item)}'"));

            string cantoneseSql = $"select * from v_CantoneseSyllables where Word in({wordsIn}) and Hidden=0";
            string mandarinSql = $"select * from v_MandarinSyllables where Word in({wordsIn})";          

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                var cantoneseSyllables = (await connection.QueryAsync<V_CantoneseSyllable>(cantoneseSql));
                var mandarinSyllables = (await connection.QueryAsync<V_MandarinSyllable>(mandarinSql));

                return (cantoneseSyllables, mandarinSyllables);
            }           
        }

        public static async Task<IEnumerable<V_SyllableRelation>> GetSyllableRelations(SyllableType type, string syllable)
        {
            string syllableType = "";

            switch (type)
            {
                case SyllableType.Mandarin:
                    syllableType = "Syllable_M";
                    break;
                case SyllableType.Cantonese_YP:
                    syllableType = "Syllable";
                    break;
                case SyllableType.Cantonese_GP:
                    syllableType = "Syllable_GP";
                    break;
                default:
                    syllableType = "Syllable_M";
                    break;
            }

            string sql = $"select * from v_SyllableRelations where {syllableType} = @Syllable order by Consonant,Vowel";

            Dictionary<string, object> para = new Dictionary<string, object>();

            para.Add("@Syllable", DbUtitlity.GetParameterValue(syllable));

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_SyllableRelation>(sql, para));
            }
        }

        public static async Task<IEnumerable<MandarinVowel>> GetMandarinVowels()
        {
            string sql = "select * from MandarinVowel";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<MandarinVowel>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseConsonant_GPYP>> GetCantoneseConsonant_GPYPs()
        {
            string sql = "select * from CantoneseConsonant_GPYP order by Consonant_YP";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseConsonant_GPYP>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseVowel_GPYP>> GetCantoneseVowel_GPYPs()
        {
            string sql = "select * from CantoneseVowel_GPYP order by Vowel_YP";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseVowel_GPYP>(sql));
            }
        }

        public static async Task<IEnumerable<V_CantoneseSyllableAlt>> GetCantoneseSyllableAlts(int syllableId)
        {
            string sql = "select * from v_CantoneseSyllableAlts where SyllableId=@SyllableId";

            Dictionary<string, object> para = new Dictionary<string, object>();

            para.Add("@SyllableId", DbUtitlity.GetParameterValue(syllableId.ToString()));

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_CantoneseSyllableAlt>(sql, para));
            }
        }

        public static async Task<IEnumerable<Mandarin2Cantonese>> GetMandarin2Cantoneses()
        {
            string sql = "select * from Mandarin2Cantonese order by Id";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<Mandarin2Cantonese>(sql));
            }
        }

        public static async Task<IEnumerable<V_Mandarin2Cantonese>> GetVMandarin2Cantoneses()
        {
            string sql = "select * from v_Mandarin2Cantonese";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_Mandarin2Cantonese>(sql));
            }
        }
        

        public static async Task<IEnumerable<V_CantoneseExample>> GetVCantoneseExamples()
        {
            string sql = "select * from v_CantoneseExamples";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<V_CantoneseExample>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseSynonym>> GetCantoneseSynonyms()
        {
            string sql = "select * from CantoneseSynonym";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseSynonym>(sql));
            }
        }

        public static async Task<IEnumerable<CantoneseSentencePattern>> GetCantoneseSentencePatterns()
        {
            string sql = "select * from CantoneseSentencePattern";

            using (var connection = DbUtitlity.CreateDbConnection())
            {
                return (await connection.QueryAsync<CantoneseSentencePattern>(sql));
            }
        }
    }
}
