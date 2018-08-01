namespace TeamR.Core.Filing
{
	using System;
	using System.Collections.Generic;
	using Teamr.Core.Domain;
	using Teamr.Core.Security.Activity;
	using TeamR.Filing;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Output;

	[RegisterEntry(Key)]
	public class ActivityFileManager : IEntityFileManager
	{
		public const string Key = "activity";
		private readonly ActivityRepository repository;
		private readonly UserContext userContext;
		private readonly ActivityPermissionManager activityPermissionManager;

		public ActivityFileManager(
			ActivityPermissionManager activityPermissionManager,
			UserContext userContext,
			ActivityRepository repository)
		{
			this.activityPermissionManager = activityPermissionManager;
			this.userContext = userContext;
			this.repository = repository;
		}

		public bool CanDeleteFiles(object entityId, string metTag)
		{
			return this.CanDo(entityId, ActivityAction.Edit);
		}

		public bool CanUploadFiles(object entityId)
		{
			return this.CanDo(entityId, ActivityAction.Edit);
		}

		public bool CanViewFiles(object entityId)
		{
			return this.CanDo(entityId, ActivityAction.Edit);
		}

		public IEnumerable<FormLink> GetActions(object entityId, string metaTag = null, bool isMultiple = false)
		{
			yield break;
		}

		public IEnumerable<FormLink> GetFileActions(object entityId, int fileId)
		{
			yield break;
		}

		public static string ContextId(object id) => $"{Key}:{id}";

		private bool CanDo(object entityId, ActivityAction action)
		{
			var verificationId = Convert.ToInt32(entityId);
			var verification = (Activity)this.repository.Find(verificationId);

			return this.activityPermissionManager.CanDo(
				action,
				this.userContext,
				verification);
		}
	}
}