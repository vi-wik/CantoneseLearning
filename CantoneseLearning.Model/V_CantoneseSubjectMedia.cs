namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseSubjectMedia : V_CantoneseMedia
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int Priority { get; set; }
    }
}
