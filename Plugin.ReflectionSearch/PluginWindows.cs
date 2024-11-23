using System;
using SAL.Flatbed;
using System.Diagnostics;
using SAL.Windows;
using System.Collections.Generic;
using Plugin.ReflectionSearch.Bll;

namespace Plugin.ReflectionSearch
{
	public class PluginWindows : IPlugin
	{
		private TraceSource _trace;
		private Dictionary<String, DockState> _documentTypes;
		private IMenuItem _menuSearch;
		private IMenuItem _menuSearchReflection;

		internal TraceSource Trace => this._trace ?? (this._trace = PluginWindows.CreateTraceSource<PluginWindows>());

		internal IHostWindows HostWindows { get; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(PanelSearch).ToString(), DockState.Document },
					};
				return this._documentTypes;
			}
		}

		public PluginWindows(IHostWindows hostWindows)
			=> this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));

		public IWindow GetPluginControl(String typeName, Object args)
			=> this.CreateWindow(typeName, false, args);

		/// <summary>Search object by enumerating all members of <paramref name="target"/> and searching for object by <paramref name="path"/> and comparing it with <paramref name="value"/></summary>
		/// <param name="target">Target object where instance of value will be searched</param>
		/// <param name="path">FullName of object in the target</param>
		/// <param name="sign">Valid values: Equals, NotEquals, More, MoreOrEquals, Less, LessOrEquals, Contains</param>
		/// <param name="value">Value to find</param>
		/// <returns>Search succedded</returns>
		public Boolean Search(Object target, String path, String sign, Object value)
		{
			_ = target ?? throw new ArgumentNullException(nameof(target));
			if(String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			Sign realSign = (Sign)Enum.Parse(typeof(Sign),sign);
			SearchFilter filter = new SearchFilter(realSign, value);

			SearchEngine engine = new SearchEngine(path, filter);

			return engine.StartSearch(target);
		}

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IMenuItem menuSearch = this.HostWindows.MainMenu.FindMenuItem("Search");
			if(menuSearch == null)
			{
				this._menuSearch = menuSearch = this.HostWindows.MainMenu.Create("Search");
				menuSearch.Name = "Search";
				this.HostWindows.MainMenu.Items.Insert(0, menuSearch);
			}

			this._menuSearchReflection = menuSearch.Create("Reflection");
			this._menuSearchReflection.Name = "Search.Reflection";
			this._menuSearchReflection.Click += (sender, e) => { this.CreateWindow(typeof(PanelSearch).ToString(), true); };

			menuSearch.Items.Add(this._menuSearchReflection);
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this._menuSearchReflection != null)
				this.HostWindows.MainMenu.Items.Remove(this._menuSearchReflection);
			if(this._menuSearch != null)
				this.HostWindows.MainMenu.Items.Remove(this._menuSearch);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}