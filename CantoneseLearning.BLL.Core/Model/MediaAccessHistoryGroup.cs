using System.Collections.ObjectModel;

namespace viwik.CantoneseLearning.BLL.Core.Model
{
    public class MediaAccessHistoryGroup : ObservableCollection<CantoneseMediaForEditing>
    {
        public string Name { get; set; }
    

        public MediaAccessHistoryGroup(string name, ObservableCollection<CantoneseMediaForEditing> medias) : base(medias)
        {
            this.Name = name;          
        }
    }
}
