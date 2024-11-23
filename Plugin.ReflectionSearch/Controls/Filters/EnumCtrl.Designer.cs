namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class EnumCtrl
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
			this.ddlValue = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// ddlValue
			// 
			this.ddlValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ddlValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlValue.FormattingEnabled = true;
			this.ddlValue.Location = new System.Drawing.Point(71, 31);
			this.ddlValue.Name = "ddlValue";
			this.ddlValue.Size = new System.Drawing.Size(174, 21);
			this.ddlValue.Sorted = true;
			this.ddlValue.TabIndex = 3;
			// 
			// EnumCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ddlValue);
			this.Name = "EnumCtrl";
			this.Controls.SetChildIndex(this.ddlValue, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox ddlValue;
	}
}
