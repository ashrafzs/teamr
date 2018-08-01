namespace TeamR.Infrastructure.Tests
{
	using FluentAssertions;
	using TeamR.DataSeed;
	using Xunit;

	public class MetadataTest
	{
		[Fact]
		public void MetadataIsConfiguredCorrectly()
		{
			new DataSeedDiContainer().Should().NotBeNull();
		}
	}
}
