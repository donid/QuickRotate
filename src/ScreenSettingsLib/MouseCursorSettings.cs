using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

//using Vanara.PInvoke;

// todo: infos regarding cursor color - maybe we can make a setter for it
//https://stackoverflow.com/a/73583065


namespace ScreenSettingsLib
{
	/// <summary>
	/// System-wide setting for the mouse cursor
	/// </summary>
	public class MouseCursorSettings
	{
		private const string cRegPathSizeGrade = @"SOFTWARE\Microsoft\Accessibility";
		private const string cRegKeySizeGrade = "CursorSize";
		private const string cRegPathSizePixels = @"Control Panel\Cursors";
		private const string cRegKeySizePixels = "CursorBaseSize";

		/// <summary>
		/// System-wide set the size of the mouse cursor (all shapes of it)
		/// </summary>
		/// <param name="cursorSizeGrade">cursor size as displayed in SystemSettings.exe (Accessibility) 1..15</param>
		/// <exception cref="ArgumentException"></exception>
		public static void SetCursorSizeGrade(int cursorSizeGrade)
		{
			if (cursorSizeGrade < 1 || cursorSizeGrade > 15)
			{
				throw new ArgumentException("Value must be 1 .. 15", nameof(cursorSizeGrade));
			}

			int cursorSizeInPixels = CursorSizeGradeToPixels(cursorSizeGrade);

			// found on https://stackoverflow.com/a/69687213 - 0x2029 seems to be still undocumented
			const /*User32.SPI*/ uint spi = /*(User32.SPI)*/ 0x2029;// decimal 8233
			const /*User32.SPIF*/ uint spif = 1 /*User32.SPIF.SPIF_UPDATEINIFILE*/;
			// this does not change: HKCU\SOFTWARE\Microsoft\Accessibility\CursorSize
			// it changes: HKCU\Control Panel\Cursors\CursorBaseSize
			bool success = NativeMethods.SystemParametersInfo(spi, 0, cursorSizeInPixels, spif);

			// size change works without this, but we want to have a consistent value, that is displayed in SystemSettings.exe
			using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(cRegPathSizeGrade, true))
			{
				if (key == null)
				{
					Debug.WriteLine("cRegPathSizeGrade registry key not found.");
				}
				else
				{
					key.SetValue(cRegKeySizeGrade, cursorSizeGrade, RegistryValueKind.DWord);
				}
			}
		}

		public static int CursorSizeGradeToPixels(int cursorSizeGrade)
		{
			return 16 + cursorSizeGrade * 16;
		}

		public static CursorSize GetCursorSize()
		{
			const int defaultSize = -1;
			int cursorSizeGrade = defaultSize;
			using (RegistryKey? gradeKey = Registry.CurrentUser.OpenSubKey(cRegPathSizeGrade, true))
			{
				if (gradeKey != null)
				{
					cursorSizeGrade = (int?)gradeKey.GetValue(cRegKeySizeGrade) ?? defaultSize;
				}
			}

			int cursorSizeInPixels = defaultSize;
			using (RegistryKey? pixelsKey = Registry.CurrentUser.OpenSubKey(cRegPathSizePixels, true))
			{
				if (pixelsKey != null)
				{
					cursorSizeInPixels = (int?)pixelsKey.GetValue(cRegKeySizePixels) ?? defaultSize;
				}
			}

			bool isConsistent = cursorSizeInPixels == CursorSizeGradeToPixels(cursorSizeGrade);
			var result = new CursorSize(cursorSizeGrade, cursorSizeInPixels, isConsistent);
			return result;
		}


	}

	// could be in .net80:
	// record CursorSize
	public class CursorSize
	{
		public CursorSize(int cursorSizeGrade, int cursorSizeInPixels, bool isConsistent)
		{
			CursorSizeGrade = cursorSizeGrade;
			CursorSizeInPixels = cursorSizeInPixels;
			IsConsistent = isConsistent;
		}

		public int CursorSizeGrade { get; }
		public int CursorSizeInPixels { get; }
		public bool IsConsistent { get; }

	}
}
