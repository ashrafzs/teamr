namespace TeamR.Core.Events
{
	using Teamr.Core.Domain;
	using TeamR.Infrastructure.Domain;

	public class ActivityRecorded : DomainEvent<Activity>
	{
		public ActivityRecorded(Activity context) : base(context)
		{
		}
	}
}