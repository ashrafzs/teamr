namespace TeamR.Core.EventHandlers
{
	using TeamR.Core.Events;
	using TeamR.Infrastructure.Domain;

	public class OnActivityRecorded : AppEventHandler<ActivityRecorded>
	{
		public OnActivityRecorded(EventManager manager) : base(manager)
		{
		}

		public override void HandleEvent(ActivityRecorded @event)
		{
			// Event-handling code goes here.
		}
	}
}
