using ScreenSettingsLib;

namespace ScreenSettingsConsoleTest
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Turning Screen Off!");
			ScreenSettings.TurnScreensOff();
			Thread.Sleep(15000);
			Console.WriteLine("Turning Screen On!");
			ScreenSettings.TurnScreensOn();
		}
	}
}