using System;
using System.Collections.Generic;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch
{
	public class PanelSearchSettings
	{
		public Dictionary<String, Dictionary<String, SearchFilter>> PluginsFilters { get; set; } = new Dictionary<String, Dictionary<String, SearchFilter>>();
	}
}