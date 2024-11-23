namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class StringCtrl
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
			this.txtValue = new System.Windows.Forms.TextBox();
			this.ddlSign = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// txtValue
			// 
			this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue.Location = new System.Drawing.Point(71, 31);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(114, 20);
			this.txtValue.TabIndex = 3;
			// 
			// ddlSign
			// 
			this.ddlSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ddlSign.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ddlSign.FormattingEnabled = true;
			this.ddlSign.Items.AddRange(new object[] {
            "Contains",
            "==",
            "!="});
			this.ddlSign.Location = new System.Drawing.Point(191, 32);
			this.ddlSign.Name = "ddlSign";
			this.ddlSign.Size = new System.Drawing.Size(54, 21);
			this.ddlSign.TabIndex = 5;
			// 
			// StringCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.ddlSign);
			this.Controls.Add(this.txtValue);
			this.Name = "StringCtrl";
			this.Controls.SetChildIndex(this.txtValue, 0);
			this.Controls.SetChildIndex(this.ddlSign, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtValue;
		private System.Windows.Forms.ComboBox ddlSign;
	}
}