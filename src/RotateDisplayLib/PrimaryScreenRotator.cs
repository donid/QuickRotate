using System;


namespace RotateDisplayLib
{
	/// <summary>
	/// This specifies how the windows desktop will be displayed, when you don't move/rotate your pysical screen from its default angle.
	/// </summary>
	public enum RotationClockwise
	{
		Deg0,
		Deg90,
		Deg180,
		Deg270
	}

	public class PrimaryScreenRotator
	{
		public static RotationClockwise GetCurrentRotation()
		{
			DEVMODE dm = DEVMODE.Create();

			if (0 == NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
			{
				throw new InvalidOperationException("Failed to enumerate display settings.");
			}

			return DmdoToRotationClockwise(dm.dmDisplayOrientation);
		}

		public static string Rotate(RotationClockwise rotation)
		{
			DMDO newOrientation = RotationClockwiseToDmdo(rotation);

			DEVMODE dm = DEVMODE.Create();

			if (0 == NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm))
			{
				return "Failed to enumerate display settings.";
			}

			if (SwapWidthAndHeightNeccessary(dm.dmDisplayOrientation, newOrientation))
			{
				SwapWidthAndHeight(ref dm);
			}
			dm.dmDisplayOrientation = newOrientation;


			int res1 = NativeMethods.ChangeDisplaySettings(ref dm, NativeMethods.CDS_TEST);

			if (res1 == NativeMethods.DISP_CHANGE_FAILED)
			{
				return "Unable to change display settings.";
			}

			int res2 = NativeMethods.ChangeDisplaySettings(ref dm, NativeMethods.CDS_UPDATEREGISTRY);
			if (res2 == NativeMethods.DISP_CHANGE_SUCCESSFUL)
			{
				return "Successfully changed display settings.";
			}
			if (res2 == NativeMethods.DISP_CHANGE_RESTART)
			{
				return "Changed display settings will be active after reboot.";
			}
			return "Failed to change display settings.";
		}


		private static DMDO RotationClockwiseToDmdo(RotationClockwise rotation)
		{
			switch (rotation)
			{
				case RotationClockwise.Deg0:
					return DMDO.DMDO_DEFAULT;
				case RotationClockwise.Deg90:
					return DMDO.DMDO_270;
				case RotationClockwise.Deg180:
					return DMDO.DMDO_180;
				case RotationClockwise.Deg270:
					return DMDO.DMDO_90;
				default:
					throw new ArgumentException("Unknown value for rotation");
			}
		}

		private static RotationClockwise DmdoToRotationClockwise(DMDO rotation)
		{
			switch (rotation)
			{
				case DMDO.DMDO_DEFAULT:
					return RotationClockwise.Deg0;
				case DMDO.DMDO_90:
					return RotationClockwise.Deg270;
				case DMDO.DMDO_180:
					return RotationClockwise.Deg180;
				case DMDO.DMDO_270:
					return RotationClockwise.Deg90;
				default:
					throw new ArgumentException("Unknown value for rotation");
			}
		}


		private static void SwapWidthAndHeight(ref DEVMODE dm)
		{
			uint temp = dm.dmPelsHeight;
			dm.dmPelsHeight = dm.dmPelsWidth;
			dm.dmPelsWidth = temp;
		}

		private static bool SwapWidthAndHeightNeccessary(DMDO oldOrientation, DMDO newOrientation)
		{
			bool oldIs0Or180 = oldOrientation == DMDO.DMDO_180 || oldOrientation == DMDO.DMDO_DEFAULT;
			bool newIs90Or270 = newOrientation == DMDO.DMDO_270 || newOrientation == DMDO.DMDO_90;
			if (oldIs0Or180 && newIs90Or270)
			{
				return true;
			}
			bool newIs0Or180 = newOrientation == DMDO.DMDO_180 || newOrientation == DMDO.DMDO_DEFAULT;
			bool oldIs90Or270 = oldOrientation == DMDO.DMDO_270 || oldOrientation == DMDO.DMDO_90;
			if (oldIs90Or270 && newIs0Or180)
			{
				return true;
			}
			return false;
		}

	}
}
