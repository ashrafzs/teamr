namespace Teamr.Infrastructure.Forms.CustomProperties
{
	public class MoneyInputAttribute : NumberConfigAttribute
	{
		public MoneyInputAttribute() : base(0.01)
		{
			this.MinValue = 0;
		}
	}
}