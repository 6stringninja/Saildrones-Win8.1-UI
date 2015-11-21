using SailDronesCommunications.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications.Packets
{
  public  class PacketPing : Packet
    {
        Ping ping;
       
        public PacketPing() : base(SailDronesCommunicationsCommands.Ping,typeof(PacketPing) )
        {
            


        }
        public override void request()
        {
            base.request();

            if ((Ping)(PacketObject) == null)
                PacketObject = new Ping() { TimeStamp = 99 };

          foreach(byte b in  BitConverter.GetBytes(((Ping)(PacketObject)).TimeStamp))
            {
                AddByteToPackage(b);
         }
            SendPackage();
        }
        public override byte[] ToBytes()
        {
            Ping tmp = ((Ping)(PacketObject));
            tmp.TimeStamp = (Int32)DateTime.Now.Ticks;
            return BitConverter.GetBytes(tmp.TimeStamp);

        }
        public override  void  Process(byte[] _b)
        {

            ping = new Ping();

            ping.TimeStamp= BitConverter.ToInt32(_b, 0);

            PacketObject = ping;

        }
    }

 
}
