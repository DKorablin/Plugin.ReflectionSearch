using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AlphaOmega.Windows.Forms;
using Plugin.ReflectionSearch.Bll;
using Plugin.ReflectionSearch.Controls;
using Plugin.ReflectionSearch.Search;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.ReflectionSearch
{
	public partial class PanelSearch : UserControl
	{
		private const String Caption = "Reflection Search";
		private enum ThreadResult
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

		private class ThreadArrayArgs
		{
			public ThreadResult[] Result;
			public String[] Messages;
			public readonly String[] Files;
			public readonly Dictionary<String, SearchFilter> Filters;
			public readonly PanelSearch Panel;
			public readonly ManualResetEvent DoneEvent;

			public ThreadArrayArgs(String[] files, PanelSearch panel, Dictionary<String,SearchFilter> filters, ManualResetEvent doneEvt)
			{
				this.Result = new ThreadResult[files.Length];
				this.Messages = new String[files.Length];
				this.Files = files;
				this.Filters = filters;
				this.Panel = panel;
				this.DoneEvent = doneEvt;
			}
		}

		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;
		private IWindow Window => (IWindow)base.Parent;

		//private static Int32 ThreadPoolCount = 0;
		private Int32 ThreadsCount = 0;
		private UInt64 _itemsMax = 0;
		private UInt64 _itemsFetched = 0;
		private volatile Boolean ThreadsTerminate = false;
		private SystemImageList _smallImageList;

		private Dictionary<String, Dictionary<String, SearchFilter>> _pluginsFilters = new Dictionary<String, Dictionary<String, SearchFilter>>();
		private Dictionary<String,SearchFilter> ReflectionSearch
		{
			get
			{
				ListViewItem selecteditem = lvPlugins.SelectedItems.Count == 0 ? null : lvPlugins.SelectedItems[0];
				if(selecteditem == null)
					return new Dictionary<String, SearchFilter>();

				if(!this._pluginsFilters.TryGetValue(selecteditem.Text, out Dictionary<String, SearchFilter> result))
				{
					result = new Dictionary<String, SearchFilter>();
					this._pluginsFilters.Add(selecteditem.Text, result);
				}
				return result;
			}
			set
			{
				ListViewItem selectedItem = lvPlugins.SelectedItems.Count == 0 ? null : lvPlugins.SelectedItems[0];
				if(selectedItem == null) return;

				if(this._pluginsFilters.ContainsKey(selectedItem.Text))
					this._pluginsFilters[selectedItem.Text] = value;
				else
					this._pluginsFilters.Add(selectedItem.Text, value);
			}
		}

		public PanelSearch()
		{
			this.InitializeComponent();
			this._smallImageList = new SystemImageList(SystemImageListSize.SmallIcons);
			SystemImageListHelper.SetImageList(lvResult, this._smallImageList, false);
		}

		protected override void OnCreateControl()
		{
			this.Window.Caption = PanelSearch.Caption;
			this.Window.SetDockAreas(DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float | DockAreas.Document);

			base.OnCreateControl();

			List<ListViewItem> itemsToAdd = new List<ListViewItem>();
			foreach(IPluginDescription plugin in this.Plugin.HostWindows.Plugins)
				if(SearchPluginWrapper.IsConsumable(plugin))
				{
					ListViewItem item = new ListViewItem(plugin.Name)
					{
						Tag = plugin.ID
					};
					itemsToAdd.Add(item);
				}

			lvPlugins.Items.AddRange(itemsToAdd.ToArray());
			lvPlugins.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			if(itemsToAdd.Count > 0)
				lvPlugins.Items[0].Selected = true;
		}

		private void lvPlugins_SelectedIndexChanged(Object sender, EventArgs e)
		{
			ListViewItem selectedItem = lvPlugins.SelectedItems.Count == 0 ? null : lvPlugins.SelectedItems[0];
			tsMain.Enabled = selectedItem != null;
			lvResult.Items.Clear();
			this.InvalidateFilters();

			this.Window.Caption = selectedItem == null
				? PanelSearch.Caption
				: PanelSearch.Caption + " - " + selectedItem.Text;
		}

		private SearchPluginWrapper GetSelectedPlugin()
		{
			if(lvPlugins.InvokeRequired)
				return (SearchPluginWrapper)lvPlugins.Invoke((Func<SearchPluginWrapper>)delegate { return GetSelectedPlugin(); });

			ListViewItem item = lvPlugins.SelectedItems.Count == 0 ? null : lvPlugins.SelectedItems[0];
			IPluginDescription plugin = this.Plugin.HostWindows.Plugins[(String)item.Tag];
			return new SearchPluginWrapper(plugin);
		}

		private void tsbnSearchFilters_ButtonClick(Object sender, EventArgs e)
		{
			SearchPluginWrapper plugin = this.GetSelectedPlugin();
			Type root = plugin.GetEntityType();
			using(ReflectionSearchDlg dlg = new ReflectionSearchDlg(root, this.ReflectionSearch))
				if(dlg.ShowDialog() == DialogResult.OK)
				{
					this.ReflectionSearch = dlg.Search;
					this.InvalidateFilters();
				}
		}

		private void tsbnSearchFilters_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem != tsmiFilterEmpty)
			{
				String filterKey = (String)e.ClickedItem.Tag;
				this.ReflectionSearch.Remove(filterKey);
				this.InvalidateFilters();
			}
		}

		private void MessageCtrl_OnClosed(Object sender, ControlEventArgs e)
		{
			String filterKey = (String)e.Control.Tag;
			this.ReflectionSearch.Remove(filterKey);
			this.InvalidateFilters();
		}

		private void InvalidateFilters()
		{
			var filters = this.ReflectionSearch;

			this.SuspendLayout();
			try
			{
				if(filters.Count > 0)
				{
					if(tsbnSearchFilters.DropDownItems.Count == 1 && tsbnSearchFilters.DropDownItems[0] == tsmiFilterEmpty)
						tsbnSearchFilters.DropDownItems.Remove(tsmiFilterEmpty);

					foreach(var filter in filters)
						if(!tableFilters.Controls.OfType<MessageCtrl>().Any(c => String.Equals(filter.Key, c.Tag)))
						{
							MessageCtrl newCtrl = new MessageCtrl()
							{
								Tag = filter.Key,
							};
							newCtrl.OnClosed += MessageCtrl_OnClosed;
							newCtrl.ShowMessage(MessageCtrl.StatusMessageType.None, filter.Key + ": " + filter.Value.AsString());
							tableFilters.Controls.Add(newCtrl);

							ToolStripMenuItem menuItem = new ToolStripMenuItem()
							{
								Text = String.Join(" ", new String[] { filter.Key, filter.Value.AsString(), }),
								Tag = filter.Key,
							};
							if(filter.Value.Value == null)
								menuItem.SetNull();
							tsbnSearchFilters.DropDownItems.Add(menuItem);
						}

					for(Int32 loop = tableFilters.Controls.Count - 1; loop >= 0; loop--)
						if(!filters.ContainsKey((String)tableFilters.Controls[loop].Tag))
						{
							tableFilters.Controls.RemoveAt(loop);
							tsbnSearchFilters.DropDownItems.RemoveAt(loop);
						}
				}

				if(filters.Count == 0)
				{
					lvResult.AllowDrop = false;
					tsbnSearch.Enabled = false;
					tableFilters.Controls.Clear();
					tsbnSearchFilters.DropDownItems.Clear();
					tsbnSearchFilters.DropDownItems.Add(tsmiFilterEmpty);
				} else
				{
					lvResult.AllowDrop = true;
					tsbnSearch.Enabled = true;
				}
			} finally
			{
				this.ResumeLayout(false);
			}
		}

		private void tsbnSearch_Click(Object sender, EventArgs e)
		{
			if(bgSearch.IsBusy)
			{
				if(!tsbnSearch.Checked)
				{
					tsbnSearch.Checked = false;
					this.ThreadsTerminate = true;
				}
				return;
			}

			try
			{
				this.ThreadsCount = lvResult.GetVisibleRowsCount();//Кол-во одновременных потоков
				this.ThreadsTerminate = false;

				SearchPluginWrapper plugin = this.GetSelectedPlugin();
				if(plugin != null)
				{
					Controls.FolderBrowserDialog2 dlg = new Controls.FolderBrowserDialog2();
					if(dlg.ShowDialog(this.Handle) == true)
					{
						Object[] items = plugin.GetSearchObjects(dlg.ResultPath);
						if(items != null && items.Length > 0)
							bgSearch.RunWorkerAsync(items);
					}
				}
			} catch(Exception)
			{//TODO: Add fatal exception skipping
				this.UnlockControls();
				throw;
			}
		}

		private ListViewItem CreateListItem(ThreadArrayArgs args, Int32 index)
		{
			ListViewItem result = new ListViewItem();
			String[] subItems = Array.ConvertAll(new String[lvResult.Columns.Count], (String) => { return String.Empty; });
			result.SubItems.AddRange(subItems);

			String filePath = args.Files[index];
			result.SubItems[colName.Index].Text = filePath;
			result.ImageIndex = this._smallImageList.IconIndex(filePath);

			switch(args.Result[index])
			{
			case ThreadResult.Exception:
				result.SetException();
				result.SubItems[colString.Index].Text = args.Messages[index];
				break;
			case ThreadResult.Empty:
				result.SetNull();
				break;
			}
			return result;
		}

		private static void SearchAssemblyThreads(Object state)
		{
			ThreadArrayArgs args = (ThreadArrayArgs)state;
			try
			{
				PanelSearch pnl = args.Panel;
				SearchEngine engine = new SearchEngine(pnl.ReflectionSearch);
				SearchPluginWrapper plugin = args.Panel.GetSelectedPlugin();
				for(Int32 loop = 0; loop < args.Files.Length; loop++)
				{
					if(pnl.ThreadsTerminate == true)
						break;

					Object searchInstance = null;
					String filePath = args.Files[loop];
					try
					{
						FileInfo info = new FileInfo(filePath);
						if(info.Length == 0)
						{
							args.Result[loop] = ThreadResult.Empty;
							continue;
						}
						searchInstance = plugin.CreateEntityInstance(filePath);

						if(searchInstance == null)
							args.Result[loop] = ThreadResult.None;
						else if(engine.StartSearch(searchInstance))
							args.Result[loop] = ThreadResult.Found;
						else
							args.Result[loop] = ThreadResult.None;

						if(loop % 10 == 0)//TODO: Last item will not count...
							pnl.bgSearch.ReportProgress(10);
					} catch(Exception exc)
					{
						args.Result[loop] = ThreadResult.Exception;
						args.Messages[loop] = exc.Message;
						exc.Data.Add("File", filePath);
						args.Panel.Plugin.Trace.TraceData(TraceEventType.Error, 10, exc);
					} finally
					{
						IDisposable disp = searchInstance == null ? null : searchInstance as IDisposable;
						disp?.Dispose();
					}
				}
			} finally
			{
				//if(Interlocked.Decrement(ref PanelSearch.ThreadPoolCount) == 0)
				args.DoneEvent.Set();
			}
		}

		private void bgSearch_DoWork(Object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			List<ListViewItem> itemsToAdd = new List<ListViewItem>();
			try
			{
				String[] filePath = (String[])e.Argument;

				bgSearch.ReportProgress(0, filePath.Length);

				Int32 numberInThread = (filePath.Length / this.ThreadsCount) + 1;
				ManualResetEvent[] doneEventEx = new ManualResetEvent[this.ThreadsCount];//Семафоры завершения события
				ThreadArrayArgs[] args = new ThreadArrayArgs[this.ThreadsCount];//Аргументы для потоков
				Int32 threadIndex = 0;
				Int32 arrayIndex = 0;
				while(arrayIndex < filePath.Length)
				{
					if(filePath.Length - arrayIndex < numberInThread)
						numberInThread = filePath.Length - arrayIndex;

					if(numberInThread > 0)
					{
						doneEventEx[threadIndex] = new ManualResetEvent(false);
						String[] filesInThread = new String[numberInThread];
						Array.Copy(filePath, arrayIndex, filesInThread, 0, filesInThread.Length);
						args[threadIndex] = new ThreadArrayArgs(filesInThread, this, this.ReflectionSearch, doneEventEx[threadIndex]);
						ThreadPool.QueueUserWorkItem(new WaitCallback(SearchAssemblyThreads), args[threadIndex]);
						threadIndex++;
						arrayIndex += numberInThread;
					}
				}
				//Ожидание завершения пула потоков
				for(Int32 eventLoop = 0;eventLoop < doneEventEx.Length;eventLoop++)
					if(doneEventEx[eventLoop] != null//Накопилось только на один поток
						&& doneEventEx[eventLoop].WaitOne())
					{
						ThreadArrayArgs a = args[eventLoop];
						for(Int32 itemLoop = 0;itemLoop < a.Files.Length;itemLoop++)
							if(a.Result[itemLoop] != ThreadResult.None)
								itemsToAdd.Add(this.CreateListItem(a, itemLoop));
					} else
						return;
			} finally
			{
				bgSearch.ReportProgress(100, itemsToAdd.ToArray());
			}
		}

		private void LockControls()
		{
			base.Cursor = Cursors.WaitCursor;
			lvPlugins.Enabled = false;
			tsbnSearchFilters.Enabled = false;
			tsbnSearch.Checked = true;
			lvResult.Items.Clear();
		}

		private void UnlockControls()
		{
			base.Cursor = Cursors.Default;
			lvPlugins.Enabled = true;
			tsbnSearchFilters.Enabled = true;
			tsbnSearch.Checked = false;
			tsslStatus.Text = "Ready";
		}

		private void bgSearch_ProgressChanged(Object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			if(e.ProgressPercentage == 0)
			{
				this.LockControls();

				this._itemsFetched = 0;
				this._itemsMax = (UInt64)Convert.ToInt64(e.UserState);
				tsslStatus.Text = $"Searching... {this._itemsMax:n0}/";

			} else if(e.ProgressPercentage == 100)
			{
				this.UnlockControls();
				lvResult.Items.AddRange((ListViewItem[])e.UserState);
				lvResult.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			} else
			{
				this._itemsFetched += (UInt64)e.ProgressPercentage;
				tsslCount.Text = this._itemsFetched.ToString("n0");
			}
		}

		private void lvResult_DragEnter(Object sender, DragEventArgs e)
			=> e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Move : DragDropEffects.None;

		private void lvResult_DragDrop(Object sender, DragEventArgs e)
		{
			if(bgSearch.IsBusy)
				while(bgSearch.IsBusy)
					System.Threading.Thread.Sleep(100);

			this.ThreadsCount = lvResult.GetVisibleRowsCount();//Кол-во одновременных потоков
			bgSearch.RunWorkerAsync((String[])e.Data.GetData(DataFormats.FileDrop));
		}

		private void cmsResult_Opening(Object sender, System.ComponentModel.CancelEventArgs e)
			=> e.Cancel = lvResult.SelectedItems.Count == 0;

		private void cmsResult_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiResultCopy)
			{
				List<String> files = new List<String>(lvResult.SelectedItems.Count);
				foreach(ListViewItem item in lvResult.SelectedItems)
					files.Add(item.SubItems[colName.Index].Text);

				Clipboard.SetText(String.Join(Environment.NewLine, files.ToArray()));
			} else if(e.ClickedItem == tsmiResultDelete)
			{
				if(MessageBox.Show("Are you shure you want to remove selected items from file system?", this.Window.Caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					while(lvResult.SelectedItems.Count > 0)
					{
						ListViewItem item = lvResult.SelectedItems[0];
						File.Delete(item.SubItems[colName.Index].Text);
						item.Remove();
					}
			} else if(e.ClickedItem == tsmiResultOpenFileLocation)
			{
				List<ListViewItem> itemsToOpen = new List<ListViewItem>(lvResult.SelectedItems.Count);
				foreach(ListViewItem item in lvResult.SelectedItems)
					itemsToOpen.Add(item);

				for(Int32 loop = itemsToOpen.Count - 1; loop >= 0; loop--)
				{
					String filePath = itemsToOpen[loop].SubItems[colName.Index].Text;
					if(File.Exists(filePath))
						Shell32.OpenFolderAndSelectItem(filePath);
					else
						itemsToOpen[loop].Remove();
				}
			}
		}

		private void lvResult_KeyDown(Object sender, KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
			case Keys.C:
				if(e.Control && !e.Alt)
				{
					this.cmsResult_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiResultCopy));
					e.Handled = true;
				}
				break;
			case Keys.Delete:
				this.cmsResult_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiResultDelete));
				e.Handled = true;
				break;
			case Keys.Return:
				if(lvResult.SelectedItems.Count > 0)
				{
					this.cmsResult_ItemClicked(sender, new ToolStripItemClickedEventArgs(tsmiResultOpenFileLocation));
					e.Handled = true;
				}
				break;
			}
		}
	}
}