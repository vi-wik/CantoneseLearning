using System;

namespace viwik.CantoneseLearning.Model
{
    public class V_MediaFavorite : V_CantoneseMedia
    {
        public int Id { get; set; }      
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
