using SailDronesCommunications.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SailDronesCommunications
{


    public enum SailDronesCommunicationsCommands
    {
        Ping = 1,
        GPS = 5
    }
    public enum SailDronesCommunicationsProcessStatus
    {
        Default = 0,
        CheckChar1,
        CheckChar2,
        Message,
        Size,
        CheckSum,
        Data,
        Process,
        Complete
    }
    public class Protocol
    {
        public delegate void NewObjectDelegate(object o, Type t);
        public event NewObjectDelegate Update;
        public delegate void ProtocolSendDataDelegate(byte[] b);
        public event ProtocolSendDataDelegate SendData;


        SailDronesCommunicationsProcessStatus _processStatus = SailDronesCommunicationsProcessStatus.Default;
        Packet _current;
        List<Packet> _packets = new List<Packet>();
        byte messageCommand = 0;
        byte checkSum = 0;
        byte checkSumRec = 0;
        byte Size = 0;
        byte[] buffer = new byte[256];
        byte index = 0;

        private void updateObject(object o, Type t)
        {

            if (Update != null)
                Update(o, t);
        }
        public Protocol()
        {
          PacketPing tmp = new PacketPing();
            tmp.Send += _Send;
            _packets.Add(tmp);
            _packets.Add(new PacketGPSPacket());

        }

        private void _Send(byte[] message)
        {
            if (SendData != null)
                SendData(message);
        }

        public void ProtocolRequest(SailDronesCommunicationsCommands com)
        {
            foreach (var item in _packets)
            {
                if (item.Command == com)
                    item.request();

            }


        }
        public void Process(byte[] ss)
        {


            
            foreach (byte _c in ss)
                if (_c == 'B' && _processStatus == SailDronesCommunicationsProcessStatus.Default)
                    _processStatus = SailDronesCommunicationsProcessStatus.CheckChar1;
                else if (_c == '>' && _processStatus == SailDronesCommunicationsProcessStatus.CheckChar1)
                    _processStatus = SailDronesCommunicationsProcessStatus.CheckChar2;
                else if (_c == '$' && _processStatus == SailDronesCommunicationsProcessStatus.CheckChar2)
                    _processStatus = SailDronesCommunicationsProcessStatus.Message;
                else if (_processStatus == SailDronesCommunicationsProcessStatus.Message)
                {
                    checkSum = 0;
                    checkSum ^= (byte)_c;
                    //        System.Diagnostics.Debug.WriteLine(checkSum);
                    messageCommand = (byte)_c;
                    _processStatus = SailDronesCommunicationsProcessStatus.Size;
                }
                else if (_processStatus == SailDronesCommunicationsProcessStatus.Size)
                {

                    //   checkSum ^= (byte)_c;
                    Size = (byte)_c;
                    _processStatus = SailDronesCommunicationsProcessStatus.CheckSum;

                }
                else if (_processStatus == SailDronesCommunicationsProcessStatus.CheckSum)
                {


                    checkSumRec = (byte)_c;
                    //      System.Diagnostics.Debug.WriteLine(checkSum);
                    _processStatus = SailDronesCommunicationsProcessStatus.Data;

                }
                else if (_processStatus == SailDronesCommunicationsProcessStatus.Data && index <= Size)
                {

                    if (index < Size)
                    {
                        checkSum ^= (byte)_c;
                        buffer[index] = (byte)_c;
                        //         System.Diagnostics.Debug.WriteLine(checkSum);
                    }
                    else
                    {
                        _processStatus = SailDronesCommunicationsProcessStatus.Process;

                    }

                    index++;
                    if (index == Size)
                        _processStatus = SailDronesCommunicationsProcessStatus.Process;

                }
                else if (_processStatus == SailDronesCommunicationsProcessStatus.Process)
                {
                    if (checkSum == checkSumRec)
                    {
                        // string message = System.Text.Encoding.UTF8.GetString(buffer, 0, Size);
                        //   Debug.WriteLine("sweet");
                        foreach (var packet in _packets)
                        {
                            if ((int)packet.Command == (int)messageCommand)
                            {
                                packet.Process(buffer);
                                 updateObject(packet.PacketObject, packet.PacketType);
                                break;

                            }

                        }
                        _processStatus = SailDronesCommunicationsProcessStatus.Complete;

                    }

                }


                else

                {
                    messageCommand = 0;
                    checkSum = 0;
                    checkSumRec = 0;
                    Size = 0;

                    index = 0;
                    _processStatus = SailDronesCommunicationsProcessStatus.Default;
                  
                }
         
        }

    }
}
