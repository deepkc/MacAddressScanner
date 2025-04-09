using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MacAddressScanner.Services
{
    public static class WiFiMacAddressScanner
    {
        // Method to get available Wi-Fi networks and their MAC addresses
        public static List<string> GetAvailableNetworks()
        {
            List<string> networks = new List<string>();

            try
            {
                // Run the netsh command to get Wi-Fi networks in BSSID mode
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "netsh",
                        Arguments = "wlan show networks mode=bssid", // Using bssid mode to get MAC addresses
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Debug output to see what we're getting from the command
                Console.WriteLine("Command Output: \n" + output);

                // Split the output into lines (handle all line endings properly)
                var lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Regex pattern to match valid MAC addresses (XX:XX:XX:XX:XX:XX)
                var macPattern = new Regex(@"([A-Fa-f0-9]{2}:){5}[A-Fa-f0-9]{2}");

                string currentSSID = "Unknown SSID"; // Track the current SSID

                // Iterate through the lines and fetch the MAC addresses (BSSID)
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();

                    // Check if the line contains "SSID" to get the Wi-Fi name
                    if (trimmedLine.StartsWith("SSID ", StringComparison.OrdinalIgnoreCase))
                    {
                        int colonIndex = trimmedLine.IndexOf(':');
                        if (colonIndex != -1 && colonIndex + 1 < trimmedLine.Length)
                        {
                            currentSSID = trimmedLine.Substring(colonIndex + 1).Trim();
                        }
                    }

                    // Check if the line contains "BSSID" (case-insensitive check)
                    if (trimmedLine.StartsWith("BSSID", StringComparison.OrdinalIgnoreCase))
                    {
                        // Find the position of ':' and extract the part after it
                        int colonIndex = trimmedLine.IndexOf(':');
                        if (colonIndex != -1 && colonIndex + 1 < trimmedLine.Length)
                        {
                            var macAddress = trimmedLine.Substring(colonIndex + 1).Trim();

                            // Check if the extracted part matches the MAC address pattern
                            if (macPattern.IsMatch(macAddress))
                            {
                                // Add the MAC address along with the SSID to the list
                                networks.Add($"{macAddress} ({currentSSID})");
                            }
                        }
                    }
                }

                // Check if no networks were found
                if (networks.Count == 0)
                {
                    Console.WriteLine("No Wi-Fi MAC addresses found.");
                }
                else
                {
                    // Output the MAC addresses and SSIDs found
                    Console.WriteLine("Found Wi-Fi MAC Addresses (BSSIDs) with SSIDs:");
                    foreach (var network in networks)
                    {
                        Console.WriteLine(network);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching Wi-Fi MAC addresses: " + ex.Message);
            }

            return networks;
        }
    }
}
