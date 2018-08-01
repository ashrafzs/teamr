namespace Teamr.Core.Commands.ActivityType
{
	using System;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Menus;
	using TeamR.Core.Security;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Menu = CoreMenus.Activity, Id = "activity-types", Label = "Activity types", PostOnLoad = true)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewActivityTypes))]
	public class ActivityTypes : MyForm<ActivityTypes.Request, ActivityTypes.Response>
	{
		private readonly CoreDbContext context;

		public ActivityTypes(CoreDbContext context)
		{
			this.context = context;
		}

		protected override Response Handle(Request message)
		{
			var activityTypes = this.context.ActivityTypes
				.AsQueryable()
				.Include(t => t.User)
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
					Actions = new ActionList(EditActivityType.Button(s.Id), DeleteActivityType.Button(s.Id))
				}),
				Actions = new ActionList(AddActivityType.Button())
			};
		}

		public class Response : MyFormResponse
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

			[OutputField(OrderIndex = 70, Label = "Created by")]
			public string CreatedBy { get; set; }

			[OutputField(OrderIndex = 60, Label = "Created on")]
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