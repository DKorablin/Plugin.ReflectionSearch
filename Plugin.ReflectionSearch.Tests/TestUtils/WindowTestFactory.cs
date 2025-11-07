using System;
using System.ComponentModel;
using System.Windows.Forms;
using Moq;
using Moq.AutoMock;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.ReflectionSearch.Tests.TestUtils;

/// <summary>Factory for creating test doubles for IWindow</summary>
internal static class WindowTestFactory
{
	/// <summary>Custom control for testing that can be cast to IWindow</summary>
	/// <remarks>This is a workaround since IWindow comes from an external package</remarks>
	public class TestWindowControl : Control, IWindow
	{
		public IPlugin Plugin { get; set; }

		public Object Control { get; set; }

		public String Caption { get; set; }

		public Object Object { get; set; }

		public event EventHandler Shown;

		public event EventHandler<CancelEventArgs> Closing;

		public event EventHandler Closed;

		public void AddEventHandler(String eventName, EventHandler<DataEventArgs> handler)
		{
			// Empty implementation for testing
		}

		public void Close()
		{
			// Empty implementation for testing
		}

		public void RemoveEventHandler(String eventName, EventHandler<DataEventArgs> handler)
		{
			// Empty implementation for testing
		}

		public void SetDockAreas(DockAreas dockAreas)
		{
			// Empty implementation for testing
		}

		public void SetTabPicture(Object icon)
		{
			// Empty implementation for testing
		}
	}

	/// <summary>Creates a test window instance with sensible defaults</summary>
	public static TestWindowControl CreateTestWindow(IPlugin pluginMock)
	{
		var mockControl = new Control();
		var window = new TestWindowControl
		{
			Caption = "Test Window",
			Plugin = pluginMock,
			Control = mockControl,
			Object = mockControl
		};

		return window;
	}
}