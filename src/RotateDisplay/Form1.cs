using System.Runtime.InteropServices;

using Microsoft.Toolkit.Uwp.Notifications;

using RotateDisplayLib;



namespace RotateDisplay
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}


		private void buttonDefault_Click(object sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg0);
		}

		private void buttonNinetyDegrees_Click(object sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg90);
		}

		private void button180Deg_Click(object sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg180);
		}

		private void button270Deg_Click(object sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg270);
		}

		private static void RotateDisplay(RotationClockwise rotation)
		{
			string result = PrimaryScreenRotator.Rotate(rotation);

			ToastContentBuilder toastBuilder = new ToastContentBuilder()
				.AddText($"Rotation to {rotation} - result:")
				.AddText(result);
			toastBuilder.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
		}

		private void buttonShowDesktop_Click(object sender, EventArgs e)
		{
			//needs reference to C:\Windows\System32\Shell32.dll
			//Shell32.ShellClass objShel = new Shell32.ShellClass();
			//objShel.ToggleDesktop();

			// untested: Add COM reference to "Microsoft Shell Controls and Automation"
			//Shell shellObject = new Shell();
			//shellObject.ToggleDesktop();

			// alternaive method which does not need a additional reference
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
			//System.Threading.Thread.Sleep(2000);
			//SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL_UNDO, IntPtr.Zero);
		}

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);
		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

		private const int WM_COMMAND = 0x111;
		private const int MIN_ALL = 419;
		private const int MIN_ALL_UNDO = 416;
	}
}
