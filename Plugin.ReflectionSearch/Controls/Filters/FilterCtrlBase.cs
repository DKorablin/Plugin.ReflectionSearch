using System;
using System.Windows.Forms;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class FilterCtrlBase : UserControl
	{
		private Boolean _isNullable;
		protected Boolean IsNullable
		{
			get => this._isNullable;
			private set
			{
				this._isNullable = value;
				if(!value)
				{
					cbEnabled.Checked = true;
					cbEnabled.Enabled = false;
				}
			}
		}
		public String FilterName
		{
			get => txtName.Text;
			set
			{
				txtName.Text = value;
				this.IsNullable = value.EndsWith("?") || value.EndsWith("[]");
			}
		}
		protected Boolean IsFilterEnabled
		{
			get => cbEnabled.Checked;
			set
			{
				if(this.IsNullable)
					cbEnabled.Checked = value;
				else cbEnabled.Checked = true;

				this.cbEnabled_CheckedChanged(cbEnabled, EventArgs.Empty);
			}
		}

		public FilterCtrlBase()
			=> InitializeComponent();

		private void cbEnabled_CheckedChanged(Object sender, EventArgs e)
		{
			Boolean isEnabled = this.IsFilterEnabled;
			lblNull.Visible = !isEnabled;
			foreach(Control ctrl in base.Controls)
				if(ctrl != lblName && ctrl != txtName && ctrl != cbEnabled && ctrl != lblNull)
					ctrl.Visible = isEnabled;
		}
	}
}