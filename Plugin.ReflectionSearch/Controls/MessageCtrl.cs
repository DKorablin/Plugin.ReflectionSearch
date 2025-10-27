using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plugin.ReflectionSearch.Controls
{
	internal partial class MessageCtrl : UserControl
	{
		public event EventHandler<ControlEventArgs> OnClosed;

		public enum StatusMessageType
		{
			None = 0,
			Success = 1,
			Progress = 2,
			Failed = 3,
		}

		private static readonly Color[] StatusMessageColor = new Color[] { Color.Empty, Color.LightCyan, Color.AntiqueWhite, Color.Pink, };

		public MessageCtrl()
		{
			this.InitializeComponent();
			this.Visible = false;
		}

		public void ShowMessage(StatusMessageType type, String message)
		{
			if(message == null)
				this.Visible = false;
			else
			{
				this.Visible = true;
				base.BackColor = MessageCtrl.StatusMessageColor[(Int32)type];
				lblMessage.Text = message;
			}
		}

		private void bnClose_MouseHover(Object sender, EventArgs e)
			=> bnClose.ImageIndex = 1;

		private void bnClose_MouseLeave(Object sender, EventArgs e)
			=> bnClose.ImageIndex = 0;

		private void bnClose_Click(Object sender, EventArgs e)
			=> this.OnClosed?.Invoke(this, new ControlEventArgs(this));
	}
}