using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications
{
    interface IPacket
    {
      
    
        byte[] Data { get; set; }
        SailDronesCommunicationsCommands Command { get; set; }


    }
}
