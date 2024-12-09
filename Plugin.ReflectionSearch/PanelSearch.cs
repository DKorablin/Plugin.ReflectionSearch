using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using AlphaOmega.Windows.Forms;
using Plugin.ReflectionSearch.Bll;
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
			/// <summary>Не найдено</summary>
			None,
			/// <summary>Найдено</summary>
			Found,
			/// <summary>Исключение</summary>
			Exception,
		}

		private class ThreadArrayArgs
		{
			public ThreadResult[] Result;
			public String[] Messages;
			public readonly String[] Files;
			public readonly PanelSearch Panel;
			public readonly ManualResetEvent DoneEvent;

			public ThreadArrayArgs(String[] files, PanelSearch panel, ManualResetEvent doneEvt)
			{
				this.Result = new ThreadResult[files.Length];
				this.Messages = new String[files.Length];
				this.Files = files;
				this.Panel = panel;
				this.DoneEvent = doneEvt;
			}
		}
		//private static Int32 ThreadPoolCount = 0;
		private Int32 ThreadsCount = 0;
		private UInt64 _itemsMax = 0;
		private UInt64 _itemsFetched = 0;
		private volatile Boolean ThreadsTerminate = false;
		private Dictionary<String, SearchFilter> _reflectionSearch;
		private SystemImageList _smallImageList;
		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;
		private IWindow Window => (IWindow)base.Parent;

		public PanelSearch()
		{
			InitializeComponent();
			_smallImageList = new SystemImageList(SystemImageListSize.SmallIcons);
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
			this._reflectionSearch = null;

			this.Window.Caption = selectedItem == null
				? PanelSearch.Caption
				: String.Join(" - ", new String[] { PanelSearch.Caption, selectedItem.Text, });
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
			using(ReflectionSearchDlg dlg = new ReflectionSearchDlg(root, this._reflectionSearch))
				if(dlg.ShowDialog() == DialogResult.OK)
				{
					this._reflectionSearch = dlg.Search;
					lvResult.AllowDrop = tsbnSearch.Enabled = this._reflectionSearch.Count > 0;
				}
		}

		private void tsbnSearchFilters_DropDownOpening(Object sender, EventArgs e)
		{
			tsbnSearchFilters.DropDownItems.Clear();
			if(this._reflectionSearch==null || this._reflectionSearch.Count == 0)
				tsbnSearchFilters.DropDownItems.Add(tsmiFilterEmpty);
			else
			{
				foreach(KeyValuePair<String, SearchFilter> item in this._reflectionSearch)
				{
					ToolStripMenuItem menuItem = new ToolStripMenuItem()
					{
						Text = String.Join(" ", new String[] { item.Key, item.Value.AsString(), }),
						Tag = item.Key,
					};
					if(item.Value.Value == null)
						menuItem.SetNull();
					tsbnSearchFilters.DropDownItems.Add(menuItem);
				}
			}
		}

		private void tsbnSearchFilters_DropDownItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem != tsmiFilterEmpty)
			{
				String key = (String)e.ClickedItem.Tag;
				if(this._reflectionSearch.Remove(key))
					tsbnSearchFilters.DropDownItems.Remove(e.ClickedItem);

				if(this._reflectionSearch.Count == 0)
				{
					lvResult.AllowDrop = false;
					tsbnSearch.Enabled = false;
					tsbnSearchFilters.DropDownItems.Add(tsmiFilterEmpty);
				} else
				{
					lvResult.AllowDrop = true;
					tsbnSearch.Enabled = true;
				}
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

			if(args.Result[index] == ThreadResult.Exception)
			{
				result.SetException();
				result.SubItems[colString.Index].Text = args.Messages[index];
			}
			return result;
		}

		private static void SearchAssemblyThreads(Object state)
		{
			ThreadArrayArgs args = (ThreadArrayArgs)state;
			try
			{
				PanelSearch pnl = args.Panel;
				SearchEngine engine = new SearchEngine(pnl._reflectionSearch);
				SearchPluginWrapper plugin = args.Panel.GetSelectedPlugin();
				for(Int32 loop = 0; loop < args.Files.Length; loop++)
				{
					if(pnl.ThreadsTerminate == true)
						break;

					Object searchInstance = null;
					try
					{
						FileInfo info = new FileInfo(args.Files[loop]);
						if(info.Length == 0)
						{
							args.Result[loop] = ThreadResult.Exception;
							args.Messages[loop] = "File length 0 bytes";
							continue;
						}
						searchInstance = plugin.CreateEntityInstance(args.Files[loop]);

						if(searchInstance == null)//TODO: Wee need to check for files with empty length
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
						exc.Data.Add("File", args.Files[loop]);
						args.Panel.Plugin.Trace.TraceData(TraceEventType.Error, 10, exc);
					} finally
					{
						IDisposable disp = searchInstance == null ? null : searchInstance as IDisposable;
						if(disp != null)
							disp.Dispose();
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
						args[threadIndex] = new ThreadArrayArgs(filesInThread, this, doneEventEx[threadIndex]);
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