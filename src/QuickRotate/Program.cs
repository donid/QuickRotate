using System.Threading;

namespace QuickRotate
{
	internal static class Program
	{
		private static Mutex? mutex = null;

		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			const string appName = "QuickRotateSingleInstanceApp";
			bool createdNew;
			mutex = new Mutex(true, appName, out createdNew);
			if (!createdNew)
			{
				MessageBox.Show("QuickRotate is already running! Exiting the application.", "QuickRotate");
				return;
			}

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			Application.Run(new CustomApplicationContext());
		}
	}
}
