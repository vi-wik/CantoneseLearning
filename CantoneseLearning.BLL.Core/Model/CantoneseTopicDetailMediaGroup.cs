using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.BLL.Core.Model
{
    public class CantoneseTopicDetailMediaGroup:List<V_CantoneseTopicDetailMedia>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CantoneseTopicDetailMediaGroup(string name, List<V_CantoneseTopicDetailMedia> medias) : base(medias)
        {
            Name = name;
        }
    }
}
