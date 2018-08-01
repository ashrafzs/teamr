namespace TeamR.Notifications
{
	using TeamR.Infrastructure;

	public class NotificationManagerRegister : Register<INotificationManager>
	{
		public NotificationManagerRegister(DependencyInjectionContainer dependencyInjectionContainer) : base(dependencyInjectionContainer)
		{
		}
	}
}
