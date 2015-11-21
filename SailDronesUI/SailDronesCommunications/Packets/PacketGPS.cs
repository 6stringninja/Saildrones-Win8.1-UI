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
            GPS tmp = new GPS()
            {
                Satelites = _b[0],
                lat = BitConverter.ToSingle(_b, 1),
                lon = BitConverter.ToSingle(_b, 5),
                heading = BitConverter.ToSingle(_b, 9),
                speed = BitConverter.ToSingle(_b, 13)
            };
        //      public byte Satelites { get; set; }
        //public float lat { get; set; }
        //public float lon { get; set; }
        //public float heading { get; set; }
        //public float speed { get; set; }
   
            PacketObject = tmp; 
        }
        
    }
}
