namespace Teamr.Infrastructure.Forms.CustomProperties
{
	using UiMetadataFramework.Core.Binding;

	public class DecimalStepAttribute : StringPropertyAttribute
	{
		public DecimalStepAttribute(string value)
			: base("decimalStep", value)
		{
		}
	}
}