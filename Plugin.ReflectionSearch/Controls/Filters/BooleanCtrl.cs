using System;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class BooleanCtrl : FilterCtrlBase, IFilterCtrl
	{
		public Object Value
		{
			get => base.IsFilterEnabled ? cbValue.Checked : (Boolean?)null;
			set
			{
				if(value == null)
				{
					base.IsFilterEnabled = false;
					cbValue.Checked = false;
				} else
				{
					base.IsFilterEnabled = true;
					cbValue.Checked = (Boolean)value;
				}
			}
		}
		public Sign? Sign { get => null; set { _ = value; } }

		public BooleanCtrl()
			=> this.InitializeComponent();
	}
}