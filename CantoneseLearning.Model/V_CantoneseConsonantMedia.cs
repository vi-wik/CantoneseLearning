namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseConsonantMedia : V_CantoneseMedia
    {
        public int Id { get; set; }
        public int ConsonantId { get; set; }
        public string Consonant { get; set; }
        public int Priority { get; set; }
    }
}
