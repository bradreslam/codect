using Xunit;

namespace CodectUnitTests;

[TestClass]
public class ComponentControllerIntegrationTest
{
	[Fact]
	public void GetFeatureListReturnsListOfString()
	{
		var client = new HttpClient();
		
		var result = client.GetAsync();
	}
}