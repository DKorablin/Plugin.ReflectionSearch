using System;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal partial class EnumCtrl : FilterCtrlBase, IFilterCtrl
	{
		public Object Value
		{
			get => base.IsFilterEnabled ? ddlValue.SelectedItem : null;
			set
			{
				if(value == null)
				{
					base.IsFilterEnabled = false;
					ddlValue.SelectedIndex = -1;
				} else
				{
					base.IsFilterEnabled = true;
					ddlValue.SelectedItem = value;
				}
			}
		}

		public Sign? Sign { get => null; set { } }

		public EnumCtrl(Type type)
		{
			InitializeComponent();
			ddlValue.Items.Clear();
			foreach(Object item in Enum.GetValues(type))
				ddlValue.Items.Add(item);
		}
	}
}