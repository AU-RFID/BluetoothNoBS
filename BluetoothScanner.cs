using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Text.RegularExpressions;

namespace BluetoothNoBS;

internal class BluetoothScanner
{
	BluetoothClient bluetoothClient;

	public BluetoothScanner()
	{
		bluetoothClient = new();
	}

	private string FormatAddress(string rawAddress)
	{
		return string.Join(":", Enumerable.Range(0, 6).Select(index => rawAddress.Substring(index * 2, 2)));
	}

	private string FormatType(string rawType)
	{
		return Regex.Replace(rawType, @"([a-z])([A-Z])", "$1 $2");
	}

	public BluetoothClassicDevice[] DiscoverDevices()
	{
		// Discover devices
		BluetoothDeviceInfo[] devicesInfo = bluetoothClient.DiscoverDevices(int.MaxValue, false, true, true, false);
		BluetoothClassicDevice[] devices = new BluetoothClassicDevice[devicesInfo.Length];

		// Populate array
		for (int deviceIndex = 0; deviceIndex < devicesInfo.Length; deviceIndex++)
		{
			BluetoothDeviceInfo deviceInfo = devicesInfo[deviceIndex];
			BluetoothClassicDevice device = new BluetoothClassicDevice
			{
				Name = deviceInfo.DeviceName,
				Address = FormatAddress(deviceInfo.DeviceAddress.ToString()),
				IsRemembered = deviceInfo.Remembered,
				IsPaired = deviceInfo.Connected,
				Type = FormatType(deviceInfo.ClassOfDevice.Device.ToString()),
				Services = FormatType(deviceInfo.ClassOfDevice.Service.ToString())
			};
			devices[deviceIndex] = device;
		}
		return devices;
	}
}
