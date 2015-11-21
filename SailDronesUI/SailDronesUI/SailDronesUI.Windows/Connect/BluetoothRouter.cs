using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace SailDronesUI.Connect
{
   public  class BluetoothRouter
    {
        public delegate void AddOnMessageReceivedDelegate(byte[] message);
        public event AddOnMessageReceivedDelegate MessageReceived;

        public delegate void AddOnErrorReceivedDelegate(object o, Exception ex);
        public event AddOnErrorReceivedDelegate ErrorOccured;

        Task _listen;
        private IAsyncOperation<RfcommDeviceService> _connectService;
        private IAsyncAction _connectAction;
        private RfcommDeviceService _rfcommService;
        private StreamSocket _socket;
        private DataReader _reader;
        private DataWriter _writer;
        private  const uint _packetSize = 12;

        public BluetoothRouter(uint packetSize)
        {
            PacketSize = packetSize;

        }
        public BluetoothRouter()
        {
            PacketSize = _packetSize;

        }
        public uint PacketSize { get; set; }
        public async Task<List<BluetoothDevice>> DeviceList()
        {
            List<BluetoothDevice> ret = new List<BluetoothDevice>();
            DeviceInformationCollection serviceInfoCollection = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));
            foreach (var item in serviceInfoCollection)
            {
                ret.Add(new BluetoothDevice(item.Id, item.Name));

            }

            return ret;
        }
        public void  SendMessage (byte[] message)
        {
            uint sentMessageSize = 0;
            foreach(byte m in message)
            _writer.WriteByte(m);
            if (_writer != null)
            {


                 uint x =  _writer.StoreAsync().GetResults();
                uint z = x;
            }

            
        }
        public async Task<uint> SendStringMessageAsync(string message)
        {
            uint sentMessageSize = 0;
            if (_writer != null)
            {
                uint messageSize = _writer.MeasureString(message);
                _writer.WriteByte((byte)messageSize);
                sentMessageSize = _writer.WriteString(message);
                await _writer.StoreAsync();
            }
            return sentMessageSize;
        }
        public void DisconnectFromDevice()
        {

            if (_reader != null)
                _reader = null;
            if (_writer != null)
            {
                _writer.DetachStream();
                _writer = null;
            }
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
            if (_rfcommService != null)
                _rfcommService = null;
        }
        public async Task<RfcommDeviceService> ConnectToDevice(string id)
        {
            // Initialize the target Bluetooth RFCOMM device service
            _rfcommService = await RfcommDeviceService.FromIdAsync(id);

            if (_rfcommService != null)
            {
                // Create a socket and connect to the target 
                _socket = new StreamSocket();
                _connectAction = _socket.ConnectAsync(_rfcommService.ConnectionHostName, _rfcommService.ConnectionServiceName, SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);

                _writer = new DataWriter(_socket.OutputStream);
                _reader = new DataReader(_socket.InputStream);
                _listen = ListenForMessagesAsync();

          
            }
            // else
            //  OnExceptionOccuredEvent(this, new Exception("Unable to create service.\nMake sure that the 'bluetooth.rfcomm' capability is declared with a function of type 'name:serialPort' in Package.appxmanifest."));
            return _rfcommService;
        }

        private void DataArrives( byte[] what)
        {
            if (MessageReceived != null)
                MessageReceived(what);
        }
        private void LogError(object o, Exception ex)
        {
            if (ErrorOccured != null)
                ErrorOccured(o, ex);
        }
        private async Task ListenForMessagesAsync()
        {
            while (_reader != null)
            {
                try
                {
                   
                    // Read the message. 
                    uint messageLength = PacketSize;
                    uint actualMessageLength = await _reader.LoadAsync(messageLength);
                    //// Read the message and process it.
                    byte[] _b1 = new byte[actualMessageLength];
                    _reader.ReadBytes(_b1);
                    DataArrives(_b1);
                }
                catch (Exception ex)
                {

                    LogError(this, ex);
                }

            }
        }
    }
    public class BluetoothDevice
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public BluetoothDevice( string id, string desc)
        {
            Id = id;
            Description = desc;


        }

        public override string ToString()
        {
            return Description;
        }
    }
}
