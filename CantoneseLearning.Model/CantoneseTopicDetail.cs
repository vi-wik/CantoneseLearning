using System.Collections.Generic;

namespace viwik.CantoneseLearning.Model
{
    public class CantoneseTopicDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public string Description { get; set; }
        public int TopicId { get; set; }
        public int Priority { get; set; }
    }
}
