namespace Teamr.Infrastructure.Forms.CustomProperties
{
	using UiMetadataFramework.Core.Binding;

	public class CssClassAttribute : StringPropertyAttribute
	{
		public CssClassAttribute(string value)
			: base("cssClass", value)
		{
		}
	}
}