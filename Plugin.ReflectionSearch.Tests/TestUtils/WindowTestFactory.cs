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
	public sealed class TestWindowControl : Control, IWindow
	{
		public IPlugin Plugin => throw new NotImplementedException();

		public Object Control => throw new NotImplementedException();

		public String Caption { get; set; }

		public Object Object => throw new NotImplementedException();

		public event EventHandler Shown;

		public event EventHandler<CancelEventArgs> Closing;

		public event EventHandler Closed;

		public void AddEventHandler(String eventName, EventHandler<DataEventArgs> handler)
			=> throw new NotImplementedException();

		public void Close()
			=> throw new NotImplementedException();

		public void RemoveEventHandler(String eventName, EventHandler<DataEventArgs> handler)
			=> throw new NotImplementedException();

		public void SetDockAreas(DockAreas dockAreas)
			=> throw new NotImplementedException();

		public void SetTabPicture(Object icon)
			=> throw new NotImplementedException();
	}

	/// <summary>Creates a mocked IWindow using Moq with sensible defaults</summary>
	public static Mock<T> CreateMockWindow<T>(this AutoMocker autoMocker, IPlugin pluginMock) where T : Control, IWindow, new()
	{
		var mockWindow = autoMocker.GetMock<T>();

		// Setup default behaviors
		mockWindow.SetupProperty(w => w.Caption, "Test Window");
		mockWindow.SetupProperty(w => w.Plugin, pluginMock);
		mockWindow.Setup(w => w.SetDockAreas(It.IsAny<DockAreas>()));
		mockWindow.Setup(w => w.SetTabPicture(It.IsAny<Object>()));
		mockWindow.Setup(w => w.AddEventHandler(It.IsAny<String>(), It.IsAny<EventHandler<DataEventArgs>>()));
		mockWindow.Setup(w => w.RemoveEventHandler(It.IsAny<String>(), It.IsAny<EventHandler<DataEventArgs>>()));

		// Setup a Control property if IWindow has one
		var mockControl = new Control();
		mockWindow.SetupGet(w => w.Control).Returns(mockControl);
		mockWindow.SetupGet(w => w.Object).Returns(mockControl);
		mockWindow.SetupGet(x => x.Plugin).Returns(pluginMock);

		return mockWindow;
	}
}