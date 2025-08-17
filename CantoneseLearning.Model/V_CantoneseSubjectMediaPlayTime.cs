namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseSubjectMediaPlayTime : CantoneseMediaPlayTime
    {
        public int Id { get; set; }
        public string SubjectMediaId { get; set; }
        public string SubjectId     { get; set; }
        public int MediaId { get; set; }
    }
}
