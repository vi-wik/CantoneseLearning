using System;

namespace CantoneseLearning.Model
{
    public class MediaAccessHistory
    {
        public int Id { get; set; }
        public int MediaId { get; set; }       
      
        public string PositionTime { get; set; }
        public DateTime LastAccessTime { get; set; }
    }
}
