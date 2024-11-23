using System;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class IntegerCtrl : FilterCtrlBase, IFilterCtrl
	{
		public Object Value
		{
			get => base.IsFilterEnabled ? udValue.Value : (Decimal?)null;
			set
			{
				if(value == null)
				{
					base.IsFilterEnabled = false;
					udValue.Value = 0;
				} else
				{
					base.IsFilterEnabled = true;
					udValue.Value = Convert.ToDecimal(value);
				}
			}
		}
		public Sign? Sign
		{
			get => (Sign)ddlSign.SelectedIndex;
			set
			{
				if(value.HasValue)
					ddlSign.SelectedIndex = (Int32)value.Value;
				else ddlSign.SelectedIndex = 0;
			}
		}
		public IntegerCtrl()
			=> InitializeComponent();
	}
}