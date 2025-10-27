using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.ReflectionSearch.Bll
{
	internal static class NodeExtender
	{
		internal static readonly Color NullColor = Color.Gray;
		internal static readonly Color ExceptionColor = Color.Red;
		private static Font _nullFont;
		internal static Font NullFont
			=> NodeExtender._nullFont ?? (NodeExtender._nullFont = new Font(Control.DefaultFont, FontStyle.Italic));

		/// <summary>Write exception to the list item</summary>
		/// <param name="item">List item</param>
		public static void SetException(this ListViewItem item)
			=> item.ForeColor = NodeExtender.ExceptionColor;

		/// <summary>Write exception to the node</summary>
		/// <param name="node">Node</param>
		/// <param name="exc">Exception</param>
		public static void SetException(this TreeNode node, Exception exc)
			=> node.SetException(exc.Message);

		/// <summary>Write exception message to the node</summary>
		/// <param name="node">Node</param>
		/// <param name="exceptionMessage">Message describing the exception</param>
		public static void SetException(this TreeNode node, String exceptionMessage)
		{
			node.ForeColor = NodeExtender.ExceptionColor;
			node.Text = exceptionMessage;
		}

		/// <summary>Checks if the list item contains an exception value</summary>
		/// <param name="item">List item</param>
		/// <returns>True if the list item contains an exception</returns>
		public static Boolean IsException(this ListViewItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>Checks if the node is in exception state</summary>
		/// <param name="node">Node</param>
		/// <returns>True if the node contains an exception</returns>
		public static Boolean IsException(this TreeNode node)
			=> node.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>Checks if root node is closed and needs to load its content</summary>
		/// <param name="node">Node</param>
		/// <returns>True if node is closed and children need to be loaded</returns>
		public static Boolean IsClosedRootNode(this TreeNode node)
			=> node.Parent == null && node.IsClosedEmptyNode();

		/// <summary>Checks if node is closed and needs to load its content</summary>
		/// <param name="node">Node</param>
		/// <returns>True if node is closed and children need to be loaded</returns>
		public static Boolean IsClosedEmptyNode(this TreeNode node)
			=> node.Nodes.Count == 1 && (node.Nodes[0].Text.Length == 0 || node.Nodes[0].IsException());

		/// <summary>Mark current element as empty or nothing could be done with element</summary>
		/// <param name="item">The item to set null color</param>
		public static void SetNull(this ListViewItem item)
			=> item.ForeColor = NodeExtender.NullColor;

		/// <summary>Mark current element as empty or nothing could be done with element</summary>
		/// <param name="item">The item to set null color</param>
		public static void SetNull(this ToolStripItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.NullColor;
	}
}