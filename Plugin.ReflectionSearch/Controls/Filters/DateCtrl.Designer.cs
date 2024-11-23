namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class DateCtrl
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
			this.dtValue = new System.Windows.Forms.DateTimePicker();
			this.ddlSign = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// dtValue
			// 
			this.dtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.dtValue.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.dtValue.Location = new System.Drawing.Point(74, 32);
			this.dtValue.Name = "dtValue";
			this.dtValue.Size = new System.Drawing.Size(111, 20);
			this.dtValue.TabIndex = 4;
			// 
			// ddlSign
			// 
			this.ddlSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlSign.FormattingEnabled = true;
			this.ddlSign.Items.AddRange(new object[] {
            "==",
            "!=",
            ">",
            ">=",
            "<",
            "<="});
			this.ddlSign.Location = new System.Drawing.Point(191, 32);
			this.ddlSign.Name = "ddlSign";
			this.ddlSign.Size = new System.Drawing.Size(54, 21);
			this.ddlSign.TabIndex = 5;
			// 
			// DateCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ddlSign);
			this.Controls.Add(this.dtValue);
			this.Name = "DateCtrl";
			this.Controls.SetChildIndex(this.dtValue, 0);
			this.Controls.SetChildIndex(this.ddlSign, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DateTimePicker dtValue;
		private System.Windows.Forms.ComboBox ddlSign;
	}
}
