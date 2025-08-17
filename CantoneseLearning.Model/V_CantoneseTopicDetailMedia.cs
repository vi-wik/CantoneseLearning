namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseTopicDetailMedia : V_CantoneseMedia
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int TopicDetailId { get; set; }
        public string TopicDetailName { get; set; }
        public int DetailPriority { get; set; }
        public int Priority { get; set; }
    }
}
