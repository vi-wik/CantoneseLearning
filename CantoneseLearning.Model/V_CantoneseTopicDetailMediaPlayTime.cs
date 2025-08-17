namespace viwik.CantoneseLearning.Model
{
    public class V_CantoneseTopicDetailMediaPlayTime : CantoneseMediaPlayTime
    {
        public int Id { get; set; }
        public string TopicDetailMediaId { get; set; }
        public string TopicDetailId     { get; set; }
        public int MediaId { get; set; }
    }
}
