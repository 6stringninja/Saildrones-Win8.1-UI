using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications.Packets
{
     public abstract class Packet  
    {
        public delegate void SendDelegate(byte[] message);
        public event SendDelegate Send;
        public SailDronesCommunicationsCommands Command { get; set; }
        public Type PacketType { get; set; }
        public Object PacketObject { get; set; }
        public abstract  byte[] ToBytes();
        public int byteCount { get; set; } = 256;
        protected  byte[] tempBytes;
        protected int _currentIndex = 0;
        public Packet(SailDronesCommunicationsCommands _com, object o )
        {
            Command = _com;
            PacketType = o.GetType();
            //PacketObject = o;



        }
        public void AddByteToPackage(byte b)
        {
            _currentIndex++;
            tempBytes[_currentIndex] = b;
            tempBytes[4] += 1;
          //  tempBytes[5] ^= tempBytes[_currentIndex];
            

        }
        public void SendPackage()
        {
            SendData(tempBytes, _currentIndex);

            _currentIndex = 0;

        }
        public virtual void request()
        {
            tempBytes = new byte[byteCount];
            tempBytes[0] = (byte)'B';
            tempBytes[1] = (byte)'<';
            tempBytes[2] = (byte)'$';
            tempBytes[3] = (byte)SailDronesCommunicationsCommands.Ping;
            tempBytes[4] = (byte)1;
            //  tempBytes[5] ^= (byte)tempBytes[3];
            _currentIndex = 4;


          //  SendData(tempBytes,7);

        }
        public virtual  void Reset()
        {
             
        }
        private void SendData(byte[] b, int howmany)
        {
            if (Send != null)
            {
                byte[] b2 = new byte[howmany+3];
               
                b2[howmany] = (byte)'C';
                b2[howmany+1] = (byte)'>';
                b2[howmany+2] = (byte)'#';
                for (int i = 0; i < howmany; i++)
                {
                    b2[i] = b[i];
                }
              

                Send(b2);

            }

        }
        public abstract void Process(byte[] _b);
       
       
      
    }
}
