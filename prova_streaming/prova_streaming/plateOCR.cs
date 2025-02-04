using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prova_streaming
{
    public class plateOCR
    {
        private List<string> text;
        private List<List<Tuple<int, int>>> bboxPoints;

        // Property for Text with proper getter and setter
        public List<string> Text
        {
            get => text ?? (text = new List<string>());
            set => text = value ?? new List<string>(); // Default to empty list if null is set
        }

        // Property for BboxPoints with proper getter and setter
        public List<List<Tuple<int, int>>> BboxPoints
        {
            get => bboxPoints ?? (bboxPoints = new List<List<Tuple<int, int>>>());
            set => bboxPoints = value ?? new List<List<Tuple<int, int>>>(); // Default to empty list if null is set
        }

        // Constructor
        public plateOCR(List<string> text, List<List<Tuple<int, int>>> bboxPoints)
        {
            Text = text; // Will call the setter
            BboxPoints = bboxPoints; // Will call the setter
        }
    }

}
