namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Domain;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.EntityFramework;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using Teamr.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "user-day-report", PostOnLoad = true, Label = "User day report", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	public class UserDayReport : IForm<UserDayReport.Request, UserDayReport.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;
		private readonly SystemPermissionManager permissionManager;
		private readonly UserContext userContext;

		public UserDayReport(
			SystemPermissionManager permissionManager,
			UserContext userContext,
			CoreDbContext dbContext)
		{
			this.permissionManager = permissionManager;
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		public Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId && a.PerformedOn != null && a.PerformedOn.Value.Date == message.Day.Date)
				.OrderBy(t => t.Id);

				var result = query.Paginate(t => new Item(t), message.Paginator);

			return new Response
			{
				Users = result,
				Actions = this.permissionManager.CanDo(CoreActions.AddActivity, this.userContext)
					? new ActionList(AddActivity.Button())
					: null,
				TotalPoints = query.Sum(s => s.Points)
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewActivities;
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0, Required = true)]
			public DateTime Day { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = -10)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 1, Label = "Total points")]
			public decimal TotalPoints { get; set; }

			[PaginatedData(nameof(Request.Paginator), Label = "", OrderIndex = 20)]
			public PaginatedData<Item> Users { get; set; }
		}

		public static FormLink Button(DateTime date,string label)
		{
			return new FormLink
			{
				Label=label,
				Form = typeof(UserDayReport).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Day), date }
				}
			};
		}

		public class Item
		{
			public Item(Activity t)
			{
				this.Id = t.Id;
				this.Notes = t.Notes;
				this.PerformedOn = t.PerformedOn;
				this.ScheduledOn = t.ScheduledOn;
				this.ActivityType = t.ActivityType.Name;
				this.Quantity = t.Quantity;
				this.Points = t.Points;
			}

			[OutputField(OrderIndex = 2, Label = "Activity type")]
			public string ActivityType { get; set; }

			[OutputField(OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(OrderIndex = 6)]
			public string Notes { get; set; }

			[OutputField(OrderIndex = 7, Label = "Performed on")]
			public DateTime? PerformedOn { get; set; }

			[OutputField(OrderIndex = 5)]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 4)]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 8, Label = "Scheduled on")]
			public DateTime? ScheduledOn { get; set; }
		}
	}
}