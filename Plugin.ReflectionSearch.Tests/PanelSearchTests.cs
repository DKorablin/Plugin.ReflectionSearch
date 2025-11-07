using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Plugin.ReflectionSearch.Tests.TestUtils;
using Xunit;

namespace Plugin.ReflectionSearch.Tests;

public class PanelSearchTests
{
	private readonly AutoMocker _mocker;

	public PanelSearchTests()
	{
		_mocker = new AutoMocker();
	}

	[Fact(DisplayName = "PanelSearch should construct successfully")]
	[Trait("Category", "Smoke")]
	public void PanelSearch_Should_ConstructSuccessfully()
	{
		var pluginMock = new Mock<PluginWindows>();
		var windowMock = _mocker.CreateMockWindow<WindowTestFactory.TestWindowControl>(pluginMock.Object);
		
		// Act
		using (var form = new PanelSearch() { Parent = windowMock.Object, })
		{
			form.CreateControl(); // triggers initialization
			
			// Assert
			form.IsHandleCreated.Should().BeTrue();
			form.Text.Should().Be("Reflection Search");
		}
	}
}