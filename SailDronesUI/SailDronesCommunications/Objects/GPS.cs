using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications.Objects
{
   public class GPS
    {
        public byte Satelites {get; set; }
        public float lat { get; set; }
        public float lon { get; set; }
        public float heading { get; set; }
        public float speed { get; set; }
        
    }
}
