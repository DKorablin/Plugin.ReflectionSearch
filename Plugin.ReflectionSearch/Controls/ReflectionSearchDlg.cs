using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Plugin.ReflectionSearch.Bll;
using Plugin.ReflectionSearch.Controls.Filters;

namespace Plugin.ReflectionSearch.Search
{
	internal partial class ReflectionSearchDlg : Form
	{
		private TreeNode _prevNode;
		internal Dictionary<String, SearchFilter> Search
		{
			get; private set;
		} = new Dictionary<String, SearchFilter>();

		public ReflectionSearchDlg(Type root, Dictionary<String, SearchFilter> search)
		{
			InitializeComponent();

			tvHierarchy.DataBind(root);
			if(search != null)
			{
				this.Search = search;
				TreeNode node = null;
				foreach(String key in this.Search.Keys)
				{
					node = tvHierarchy.SelectPath(key);
					if(node != null)
						node.Checked = true;
				}
				if(node != null)
					tvHierarchy.SelectedNode = node;
			}

			bnOk.Enabled = this.Search.Count > 0;
		}

		private void tvHierarchy_AfterSelect(Object sender, TreeViewEventArgs e)
		{
			this._prevNode = null;
			gbFilter.Controls.Clear();

			if(!e.Node.Checked) return;

			Type type = (Type)e.Node.Tag;
			Control ctrl = null;

			String key = tvHierarchy.GetPath(e.Node);

			SearchFilter filter = this.Search.ContainsKey(key)
				? this.Search[key]
				: new SearchFilter();

			switch(type.GetFilterControlType())
			{
			case ControlType.None:
				break;
			case ControlType.Boolean:
				ctrl = new BooleanCtrl();
				break;
			case ControlType.Date:
				ctrl = new DateCtrl();
				break;
			case ControlType.Enum:
				ctrl = new EnumCtrl(type);
				break;
			case ControlType.Integer:
				ctrl = new IntegerCtrl();
				break;
			case ControlType.String:
				ctrl = new StringCtrl();
				break;
			default: throw new NotImplementedException();
			}
			if(ctrl != null)
			{
				IFilterCtrl filterCtrl = (IFilterCtrl)ctrl;
				filterCtrl.FilterName = e.Node.Text;
				filterCtrl.Value = filter.Value;
				filterCtrl.Sign = filter.Sign;

				this._prevNode = e.Node;
				ctrl.Dock = DockStyle.Fill;
				gbFilter.Controls.Add(ctrl);
			}
		}

		private void tvHierarchy_AfterCheck(Object sender, TreeViewEventArgs e)
		{
			if(e.Node.Checked)
			{
				if(((Type)e.Node.Tag).GetFilterControlType() == ControlType.None)
					e.Node.Checked = false;
				else this.tvHierarchy_AfterSelect(sender, e);
			} else
			{
				String key = tvHierarchy.GetPath(e.Node);
				if(this.Search.ContainsKey(key))
					this.Search.Remove(key);
				gbFilter.Controls.Clear();
			}
			bnOk.Enabled = this.Search.Count > 0 || e.Node.Checked;
		}

		private void tvHierarchy_BeforeSelect(Object sender, TreeViewCancelEventArgs e)
		{
			if(gbFilter.Controls.Count == 0)
				return;

			if(this._prevNode != null && this._prevNode.Checked)
			{
				String key = tvHierarchy.GetPath(this._prevNode);
				Type type = (Type)this._prevNode.Tag;

				this.AddSearchFilter(key, type);
			}
		}

		private void bnOk_Click(Object sender, EventArgs e)
		{
			if(tvHierarchy.SelectedNode.Checked)
			{
				String key = tvHierarchy.GetPath();
				Type type = (Type)tvHierarchy.SelectedNode.Tag;

				this.AddSearchFilter(key, type);
			}
		}
		private void AddSearchFilter(String key, Type type)
		{
			IFilterCtrl ctrl = (IFilterCtrl)gbFilter.Controls[0];
			SearchFilter value = new SearchFilter(ctrl.Sign, ctrl.Value, type);

			if(this.Search.ContainsKey(key))
				this.Search[key] = value;
			else
				this.Search.Add(key, value);
		}
	}
}