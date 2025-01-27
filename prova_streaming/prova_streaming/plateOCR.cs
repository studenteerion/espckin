using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_streaming
{
    public class plateOCR
    {
        public List<string> Text { get; set; }
        public List<List<Tuple<int, int>>> BboxPoints { get; set; }


        public plateOCR(List<string> text, List<List<Tuple<int, int>>> bboxPoints)
        {
            Text = text;
            BboxPoints = bboxPoints;
        }
    }
}
