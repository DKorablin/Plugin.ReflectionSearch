using System;
using System.Windows.Forms;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls.Filters
{
	internal interface IFilterCtrl
	{
		String FilterName { get; set; }
		Object Value { get; set; }
		Sign? Sign { get; set; }
	}
}