namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class BooleanCtrl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbValue = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// cbValue
			// 
			this.cbValue.AutoSize = true;
			this.cbValue.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbValue.Location = new System.Drawing.Point(71, 33);
			this.cbValue.Name = "cbValue";
			this.cbValue.Size = new System.Drawing.Size(48, 17);
			this.cbValue.TabIndex = 3;
			this.cbValue.Text = "&True";
			this.cbValue.UseVisualStyleBackColor = true;
			// 
			// BooleanCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbValue);
			this.Name = "BooleanCtrl";
			this.Controls.SetChildIndex(this.cbValue, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cbValue;
	}
}