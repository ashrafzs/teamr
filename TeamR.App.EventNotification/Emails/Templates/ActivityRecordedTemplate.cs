namespace TeamR.App.EventNotification.Emails.Templates
{
	using Microsoft.Extensions.Options;
	using Teamr.Core.Domain;
	using TeamR.Core.Domain;
	using TeamR.Infrastructure.Configuration;
	using TeamR.Infrastructure.Emails;
	using TeamR.Infrastructure.Messages;

	public class ActivityRecordedTemplate : EmailTemplate<ActivityRecordedTemplate.Model>
	{
		public ActivityRecordedTemplate(IOptions<AppConfig> appConfig, IViewRenderService viewRenderService)
			: base(appConfig, viewRenderService, nameof(ActivityRecordedTemplate))
		{
		}

		protected override string GetSubject(Model model)
		{
			return $"Activity #{model.Id} was recorded";
		}

		public class Model
		{
			private readonly AppConfig appConfig;

			public Model(Activity item, AppConfig appConfig)
			{
				this.Id = item.Id;
				this.AssigneeName = item.CreatedByUser.Name;
				this.appConfig = appConfig;
				this.Description = item.Notes;
			}

			public string AssigneeName { get; set; }
			public string Description { get; set; }
			public int Id { get; set; }
			public string Url => $"{this.appConfig.SiteRoot}/#/form/activity?Id={this.Id}";
		}
	}
}
