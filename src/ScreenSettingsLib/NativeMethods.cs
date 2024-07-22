using System.Runtime.InteropServices;
using System.Text;



//using static Vanara.PInvoke.User32;
//using Vanara.PInvoke;

namespace ScreenSettingsLib
{


	internal enum DMDO : int
	{
		DMDO_DEFAULT = 0,
		DMDO_90 = 1, // 90 Degrees docs say Clockwise, but was CounterClockwise on my machine!!
		DMDO_180 = 2,
		DMDO_270 = 3// 90 Degrees Clockwise on my machine!!
	}

	// dmPelsWidth and dmPelsHeight were both 0 after a call to EnumDisplaySettings when I tried to use Vanara.PInvoke.DEVMODE
	[StructLayout(LayoutKind.Sequential)]
	internal struct DEVMODE
	{

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmDeviceName;

		public short dmSpecVersion;
		public short dmDriverVersion;
		public short dmSize;
		public short dmDriverExtra;
		public int dmFields;
		public int dmPositionX;
		public int dmPositionY;
		public DMDO dmDisplayOrientation;
		public int dmDisplayFixedOutput;
		public short dmColor;
		public short dmDuplex;
		public short dmYResolution;
		public short dmTTOption;
		public short dmCollate;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string dmFormName;

		public short dmLogPixels;
		public short dmBitsPerPel;
		public uint dmPelsWidth;
		public uint dmPelsHeight;
		public int dmDisplayFlags;
		public int dmDisplayFrequency;
		public int dmICMMethod;
		public int dmICMIntent;
		public int dmMediaType;
		public int dmDitherType;
		public int dmReserved1;
		public int dmReserved2;
		public int dmPanningWidth;
		public int dmPanningHeight;

		public static DEVMODE Create()
		{
			DEVMODE dm = new DEVMODE();
			dm.dmDeviceName = new string(new char[32]);
			dm.dmFormName = new string(new char[32]);
			dm.dmSize = (short)Marshal.SizeOf<DEVMODE>();
			return dm;
		}
	};



	internal class NativeMethods
	{
		[DllImport("user32.dll")]
		public static extern int EnumDisplaySettings(string? deviceName, int modeNum, ref DEVMODE devMode);

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettings(ref DEVMODE devMode, int flags);

		public const int ENUM_CURRENT_SETTINGS = -1;

		public const int CDS_UPDATEREGISTRY = 0x01;
		public const int CDS_TEST = 0x02;

		public const int DISP_CHANGE_SUCCESSFUL = 0;
		public const int DISP_CHANGE_RESTART = 1;
		public const int DISP_CHANGE_FAILED = -1;


		// Vanara seems to be missing this overload
		// and Vanara dependecies would make the lib harder to use in powershell
		[DllImport(/*Lib.User32*/ "User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SystemParametersInfo(/*SPI*/uint uiAction, uint uiParam, int vParam, [Optional] /*SPIF*/ uint fWinIni);


	}
}
