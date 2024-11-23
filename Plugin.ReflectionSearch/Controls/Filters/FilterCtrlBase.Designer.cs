namespace Plugin.ReflectionSearch.Controls.Filters
{
	partial class FilterCtrlBase
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
			this.cbEnabled = new System.Windows.Forms.CheckBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.lblNull = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cbEnabled
			// 
			this.cbEnabled.AutoSize = true;
			this.cbEnabled.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.cbEnabled.Location = new System.Drawing.Point(3, 34);
			this.cbEnabled.Name = "cbEnabled";
			this.cbEnabled.Size = new System.Drawing.Size(62, 17);
			this.cbEnabled.TabIndex = 2;
			this.cbEnabled.Text = "&Value:";
			this.cbEnabled.UseVisualStyleBackColor = true;
			this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtName.Location = new System.Drawing.Point(74, 13);
			this.txtName.Name = "txtName";
			this.txtName.ReadOnly = true;
			this.txtName.Size = new System.Drawing.Size(171, 13);
			this.txtName.TabIndex = 1;
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblName.Location = new System.Drawing.Point(3, 13);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(43, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "&Name:";
			// 
			// lblNull
			// 
			this.lblNull.AutoSize = true;
			this.lblNull.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblNull.ForeColor = System.Drawing.SystemColors.GrayText;
			this.lblNull.Location = new System.Drawing.Point(71, 35);
			this.lblNull.Name = "lblNull";
			this.lblNull.Size = new System.Drawing.Size(47, 13);
			this.lblNull.TabIndex = 3;
			this.lblNull.Text = "<null>";
			this.lblNull.Visible = false;
			// 
			// FilterCtrlBase
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblNull);
			this.Controls.Add(this.cbEnabled);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.lblName);
			this.Name = "FilterCtrlBase";
			this.Size = new System.Drawing.Size(248, 64);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckBox cbEnabled;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblNull;
	}
}
