namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class IntegerCtrl
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
			this.ddlSign = new System.Windows.Forms.ComboBox();
			this.udValue = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.udValue)).BeginInit();
			this.SuspendLayout();
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
			this.ddlSign.TabIndex = 4;
			// 
			// udValue
			// 
			this.udValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.udValue.Location = new System.Drawing.Point(71, 32);
			this.udValue.Name = "udValue";
			this.udValue.Size = new System.Drawing.Size(114, 20);
			this.udValue.TabIndex = 3;
			// 
			// IntegerCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.udValue);
			this.Controls.Add(this.ddlSign);
			this.Name = "IntegerCtrl";
			this.Controls.SetChildIndex(this.ddlSign, 0);
			this.Controls.SetChildIndex(this.udValue, 0);
			((System.ComponentModel.ISupportInitialize)(this.udValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox ddlSign;
		private System.Windows.Forms.NumericUpDown udValue;
	}
}
