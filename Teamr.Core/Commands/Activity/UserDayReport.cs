namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Domain;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Menus;
	using TeamR.Core.Security;
	using TeamR.Infrastructure.EntityFramework;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Security;
	using TeamR.Infrastructure.User;
	using UiMetadataFramework.Basic.Input;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "user-day-report", PostOnLoad = true, Label = "User day report", Menu = CoreMenus.Reports, MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewReport))]
	public class UserDayReport : MyForm<UserDayReport.Request, UserDayReport.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserContext userContext;

		public UserDayReport(UserContext userContext,
			CoreDbContext dbContext)
		{
			this.userContext = userContext;
			this.dbContext = dbContext;
		}

		protected override Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(a => a.CreatedByUserId == this.userContext.User.UserId && a.PerformedOn != null && a.PerformedOn.Value.Date == message.Day.Date)
				.OrderBy(t => t.Id);

				var result = query.Paginate(t => new Item(t), message.Paginator);

			return new Response
			{
				Users = result,
				TotalPoints = query.Sum(s => s.Points)
			};
		}
		
		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0, Required = true)]
			public DateTime Day { get; set; }

			public Paginator Paginator { get; set; }
		}

		public class Response : MyFormResponse
		{
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