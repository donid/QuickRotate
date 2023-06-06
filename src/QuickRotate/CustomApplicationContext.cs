using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Toolkit.Uwp.Notifications;

using RotateDisplayLib;

//using Shell32;

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
			_notifyIcon.Click += _notifyIcon_Click;

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
		private void showDesktop_Click(object? sender, EventArgs e)
		{
			//Shell objShel = new Shell32.ShellClass();
			//objShel.ToggleDesktop();

			// alternative method which does not need a additional reference
			IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
			SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
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
			toastBuilder.Show(); // Not seeing the Show() method? Make sure you have version 7.0, and if you're using .NET 6 (or later), then your TFM must be net6.0-windows10.0.17763.0 or greater
		}


		private void ContextMenuStrip_MouseLeave(object? sender, EventArgs e)
		{
			_notifyIcon.ContextMenuStrip.Close();
		}

		// show menu on left click - menu will not close when user clicks outside of menu
		private void _notifyIcon_Click(object? sender, EventArgs e)
		{
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
			MessageBox.Show("QuickRotate App by donid" + Environment.NewLine + "V1.0", "QuickRotate");
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

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);
		[DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
		private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

		private const int WM_COMMAND = 0x111;
		private const int MIN_ALL = 419;
		private const int MIN_ALL_UNDO = 416;
	}
}
