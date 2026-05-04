using System.Collections.Generic;
using FluentAssertions;
using Moq.AutoMock;
using Plugin.ReflectionSearch.Tests.TestUtils;
using SAL.Flatbed;
using SAL.Windows;
using Xunit;

namespace Plugin.ReflectionSearch.Tests;

public class PanelSearchTests
{
	private readonly AutoMocker _mocker;

	public PanelSearchTests()
	{
		_mocker = new AutoMocker();
	}

	[Fact]
	[Trait("Category", "Smoke")]
	public void PanelSearch_Should_ConstructSuccessfully()
	{
		// Setup IPluginStorage mock with an empty plugin collection
		var pluginStorageMock = _mocker.GetMock<IPluginStorage>();
		pluginStorageMock.As<IEnumerable<IPluginDescription>>()
			.Setup(x => x.GetEnumerator())
			.Returns(new List<IPluginDescription>().GetEnumerator());
		
		// Setup IHostWindows mock
		var hostWindowsMock = _mocker.GetMock<IHostWindows>();
		hostWindowsMock.SetupGet(h => h.Plugins).Returns(pluginStorageMock.Object);
		
		// Create PluginWindows with the mocked IHostWindows and ITraceSource
		ITraceSource traceMock = _mocker.GetMock<ITraceSource>().Object;
		PluginWindows plugin = new PluginWindows(hostWindowsMock.Object, traceMock);
		
		WindowTestFactory.TestWindowControl testWindow = WindowTestFactory.CreateTestWindow(plugin);
		
		// Act
		using (var form = new PanelSearch() { Parent = testWindow, })
		{
			form.CreateControl(); // triggers initialization
			
			// Assert
			form.IsHandleCreated.Should().BeTrue();
			testWindow.Caption.Should().Be("Reflection Search");
		}
	}
}