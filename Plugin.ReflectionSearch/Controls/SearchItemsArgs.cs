using System;
using System.Collections.Generic;
using System.Threading;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch.Controls
{
	internal class SearchItemsArgs
	{
		public Object[] ItemsForSearch { get; }

		public Dictionary<String, SearchFilter> Filters { get; }

		public SearchItemsArgs(Object[] itemsForSearch, Dictionary<String, SearchFilter> filters)
		{
			this.ItemsForSearch = itemsForSearch;
			this.Filters = filters;
		}
	}

	internal class SearchThreadsArgs : SearchItemsArgs
	{
		public enum ThreadResult
		{
			/// <summary>Nothing found</summary>
			None,
			/// <summary>Somethid is found inside object</summary>
			Found,
			/// <summary>Exception occured while searching inside object</summary>
			Exception,
			/// <summary>Unknown or empty object</summary>
			Empty,
		}

		public ThreadResult[] Result;
		public String[] Messages;
		public readonly PanelSearch Panel;
		public readonly ManualResetEvent DoneEvent;

		public SearchThreadsArgs(String[] files, PanelSearch panel, Dictionary<String, SearchFilter> filters, ManualResetEvent doneEvt)
			: base(files, filters)
		{
			this.Result = new ThreadResult[files.Length];
			this.Messages = new String[files.Length];
			this.Panel = panel;
			this.DoneEvent = doneEvt;
		}
	}
}