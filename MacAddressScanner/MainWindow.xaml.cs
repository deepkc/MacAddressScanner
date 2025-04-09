using System;
using System.Collections.ObjectModel;
using System.Windows;
using MacAddressScanner.Services;

namespace MacAddressScanner
{
    public partial class MainWindow : Window
    {
        // ObservableCollections for data binding
        public ObservableCollection<string> WiFiMacAddresses { get; set; }
        public ObservableCollection<string> BluetoothMacAddresses { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the ObservableCollections
            WiFiMacAddresses = new ObservableCollection<string>();
            BluetoothMacAddresses = new ObservableCollection<string>();

            // Bind collections to the ListBoxes
            WiFiList.ItemsSource = WiFiMacAddresses;
            BluetoothList.ItemsSource = BluetoothMacAddresses;
        }

        // Scan Wi-Fi MAC Addresses
        private void ScanWiFi_Click(object sender, RoutedEventArgs e)
        {
            WiFiMacAddresses.Clear();  // Clears previous data
            try
            {
                // Fetch available Wi-Fi MACs
                var wifiMacs = WiFiMacAddressScanner.GetAvailableNetworks();
                foreach (var mac in wifiMacs)
                {
                    WiFiMacAddresses.Add(mac);  // Adds new MAC addresses
                }

                StatusMessage.Text = $"Wi-Fi MAC Addresses: {wifiMacs.Count} found";
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Error scanning Wi-Fi MAC addresses: {ex.Message}";
            }
        }

        // Scan Bluetooth MAC Addresses
        private void ScanBluetooth_Click(object sender, RoutedEventArgs e)
        {
            BluetoothMacAddresses.Clear();  // Clears previous data
            try
            {
                // Fetch available Bluetooth devices
                var bluetoothMacs = BluetoothMacAddressScanner.GetBluetoothDevices();
                foreach (var mac in bluetoothMacs)
                {
                    BluetoothMacAddresses.Add(mac);  // Adds new MAC addresses
                }

                StatusMessage.Text = $"Bluetooth MAC Addresses: {bluetoothMacs.Count} found";
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Error scanning Bluetooth MAC addresses: {ex.Message}";
            }
        }
    }
}
