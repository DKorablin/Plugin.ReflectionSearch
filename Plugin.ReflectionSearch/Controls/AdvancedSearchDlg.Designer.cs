namespace Plugin.ReflectionSearch.Controls
{
	partial class AdvancedSearchDlg
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bnCancel = new System.Windows.Forms.Button();
			this.bnOk = new System.Windows.Forms.Button();
			this.gridProperties = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// bnCancel
			// 
			this.bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bnCancel.Location = new System.Drawing.Point(207, 231);
			this.bnCancel.Name = "bnCancel";
			this.bnCancel.Size = new System.Drawing.Size(75, 23);
			this.bnCancel.TabIndex = 0;
			this.bnCancel.Text = "&Cancel";
			this.bnCancel.UseVisualStyleBackColor = true;
			// 
			// bnOk
			// 
			this.bnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bnOk.Location = new System.Drawing.Point(126, 231);
			this.bnOk.Name = "bnOk";
			this.bnOk.Size = new System.Drawing.Size(75, 23);
			this.bnOk.TabIndex = 1;
			this.bnOk.Text = "OK";
			this.bnOk.UseVisualStyleBackColor = true;
			// 
			// gridProperties
			// 
			this.gridProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridProperties.Location = new System.Drawing.Point(12, 12);
			this.gridProperties.Name = "gridProperties";
			this.gridProperties.Size = new System.Drawing.Size(270, 213);
			this.gridProperties.TabIndex = 0;
			// 
			// AdvancedSearchDlg
			// 
			this.AcceptButton = this.bnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bnCancel;
			this.ClientSize = new System.Drawing.Size(294, 266);
			this.Controls.Add(this.gridProperties);
			this.Controls.Add(this.bnOk);
			this.Controls.Add(this.bnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(640, 480);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(196, 142);
			this.Name = "AdvancedSearchDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Advanced search";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button bnCancel;
		private System.Windows.Forms.Button bnOk;
		private System.Windows.Forms.PropertyGrid gridProperties;
	}
}