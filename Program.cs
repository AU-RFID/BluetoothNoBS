using System;
using Newtonsoft.Json;
using System.IO.Pipes;
using System.IO;

namespace BluetoothNoBS;

internal class BluetoothClassicPacket
{
	public string Name;
	public string Address;
	public bool IsRemembered;
	public bool IsPaired;
	public string Type;
	public string Services;
}

internal class Program
{
	private static BluetoothScanner bluetoothScanner;
	private static string pipeName = "Bluetooth Classic";

	static void Main(string[] args)
	{
		bluetoothScanner = new();

		while (true)
		{
			using NamedPipeServerStream serverStream = new(pipeName, PipeDirection.Out);
			serverStream.WaitForConnection();
			using StreamWriter streamWriter = new(serverStream) { AutoFlush = true };
			try
			{
				Refresh(streamWriter);
			} catch (Exception exception)
			{
				Console.WriteLine(exception);
			}
		}
	}

	private static void Refresh(StreamWriter streamWriter)
	{
		while (true)
		{
			// Get devices
			BluetoothClassicDevice[] devices = bluetoothScanner.DiscoverDevices();
			foreach (BluetoothClassicDevice device in devices) Console.WriteLine(device.Name);

			// Pipe devices
			string json = JsonConvert.SerializeObject(devices);
			streamWriter.WriteLine(json);
		}
	}
}
