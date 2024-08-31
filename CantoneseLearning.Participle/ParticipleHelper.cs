using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using System.Collections.Generic;
using System.Linq;

namespace CantoneseLearning.Participle
{
    public class ParticipleHelper
    {
        public static List<Pair> Cut(string content)
        {
            var jbs = new PosSegmenter();      
            
            return jbs.Cut(content).ToList();            
        }
    }
}
