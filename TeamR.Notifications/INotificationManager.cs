namespace TeamR.Notifications
{
	public interface INotificationManager
	{
		NotificationDetail GetLink(object entityId);
	}
}
