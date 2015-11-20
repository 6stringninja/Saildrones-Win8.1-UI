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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SailDronesUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            btCon.MessageReceived += BtCon_MessageReceived;
            btCon.Disconnected += BtCon_Disconnected;
            btCon.Connected += BtCon_Connected;
        }

        private void BtCon_Connected()
        {
            popupConnect.IsOpen = false;
        }

        private void BtCon_Disconnected()
        {
         
        }

        private void BtCon_MessageReceived(byte[] message)
        {
            string str = System.Text.Encoding.UTF8.GetString(message, 0, message.Length);
            btCon.SetDebugText(str);
        }

        private void btnConnectionDialog_Click(object sender, RoutedEventArgs e)
        {
            popupConnect.IsOpen = !popupConnect.IsOpen;
        }
    }
}
