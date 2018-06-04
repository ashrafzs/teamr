namespace Teamr.Core.Commands.ActivityType
{
	using System;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.EntityFramework;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Menu = CoreMenus.Activity, Id = "activity-types", Label = "Activity Types", PostOnLoad = true)]
	public class ActivityTypes : IForm<ActivityTypes.Request, ActivityTypes.Response>,
		ISecureHandler
	{
		private readonly CoreDbContext context;

		public ActivityTypes(CoreDbContext context)
		{
			this.context = context;
		}

		public Response Handle(Request message)
		{
			var activityTypes = this.context.ActivityTypes
				.Include(i => i.User)
				.AsNoTracking()
				.Paginate(t => t, message.ActivityTypePaginator);
			return new Response
			{
				Results = activityTypes.Transform(s => new ActivityTypeItem
				{
					Points = s.Points,
					Name = s.Name,
					Unit = s.Unit,
					CreatedBy = s.User?.Name,
					CreatedOn = s.CreatedOn,
					Remarks = s.Remarks,
					Actions = new ActionList(EditActivityType.Button(s.Id))
				}),
				Actions = new ActionList(AddActivityType.Button())
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivityTypes;
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = 0)]
			public ActionList Actions { get; set; }

			[PaginatedData(nameof(Request.ActivityTypePaginator), OrderIndex = 10, Label = "")]
			public PaginatedData<ActivityTypeItem> Results { get; set; }
		}

		public class Request : IRequest<Response>
		{
			public Paginator ActivityTypePaginator { get; set; }
		}

		public class ActivityTypeItem
		{

			[OutputField(OrderIndex = 70)]
			public string CreatedBy { get; set; }

			[OutputField(OrderIndex = 60)]
			public DateTime CreatedOn { get; set; }

			[OutputField(OrderIndex = 10)]
			public string Name { get; set; }

			[OutputField(OrderIndex = 30)]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 50)]
			public string Remarks { get; set; }

			[OutputField(OrderIndex = 20)]
			public string Unit { get; set; }

			[OutputField(OrderIndex = 90)]
			public ActionList Actions { get; set; }
		}
	}
}