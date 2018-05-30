namespace Teamr.Filing
{
	using System.Reflection;
	using Teamr.Infrastructure;

	public class Bootstrap : IAssemblyBootstrapper
	{
		public int Priority { get; } = 60;

		public void Start(DependencyInjectionContainer dependencyInjectionContainer)
		{
			dependencyInjectionContainer.RegisterUiMetadata(typeof(Bootstrap).GetTypeInfo().Assembly);
		}
	}
}
