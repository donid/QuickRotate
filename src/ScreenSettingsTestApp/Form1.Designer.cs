namespace RotateDisplay
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			buttonDefault = new Button();
			buttonNinetyDegrees = new Button();
			button180Deg = new Button();
			button270Deg = new Button();
			buttonShowDesktop = new Button();
			buttonTurnScreensOff = new Button();
			SuspendLayout();
			// 
			// buttonDefault
			// 
			buttonDefault.Location = new Point(12, 27);
			buttonDefault.Name = "buttonDefault";
			buttonDefault.Size = new Size(63, 23);
			buttonDefault.TabIndex = 0;
			buttonDefault.Text = "0 Deg";
			buttonDefault.UseVisualStyleBackColor = true;
			buttonDefault.Click += buttonDefault_Click;
			// 
			// buttonNinetyDegrees
			// 
			buttonNinetyDegrees.Location = new Point(81, 27);
			buttonNinetyDegrees.Name = "buttonNinetyDegrees";
			buttonNinetyDegrees.Size = new Size(136, 23);
			buttonNinetyDegrees.TabIndex = 0;
			buttonNinetyDegrees.Text = "90 Deg Clockwise";
			buttonNinetyDegrees.UseVisualStyleBackColor = true;
			buttonNinetyDegrees.Click += buttonNinetyDegrees_Click;
			// 
			// button180Deg
			// 
			button180Deg.Location = new Point(223, 27);
			button180Deg.Name = "button180Deg";
			button180Deg.Size = new Size(75, 23);
			button180Deg.TabIndex = 2;
			button180Deg.Text = "180 Deg";
			button180Deg.UseVisualStyleBackColor = true;
			button180Deg.Click += button180Deg_Click;
			// 
			// button270Deg
			// 
			button270Deg.Location = new Point(304, 27);
			button270Deg.Name = "button270Deg";
			button270Deg.Size = new Size(140, 23);
			button270Deg.TabIndex = 3;
			button270Deg.Text = "90 Deg Counter Clock";
			button270Deg.UseVisualStyleBackColor = true;
			button270Deg.Click += button270Deg_Click;
			// 
			// buttonShowDesktop
			// 
			buttonShowDesktop.Location = new Point(12, 84);
			buttonShowDesktop.Name = "buttonShowDesktop";
			buttonShowDesktop.Size = new Size(131, 23);
			buttonShowDesktop.TabIndex = 4;
			buttonShowDesktop.Text = "ShowDesktop";
			buttonShowDesktop.UseVisualStyleBackColor = true;
			buttonShowDesktop.Click += buttonShowDesktop_Click;
			// 
			// buttonTurnScreensOff
			// 
			buttonTurnScreensOff.Location = new Point(149, 84);
			buttonTurnScreensOff.Name = "buttonTurnScreensOff";
			buttonTurnScreensOff.Size = new Size(160, 23);
			buttonTurnScreensOff.TabIndex = 5;
			buttonTurnScreensOff.Text = "TurnScreensOffAndOn";
			buttonTurnScreensOff.UseVisualStyleBackColor = true;
			buttonTurnScreensOff.Click += buttonTurnScreensOff_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(456, 119);
			Controls.Add(buttonTurnScreensOff);
			Controls.Add(buttonShowDesktop);
			Controls.Add(button270Deg);
			Controls.Add(button180Deg);
			Controls.Add(buttonNinetyDegrees);
			Controls.Add(buttonDefault);
			Name = "Form1";
			Text = "RotateDisplay";
			ResumeLayout(false);
		}

		#endregion

		private Button buttonDefault;
		private Button buttonNinetyDegrees;
		private Button button180Deg;
		private Button button270Deg;
		private Button buttonShowDesktop;
		private Button buttonTurnScreensOff;
	}
}