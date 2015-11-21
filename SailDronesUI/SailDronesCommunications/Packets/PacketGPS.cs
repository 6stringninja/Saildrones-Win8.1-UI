using SailDronesCommunications.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications.Packets
{
    public class PacketGPSPacket : Packet
    {
        public PacketGPSPacket() : base(SailDronesCommunicationsCommands.GPS,typeof(GPS) )
        {


        }
        public override byte[] ToBytes()
        {
            byte[] _e = new byte[1];
            GPS tmp = ((GPS)(PacketObject));

            return _e;

        }

        public override    void Process(byte[] _b)
        {
            PacketObject = new GPS();
        }
        
    }
}
