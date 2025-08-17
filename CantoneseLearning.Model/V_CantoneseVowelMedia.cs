namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseVowelMedia : V_CantoneseMedia
    {
        public int Id { get; set; }
        public int VowelId { get; set; }
        public string Vowel { get; set; }
        public int Priority { get; set; }
        public string SubcategoryName { get; set; }
        public string SubcategoryDescription { get; set; }
        public int SubcategoryPriority { get; set; }
    }
}
