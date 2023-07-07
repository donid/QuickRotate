
using Microsoft.Toolkit.Uwp.Notifications;

using ScreenSettingsLib;



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
			ScreenSettings.ShowDesktop();
		}


		private void buttonTurnScreensOff_Click(object sender, EventArgs e)
		{
			ScreenSettings.TurnScreensOff();
			Thread.Sleep(5000);
			ScreenSettings.TurnScreensOn();
		}
	}
}
