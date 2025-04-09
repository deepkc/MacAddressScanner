using System;
using System.Collections.Generic;
using InTheHand.Net.Sockets; // Requires 32feet.NET library
using InTheHand.Net.Bluetooth;

namespace MacAddressScanner.Services
{
    public static class BluetoothMacAddressScanner
    {
        public static List<string> GetBluetoothDevices()
        {
            List<string> devices = new List<string>();

            try
            {
                // Create a Bluetooth client
                var bluetoothClient = new BluetoothClient();

                // Discover Bluetooth devices (maximum 255 devices)
                Console.WriteLine("Starting Bluetooth device discovery...");
                BluetoothDeviceInfo[] devicesList = bluetoothClient.DiscoverDevices(255);

                if (devicesList.Length == 0)
                {
                    Console.WriteLine("No Bluetooth devices found.");
                }
                else
                {
                    Console.WriteLine($"Found {devicesList.Length} Bluetooth devices.");
                }

                // Iterate through each discovered device
                foreach (var device in devicesList)
                {
                    // Get device name and MAC address
                    string deviceName = device.DeviceName;
                    string macAddress = device.DeviceAddress.ToString();

                    // Print device information
                    Console.WriteLine($"Device Name: {deviceName}, MAC Address: {macAddress}");

                    // Add the Bluetooth device to the list
                    devices.Add($"{deviceName} ({macAddress})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching Bluetooth MAC addresses: " + ex.Message);
            }

            return devices;
        }

        // Entry point for testing the Bluetooth scanner
        //public static void Main()
        //{
        //    List<string> bluetoothDevices = GetBluetoothDevices();

        //    Console.WriteLine("\nDiscovered Bluetooth Devices:");
        //    foreach (var device in bluetoothDevices)
        //    {
        //        Console.WriteLine(device);
        //    }

        //    Console.WriteLine("\nPress any key to exit...");
        //    Console.ReadKey();
        //}
    }
}
