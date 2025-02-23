namespace Plugin.ReflectionSearch
{
	partial class PanelSearch
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.StatusStrip ssMain;
			System.Windows.Forms.ColumnHeader colPlugin;
			System.Windows.Forms.ToolStripSeparator ResultSeparator1;
			this.tsslStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsslCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsbnSearchFilters = new System.Windows.Forms.ToolStripSplitButton();
			this.tsmiFilterEmpty = new System.Windows.Forms.ToolStripMenuItem();
			this.tsbnSearch = new System.Windows.Forms.ToolStripButton();
			this.bgSearch = new System.ComponentModel.BackgroundWorker();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvPlugins = new System.Windows.Forms.ListView();
			this.lvResult = new AlphaOmega.Windows.Forms.DbListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colReflectedType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colString = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmsResult = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiResultOpenFileLocation = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiResultCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiResultDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.tableFilters = new System.Windows.Forms.TableLayoutPanel();
			ssMain = new System.Windows.Forms.StatusStrip();
			colPlugin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			ResultSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			ssMain.SuspendLayout();
			this.tsMain.SuspendLayout();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.cmsResult.SuspendLayout();
			this.SuspendLayout();
			// 
			// ssMain
			// 
			ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslStatus,
            this.tsslCount});
			ssMain.Location = new System.Drawing.Point(0, 394);
			ssMain.Name = "ssMain";
			ssMain.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
			ssMain.Size = new System.Drawing.Size(444, 26);
			ssMain.SizingGrip = false;
			ssMain.TabIndex = 2;
			// 
			// tsslStatus
			// 
			this.tsslStatus.Name = "tsslStatus";
			this.tsslStatus.Size = new System.Drawing.Size(50, 20);
			this.tsslStatus.Text = "Ready";
			// 
			// tsslCount
			// 
			this.tsslCount.Name = "tsslCount";
			this.tsslCount.Size = new System.Drawing.Size(0, 20);
			// 
			// colPlugin
			// 
			colPlugin.Text = "";
			// 
			// ResultSeparator1
			// 
			ResultSeparator1.Name = "ResultSeparator1";
			ResultSeparator1.Size = new System.Drawing.Size(173, 6);
			// 
			// tsMain
			// 
			this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnSearchFilters,
            this.tsbnSearch});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(269, 27);
			this.tsMain.TabIndex = 0;
			// 
			// tsbnSearchFilters
			// 
			this.tsbnSearchFilters.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnSearchFilters.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFilterEmpty});
			this.tsbnSearchFilters.Image = global::Plugin.ReflectionSearch.Properties.Resources.iconSearchFilter;
			this.tsbnSearchFilters.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnSearchFilters.Name = "tsbnSearchFilters";
			this.tsbnSearchFilters.Size = new System.Drawing.Size(39, 24);
			this.tsbnSearchFilters.Text = "Filters";
			this.tsbnSearchFilters.ButtonClick += new System.EventHandler(this.tsbnSearchFilters_ButtonClick);
			this.tsbnSearchFilters.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsbnSearchFilters_DropDownItemClicked);
			// 
			// tsmiFilterEmpty
			// 
			this.tsmiFilterEmpty.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.tsmiFilterEmpty.Name = "tsmiFilterEmpty";
			this.tsmiFilterEmpty.Size = new System.Drawing.Size(134, 26);
			this.tsmiFilterEmpty.Text = "Empty";
			// 
			// tsbnSearch
			// 
			this.tsbnSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsbnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnSearch.Enabled = false;
			this.tsbnSearch.Image = global::Plugin.ReflectionSearch.Properties.Resources.iconSearch;
			this.tsbnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnSearch.Name = "tsbnSearch";
			this.tsbnSearch.Size = new System.Drawing.Size(29, 24);
			this.tsbnSearch.Text = "Search";
			this.tsbnSearch.ToolTipText = "Search";
			this.tsbnSearch.Click += new System.EventHandler(this.tsbnSearch_Click);
			// 
			// bgSearch
			// 
			this.bgSearch.WorkerReportsProgress = true;
			this.bgSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgSearch_DoWork);
			this.bgSearch.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgSearch_ProgressChanged);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Margin = new System.Windows.Forms.Padding(4);
			this.splitMain.Name = "splitMain";
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvPlugins);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.lvResult);
			this.splitMain.Panel2.Controls.Add(this.tableFilters);
			this.splitMain.Panel2.Controls.Add(this.tsMain);
			this.splitMain.Size = new System.Drawing.Size(444, 394);
			this.splitMain.SplitterDistance = 170;
			this.splitMain.SplitterWidth = 5;
			this.splitMain.TabIndex = 3;
			// 
			// lvPlugins
			// 
			this.lvPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colPlugin});
			this.lvPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvPlugins.FullRowSelect = true;
			this.lvPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvPlugins.HideSelection = false;
			this.lvPlugins.Location = new System.Drawing.Point(0, 0);
			this.lvPlugins.Margin = new System.Windows.Forms.Padding(4);
			this.lvPlugins.MultiSelect = false;
			this.lvPlugins.Name = "lvPlugins";
			this.lvPlugins.Size = new System.Drawing.Size(170, 394);
			this.lvPlugins.TabIndex = 0;
			this.lvPlugins.UseCompatibleStateImageBehavior = false;
			this.lvPlugins.View = System.Windows.Forms.View.Details;
			this.lvPlugins.SelectedIndexChanged += new System.EventHandler(this.lvPlugins_SelectedIndexChanged);
			// 
			// lvResult
			// 
			this.lvResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colReflectedType,
            this.colString});
			this.lvResult.ContextMenuStrip = this.cmsResult;
			this.lvResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvResult.FullRowSelect = true;
			this.lvResult.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvResult.HideSelection = false;
			this.lvResult.Location = new System.Drawing.Point(0, 27);
			this.lvResult.Margin = new System.Windows.Forms.Padding(4);
			this.lvResult.Name = "lvResult";
			this.lvResult.Size = new System.Drawing.Size(269, 367);
			this.lvResult.TabIndex = 1;
			this.lvResult.UseCompatibleStateImageBehavior = false;
			this.lvResult.View = System.Windows.Forms.View.Details;
			this.lvResult.SelectedIndexChanged += new System.EventHandler(this.lvResult_SelectedIndexChanged);
			this.lvResult.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvResult_DragDrop);
			this.lvResult.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvResult_DragEnter);
			this.lvResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvResult_KeyDown);
			// 
			// colName
			// 
			this.colName.Text = "Module";
			this.colName.Width = 86;
			// 
			// colReflectedType
			// 
			this.colReflectedType.DisplayIndex = 2;
			this.colReflectedType.Text = "Type";
			// 
			// colString
			// 
			this.colString.DisplayIndex = 1;
			this.colString.Text = "String";
			// 
			// cmsResult
			// 
			this.cmsResult.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmsResult.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiResultOpenFileLocation,
            this.tsmiResultCopy,
            ResultSeparator1,
            this.tsmiResultDelete});
			this.cmsResult.Name = "cmsResult";
			this.cmsResult.Size = new System.Drawing.Size(177, 82);
			this.cmsResult.Opening += new System.ComponentModel.CancelEventHandler(this.cmsResult_Opening);
			this.cmsResult.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsResult_ItemClicked);
			// 
			// tsmiResultOpenFileLocation
			// 
			this.tsmiResultOpenFileLocation.Name = "tsmiResultOpenFileLocation";
			this.tsmiResultOpenFileLocation.Size = new System.Drawing.Size(176, 24);
			this.tsmiResultOpenFileLocation.Text = "&Show in Folder";
			// 
			// tsmiResultCopy
			// 
			this.tsmiResultCopy.Name = "tsmiResultCopy";
			this.tsmiResultCopy.Size = new System.Drawing.Size(176, 24);
			this.tsmiResultCopy.Text = "&Copy";
			// 
			// tsmiResultDelete
			// 
			this.tsmiResultDelete.Name = "tsmiResultDelete";
			this.tsmiResultDelete.Size = new System.Drawing.Size(176, 24);
			this.tsmiResultDelete.Text = "&Delete";
			// 
			// tableFilters
			// 
			this.tableFilters.AutoSize = true;
			this.tableFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableFilters.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableFilters.Location = new System.Drawing.Point(0, 27);
			this.tableFilters.Name = "tableFilters";
			this.tableFilters.Size = new System.Drawing.Size(269, 0);
			this.tableFilters.TabIndex = 2;
			// 
			// PanelSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Controls.Add(ssMain);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "PanelSearch";
			this.Size = new System.Drawing.Size(444, 420);
			ssMain.ResumeLayout(false);
			ssMain.PerformLayout();
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.Panel2.PerformLayout();
			this.splitMain.ResumeLayout(false);
			this.cmsResult.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private AlphaOmega.Windows.Forms.DbListView lvResult;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colReflectedType;
		private System.Windows.Forms.ColumnHeader colString;
		private System.Windows.Forms.ToolStripSplitButton tsbnSearchFilters;
		private System.Windows.Forms.ToolStripButton tsbnSearch;
		private System.Windows.Forms.ToolStripMenuItem tsmiFilterEmpty;
		private System.Windows.Forms.ToolStripStatusLabel tsslStatus;
		private System.ComponentModel.BackgroundWorker bgSearch;
		private System.Windows.Forms.ToolStripStatusLabel tsslCount;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.ListView lvPlugins;
		private System.Windows.Forms.ContextMenuStrip cmsResult;
		private System.Windows.Forms.ToolStripMenuItem tsmiResultOpenFileLocation;
		private System.Windows.Forms.ToolStripMenuItem tsmiResultCopy;
		private System.Windows.Forms.ToolStripMenuItem tsmiResultDelete;
		private System.Windows.Forms.TableLayoutPanel tableFilters;
	}
}
