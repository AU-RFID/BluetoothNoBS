using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothNoBS;

internal struct BluetoothClassicDevice
{
	public string Name;
	public string Address;
	public bool IsRemembered;
	public bool IsPaired;
	public string Type;
	public string Services;
}
