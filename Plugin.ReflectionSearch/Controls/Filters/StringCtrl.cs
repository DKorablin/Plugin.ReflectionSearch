using System;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class StringCtrl : FilterCtrlBase, IFilterCtrl
	{
		public Object Value
		{
			get => base.IsFilterEnabled ? txtValue.Text : null;
			set
			{
				if(value == null)
				{
					base.IsFilterEnabled = false;
					txtValue.Text = String.Empty;
				} else
				{
					base.IsFilterEnabled = true;
					txtValue.Text = (String)value;
				}
			}
		}
		public Sign? Sign
		{
			get => ddlSign.SelectedIndex == 0
				? Bll.Sign.Contains
				: (Sign)ddlSign.SelectedIndex-1;
			set
			{
				if(value.HasValue)
					ddlSign.SelectedIndex = value.Value == Bll.Sign.Contains
						? 0
						: (Int32)value.Value + 1;
				else
					ddlSign.SelectedIndex = 0;
			}
		}

		public StringCtrl()
			=> InitializeComponent();
	}
}