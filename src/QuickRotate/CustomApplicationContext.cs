using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Toolkit.Uwp.Notifications;

using ScreenSettingsLib;


namespace QuickRotate
{
	internal class CustomApplicationContext : ApplicationContext
	{

		private NotifyIcon _notifyIcon { get; set; }

		// uncomment all code related to _mainForm, if we ever want to display more GUI
		//private Form1? _mainForm;

		private System.ComponentModel.IContainer? components = null;

		public CustomApplicationContext()
		{
			components = new System.ComponentModel.Container();
			_notifyIcon = new NotifyIcon(components);
			_notifyIcon.Icon = Properties.Resources.RotateScreen;
			_notifyIcon.Visible = true;
			_notifyIcon.Text = "QuickRotate";
			_notifyIcon.ContextMenuStrip = new ContextMenuStrip(components);
			_notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			_notifyIcon.ContextMenuStrip.MouseLeave += ContextMenuStrip_MouseLeave;
			//_notifyIcon.Click += _notifyIcon_Click;
			_notifyIcon.MouseDoubleClick += _notifyIcon_MouseDoubleClick;
			_notifyIcon.MouseClick += _notifyIcon_Click;

			Image? img = null;

			_notifyIcon.ContextMenuStrip.Items.Add("Show Desktop", img, showDesktop_Click);
			_notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//_notifyIcon.ContextMenuStrip.Items.Add("Show &Details", img, showDetailsItem_Click);
			_notifyIcon.ContextMenuStrip.Items.Add("0 Deg", img, rotate0Deg_Click);
			_notifyIcon.ContextMenuStrip.Items.Add("90 Deg CW", img, rotate90Deg_Click);
			_notifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			_notifyIcon.ContextMenuStrip.Items.Add("About", img, showHelpItem_Click);
			_notifyIcon.ContextMenuStrip.Items.Add("&Exit", img, exitItem_Click);

		}

		private RotationClockwise CalculateNewRotation(RotationClockwise oldRotation)
		{
			if (oldRotation == RotationClockwise.Deg0)
			{
				return RotationClockwise.Deg90;
			}
			return RotationClockwise.Deg0;
		}

		private void _notifyIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
		{
			_notifyIcon.ContextMenuStrip?.Close();// MouseClick-event seems to be sent always / first
			RotationClockwise oldRotation;
			try
			{
				oldRotation = PrimaryScreenRotator.GetCurrentRotation();
			}
			catch (InvalidOperationException ex)
			{
				ToastContentBuilder toastBuilder = new ToastContentBuilder()
					.AddText($"Querying current rotation failed:")
					.AddText(ex.Message);
				toastBuilder.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
				return;
			}
			RotationClockwise newRotation = CalculateNewRotation(oldRotation);
			RotateDisplay(newRotation);
		}

		private void showDesktop_Click(object? sender, EventArgs e)
		{
			ScreenSettings.ShowDesktop();
		}

		private void rotate0Deg_Click(object? sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg0);
		}

		private void rotate90Deg_Click(object? sender, EventArgs e)
		{
			RotateDisplay(RotationClockwise.Deg90);
		}

		private static void RotateDisplay(RotationClockwise rotation)
		{
			string result = PrimaryScreenRotator.Rotate(rotation);

			ToastContentBuilder toastBuilder = new ToastContentBuilder()
				.AddText($"Rotation to {rotation} - result:")
				.AddText(result);
			try
			{
				// encountered NullReferenceException here after quickly rotating display twice
				//   at Windows.UI.Notifications.ToastNotification..ctor(XmlDocument content)
				//   at Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder.Show(CustomizeToast customize)
				//   at Microsoft.Toolkit.Uwp.Notifications.ToastContentBuilder.Show()
				toastBuilder.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
			}
			catch (NullReferenceException ex)
			{
				Trace.WriteLine("Error in RotateDisplay - toastBuilder.Show(): " + ex.Message);
			}
		}


		private void ContextMenuStrip_MouseLeave(object? sender, EventArgs e)
		{
			_notifyIcon.ContextMenuStrip?.Close();
		}

		// show menu on left click - menu will not close when user clicks outside of menu
		private void _notifyIcon_Click(object? sender, EventArgs e)
		{
			if (_notifyIcon.ContextMenuStrip == null)
			{
				return;
			}

			if (_notifyIcon.ContextMenuStrip.Visible == false)
			{
				_notifyIcon.ContextMenuStrip.Show(Control.MousePosition);
			}
			else
			{
				_notifyIcon.ContextMenuStrip.Close();
			}
		}


		/*
		private void ShowDetailsForm()
		{
			if (_mainForm == null)
			{
				_mainForm = new Form1();
				_mainForm.Closed += detailsForm_Closed;
				_mainForm.Show();
			}
			else
			{
				_mainForm.Activate();
			}
		}*/

		// attach to context menu items
		private void showDetailsItem_Click(object? sender, EventArgs e)
		{
			//ShowDetailsForm();
		}

		// null out the forms so we know to create a new one.
		private void detailsForm_Closed(object? sender, EventArgs e)
		{
			//_mainForm = null;
		}

		private void ContextMenuStrip_Opening(object? sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = false;

			//hostManager.BuildContextMenu(_notifyIcon.ContextMenuStrip);
		}

		private void showHelpItem_Click(object? sender, EventArgs e)
		{
			MessageBox.Show("QuickRotate App by donid" + Environment.NewLine + "V1.2", "QuickRotate");
		}

		private void exitItem_Click(object? sender, EventArgs e)
		{
			ExitThread();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}

		protected override void ExitThreadCore()
		{
			//if (_mainForm != null)
			//{
			//	_mainForm.Close();
			//}

			_notifyIcon.Visible = false; // should remove lingering tray icon!
			base.ExitThreadCore();
		}

	}
}
