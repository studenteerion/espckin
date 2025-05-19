using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace provamail
{
    internal class Targa
    {
        // nome della camera
        public string camera_name { get; set; }

        public string camera_location { get; set; } 
        
        public string camera_description { get; set; }

        //targa
        public string plate_number { get; set; }

        //data
        public string timestamp { get; set; }

        public string image { get; set; }

        //bbox 
        public bbox bounding_box { get; set; }

        public class bbox
        {
            public int x_min { get; set; }

            public int y_min { get; set; }

            public int x_max { get; set; }

            public int y_max { get; set; }
        }
    }
}
