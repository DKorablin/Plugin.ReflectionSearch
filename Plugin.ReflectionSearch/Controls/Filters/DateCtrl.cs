using System;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class DateCtrl : FilterCtrlBase, IFilterCtrl
	{
		public Object Value
		{
			get => base.IsFilterEnabled ? dtValue.Value : (DateTime?)null;
			set
			{
				if(value == null)
				{
					base.IsFilterEnabled = false;
					dtValue.Value = DateTime.Now;
				} else
				{
					base.IsFilterEnabled = true;
					dtValue.Value = (DateTime)value;
				}
			}
		}

		public Sign? Sign
		{
			get => (Sign)ddlSign.SelectedIndex;
			set => ddlSign.SelectedIndex = value.HasValue ? (Int32)value.Value : 0;
		}
		public DateCtrl()
			=> this.InitializeComponent();
	}
}