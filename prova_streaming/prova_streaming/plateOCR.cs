using System;
using System.Collections.Generic;

namespace prova_streaming
{
    public class PlateOCR
    {
        // Property for Text with proper getter and setter
        public List<string> Text { get; set; }

        // Property for BboxPoints with proper getter and setter
        public List<List<List<int>>> bbox_points { get; set; }

        // Constructor
        public PlateOCR(List<string> text, List<List<List<int>>> bbox_points)
        {
            Text = text;
            this.bbox_points = bbox_points;
        }

        // Default constructor for deserialization
        public PlateOCR()
        {
            Text = new List<string>();
            bbox_points = new List<List<List<int>>>();
        }
    }
}


/*{
  "bbox_points": [
    [
      [
        206,
        897
      ],
      [
        287,
        897
      ],
      [
        206,
        944
      ],
      [
        287,
        944
      ]
    ],
    [
      [
        109,
        580
      ],
      [
        388,
        580
      ],
      [
        109,
        668
      ],
      [
        388,
        668
      ]
    ]
  ],
  "text": [
    "CZ889KF"
  ]
}*/