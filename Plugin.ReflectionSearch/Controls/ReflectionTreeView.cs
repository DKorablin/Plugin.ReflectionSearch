using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Plugin.ReflectionSearch.Bll;
using Plugin.ReflectionSearch.Search;

namespace Plugin.ReflectionSearch.Controls
{
	internal class ReflectionTreeView : TreeView
	{
		private Assembly RootAssembly { get { return base.Nodes.Count == 0 ? null : ((Type)base.Nodes[0].Tag).Assembly; } }

		public ReflectionTreeView()
			=> base.BeforeExpand += new TreeViewCancelEventHandler(this.ReflectionTreeView_BeforeExpand);

		private void ReflectionTreeView_BeforeExpand(Object sender, TreeViewCancelEventArgs e)
		{
			if(e.Action == TreeViewAction.Expand && e.Node.IsClosedEmptyNode())
			{
				e.Node.Nodes.Clear();
				Type type = (Type)e.Node.Tag;

				foreach(MemberInfo member in type.GetSearchableMembers())
				{
					if(member.MemberType == MemberTypes.Method)
					{
						MethodInfo method = (MethodInfo)member;
						ParameterInfo[] prms = method.GetParameters();
						if(prms.Length == 1)//TODO: Getting the value of enums
							e.Node.Nodes.Add(this.CreateNode(method.GetMemberType(), method.Name + "(" + prms[0].Name + ")"));
						else
							e.Node.Nodes.Add(this.CreateNode(method.GetMemberType(), method.Name + "()"));
					} else
						e.Node.Nodes.Add(this.CreateNode(member.GetMemberType(), member.Name));
				}
			}
		}
		public String GetPath()
			=> GetPath(this.SelectedNode);

		public String GetPath(TreeNode node)
		{
			List<String> result = new List<String>();
			while(node.Parent != null)
			{
				result.Add(node.Text);
				node = node.Parent;
			}
			return String.Join(".", result.ToArray());
		}

		/// <summary>Select a node by path</summary>
		/// <param name="path">Path by which to select a node</param>
		/// <returns>The found tree node</returns>
		public TreeNode SelectPath(String path)
		{
			String[] nodes = path.Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			Int32 index=0;
			TreeNode node = base.Nodes[0];
			while(index < nodes.Length)
			{
				node.Expand();
				base.OnBeforeExpand(new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
				foreach(TreeNode child in node.Nodes)
					if(child.Text == nodes[index])
					{
						index++;
						node = child;
						break;
					}
			}
			return node;
		}

		public void DataBind(Type root)
		{
			TreeNode rootNode = new TreeNode(root.ToString()) { Tag = root, };
			rootNode.Nodes.Add(ReflectionTreeView.CreateEmptyNode());
			base.Nodes.Add(rootNode);
			rootNode.Expand();
		}

		private static TreeNode CreateEmptyNode()
			=> new TreeNode(String.Empty);

		private TreeNode CreateNode(Type type, String name)
		{
			name = type.GetMemberName(name);
			type = type.GetRealType();

			TreeNode result = new TreeNode(name) { Tag = type, };

			if(type.GetFilterControlType() == ControlType.None)
				result.ForeColor = SystemColors.GrayText;

			if(!type.IsEnum && type.Assembly == this.RootAssembly)
			{
				//if(type.Assembly == this.RootAssembly)
					result.Nodes.Add(ReflectionTreeView.CreateEmptyNode());
				/*else if(type.IsGenericType)
				{
					Type[] types = type.GetGenericArguments();
					if(types.Length == 1 && types[0].Assembly == this.RootAssembly)
						result.Nodes.Add(ReflectionTreeView.CreateEmptyNode());
				}*/
			}

			return result;
		}
	}
}