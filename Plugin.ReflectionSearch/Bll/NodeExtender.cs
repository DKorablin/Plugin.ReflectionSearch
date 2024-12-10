using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.ReflectionSearch.Bll
{
	internal static class NodeExtender
	{
		internal static Color NullColor = Color.Gray;
		internal static Color ExceptionColor = Color.Red;
		internal static Font _nullFont;
		private static Font NullFont
			=> NodeExtender._nullFont ?? (NodeExtender._nullFont = new Font(Control.DefaultFont, FontStyle.Italic));

		/// <summary>Записать в элемент списка исключение</summary>
		/// <param name="item">Элемент списка</param>
		public static void SetException(this ListViewItem item)
			=> item.ForeColor = NodeExtender.ExceptionColor;

		/// <summary>В элементе списка записано значение null</summary>
		/// <param name="item">Элемент списка</param>
		/// <returns>В элементе списка записано исключение</returns>
		public static Boolean IsException(this ListViewItem item)
			=> item.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>Mark current element as empty or nothing could be done with element</summary>
		/// <param name="item">The item to set null color</param>
		public static void SetNull(this ListViewItem item)
			=> item.ForeColor = NodeExtender.NullColor;

		/// <summary>Написать в узел исключение</summary>
		/// <param name="node">Узел</param>
		/// <param name="exc">Исключение</param>
		public static void SetException(this TreeNode node, Exception exc)
			=> node.SetException(exc.Message);

		/// <summary>Написать в узел сообщение о исключении</summary>
		/// <param name="node">Узел</param>
		/// <param name="exceptionMessage">Сообщение описывающее исключительную ситуацию</param>
		public static void SetException(this TreeNode node, String exceptionMessage)
		{
			node.ForeColor = NodeExtender.ExceptionColor;
			node.Text = exceptionMessage;
		}

		/// <summary>Узел находится в статусе иксключения</summary>
		/// <param name="node">Узел</param>
		/// <returns>В узел записана исключительная ситуация</returns>
		public static Boolean IsException(this TreeNode node)
			=> node.ForeColor == NodeExtender.ExceptionColor;

		/// <summary>Корневой узел закрыт и необходимо подгрузить его содержимое</summary>
		/// <param name="node">Узел</param>
		/// <returns>Узел закыт и необходимо подгрузить детей</returns>
		public static Boolean IsClosedRootNode(this TreeNode node)
			=> node.Parent == null && node.IsClosedEmptyNode();

		/// <summary>Узел закрыт и необходимо подгрузить его содержимое</summary>
		/// <param name="node">Узел</param>
		/// <returns>Узел закыт и необходимо подгрузить детей</returns>
		public static Boolean IsClosedEmptyNode(this TreeNode node)
			=> node.Nodes.Count == 1 && (node.Nodes[0].Text.Length == 0 || node.Nodes[0].IsException());

		public static void SetNull(this ToolStripItem item)
		{
			item.Font = NodeExtender.NullFont;
			item.ForeColor = NodeExtender.NullColor;
		}

		public static Boolean IsNull(this ToolStripItem item)
			=> item.ForeColor == NodeExtender.NullColor;
	}
}