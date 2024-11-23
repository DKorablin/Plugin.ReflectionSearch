namespace Plugin.ReflectionSearch.Search
{
	partial class ReflectionSearchDlg
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
			this.gbFilter = new System.Windows.Forms.GroupBox();
			this.bnCancel = new System.Windows.Forms.Button();
			this.bnOk = new System.Windows.Forms.Button();
			this.tvHierarchy = new Plugin.ReflectionSearch.Controls.ReflectionTreeView();
			this.SuspendLayout();
			// 
			// gbFilter
			// 
			this.gbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.gbFilter.Location = new System.Drawing.Point(12, 161);
			this.gbFilter.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.gbFilter.Name = "gbFilter";
			this.gbFilter.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.gbFilter.Size = new System.Drawing.Size(260, 75);
			this.gbFilter.TabIndex = 2;
			this.gbFilter.TabStop = false;
			this.gbFilter.Text = "Filter";
			// 
			// bnCancel
			// 
			this.bnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bnCancel.Location = new System.Drawing.Point(197, 242);
			this.bnCancel.Name = "bnCancel";
			this.bnCancel.Size = new System.Drawing.Size(75, 23);
			this.bnCancel.TabIndex = 3;
			this.bnCancel.Text = "&Cancel";
			this.bnCancel.UseVisualStyleBackColor = true;
			// 
			// bnOk
			// 
			this.bnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.bnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.bnOk.Location = new System.Drawing.Point(116, 242);
			this.bnOk.Name = "bnOk";
			this.bnOk.Size = new System.Drawing.Size(75, 23);
			this.bnOk.TabIndex = 4;
			this.bnOk.Text = "&OK";
			this.bnOk.UseVisualStyleBackColor = true;
			this.bnOk.Click += new System.EventHandler(this.bnOk_Click);
			// 
			// tvHierarchy
			// 
			this.tvHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tvHierarchy.CheckBoxes = true;
			this.tvHierarchy.HideSelection = false;
			this.tvHierarchy.Location = new System.Drawing.Point(12, 12);
			this.tvHierarchy.Name = "tvHierarchy";
			this.tvHierarchy.Size = new System.Drawing.Size(260, 146);
			this.tvHierarchy.TabIndex = 0;
			this.tvHierarchy.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvHierarchy_AfterCheck);
			this.tvHierarchy.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvHierarchy_AfterSelect);
			this.tvHierarchy.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvHierarchy_BeforeSelect);
			// 
			// ReflectionSearchDlg
			// 
			this.AcceptButton = this.bnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.bnCancel;
			this.ClientSize = new System.Drawing.Size(284, 276);
			this.Controls.Add(this.bnOk);
			this.Controls.Add(this.bnCancel);
			this.Controls.Add(this.gbFilter);
			this.Controls.Add(this.tvHierarchy);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(630, 753);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(195, 184);
			this.Name = "ReflectionSearchDlg";
			this.ShowInTaskbar = false;
			this.Text = "Reflection search";
			this.ResumeLayout(false);

		}

		#endregion

		private Plugin.ReflectionSearch.Controls.ReflectionTreeView tvHierarchy;
		private System.Windows.Forms.GroupBox gbFilter;
		private System.Windows.Forms.Button bnCancel;
		private System.Windows.Forms.Button bnOk;

	}
}