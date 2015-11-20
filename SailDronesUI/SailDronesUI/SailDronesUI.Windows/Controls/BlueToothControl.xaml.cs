using SailDronesUI.Connect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SailDronesUI.Controls
{
    public sealed partial class BlueToothControl : UserControl
    {
        public BluetoothRouter router = new BluetoothRouter();
        public delegate void BlueToothControlAddOnMessageReceivedDelegate(byte[] message);
        public event BlueToothControlAddOnMessageReceivedDelegate MessageReceived;
        public delegate void BlueToothControlConnectedDelegate();
        public event BlueToothControlConnectedDelegate Connected;
        public delegate void BlueToothControlDisconnectedDelegate();
        public event BlueToothControlDisconnectedDelegate Disconnected;

        public BlueToothControl()
        {
            this.InitializeComponent();
            router.MessageReceived += Router_MessageReceived;
        }

        private void Router_MessageReceived(byte[] message)
        {
            if (MessageReceived != null)
                MessageReceived(message);
        }
        public void SetDebugText(string txt)
        {

           
        }
        private  async  void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cboDevices.Items.Clear();
            foreach (var item in await router.DeviceList() )
            {
                cboDevices.Items.Add(item);
            } 
           
            
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            btnConnect.IsEnabled = true;
            btnDisconnect.IsEnabled = false;
            router.DisconnectFromDevice();
            if (Disconnected != null)
                Disconnected();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (cboDevices.SelectedItem != null)
            {
                BluetoothDevice btd = (BluetoothDevice)cboDevices.SelectedItem;

                await router.ConnectToDevice (btd.Id);
                btnConnect.IsEnabled = false;
                btnDisconnect.IsEnabled = true;
                if (Connected != null)
                    Connected();
            }
        }
    }
}
