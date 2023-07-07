using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSettingsLib
{
	public class ScreenSettings
	{
		private static int WM_SYSCOMMAND = 0x0112;
		private static int SC_MONITORPOWER = 0xF170;

		private static int SC_MONITORPOWER_Turn_On = -1;
		private static int SC_MONITORPOWER_Turn_Off = 2;
		//private static int SC_MONITORPOWER_LowPower = 1;

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();



		private static IntPtr GetHwndForMonitorOff()
		{
			IntPtr result = IntPtr.Zero;
			try
			{
				result = Process.GetCurrentProcess().MainWindowHandle;// returns 0x0 as handle in a Console-App
			}
			catch (Exception)
			{
				// do nothing!
			}

			if (result == IntPtr.Zero)
			{
				return GetConsoleWindow();// GetConsoleWindow returns 0x0 as handle in a WinForms-App
			}
			if (result == IntPtr.Zero)
			{
				Trace.WriteLine("GetHwndForMonitorOff could not determine a target HWnd - SC_MONITORPOWER will probly be ignored");
			}

			return result;
		}


		// use in a powershell script like this:
		//Import-Module "V:\projects_fk_github\QuickRotate\src\ScreenSettingsLib\bin\Debug\net6.0\ScreenSettingsLib.dll"
		//[ScreenSettingsLib.ScreenSettings]::TurnScreensOff()
		//Start-Sleep -Seconds 10
		//[ScreenSettingsLib.ScreenSettings]::TurnScreensOn()
		public static void TurnScreensOff()
		{
			IntPtr hWnd = GetHwndForMonitorOff();
			SendMessage(hWnd, WM_SYSCOMMAND, SC_MONITORPOWER, SC_MONITORPOWER_Turn_Off);
		}

		// didn't work in my scenario - so I used TurnScreensOnAlternative
		// this turned the screen on for just half a second an then it turns off again
		public static void TurnScreensOn()
		{
			IntPtr hWnd = GetHwndForMonitorOff();
			SendMessage(hWnd, WM_SYSCOMMAND, SC_MONITORPOWER, SC_MONITORPOWER_Turn_On);
		}

		public static void TurnScreensOnAlternative()
		{
			// tested on windows 10 + 11
			// pressing ALT-key wakes machine from screen-off state, but should have little side-effects
			KeyboardSend.KeyDown(KeyboardSend.VK_MENU);
			KeyboardSend.KeyUp(KeyboardSend.VK_MENU);
		}


		//////////////////////////////////////////////////////////

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, int wParam, int lParam);

		private const int WM_COMMAND = 0x111;
		private const int MIN_ALL = 419;
		private const int MIN_ALL_UNDO = 416;



		// similar to MinimizeAllWindows but not the same!
		public static void ShowDesktop()
		{
			//needs reference to C:\Windows\System32\Shell32.dll
			//Shell32.ShellClass objShel = new Shell32.ShellClass();
			//objShel.ToggleDesktop();

			// untested: Add COM reference to "Microsoft Shell Controls and Automation"
			//Shell shellObject = new Shell();
			//shellObject.ToggleDesktop();

			// alternative method which does not need an additional reference
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, WM_COMMAND, MIN_ALL, 0);
		}

		public static void UndoShowDesktop()
		{
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, WM_COMMAND, MIN_ALL_UNDO, 0);
		}


	}


	internal class KeyboardSend
	{
		[DllImport("user32.dll")]
		private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

		private const int KEYEVENTF_EXTENDEDKEY = 1;
		private const int KEYEVENTF_KEYUP = 2;

		// VK_MENU = ALT-key
		internal const byte VK_MENU = 0x12;


		internal static void KeyDown(byte vKey)
		{
			keybd_event(vKey, 0, KEYEVENTF_EXTENDEDKEY, 0);
		}

		internal static void KeyUp(byte vKey)
		{
			keybd_event(vKey, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
		}
	}

}
