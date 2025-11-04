using Xunit;

namespace Plugin.ReflectionSearch.Tests;

public class PanelSearchTests
{
	[Fact(DisplayName = "PanelSearch should construct successfully")]
	[Trait("Category", "Smoke")]
	public void MainForm_Should_ConstructSuccessfully()
	{
		using(var form = new PanelSearch())
		{
			form.CreateControl(); // triggers initialization
			Assert.True(form.IsHandleCreated);
			Assert.Equal("Smoke Test Form", form.Text);
		}
	}
}