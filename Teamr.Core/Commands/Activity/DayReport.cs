namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.Commands.Leave;
	using Teamr.Core.Domain;
	using Teamr.Core.Pickers;
	using TeamR.Core.DataAccess;
	using TeamR.Core.Security;
	using TeamR.Help;
	using TeamR.Infrastructure;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.CustomProperties;
	using TeamR.Infrastructure.Security;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core.Binding;

	[MyForm(Id = "day-report", PostOnLoad = true, Label = "Day report", MenuOrderIndex = 1)]
	[Secure(typeof(CoreActions), nameof(CoreActions.ViewReport))]
	public class DayReport : MyForm<DayReport.Request, DayReport.Response>
	{
		private readonly CoreDbContext dbContext;
		private readonly UserSecurityContext userSecurityContext;

		public DayReport(CoreDbContext dbContext, UserSecurityContext userSecurityContext)
		{
			this.dbContext = dbContext;
			this.userSecurityContext = userSecurityContext;
		}

		public static FormLink Button(DateTime date, string label)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(DayReport).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.Day), date }
				}
			};
		}

		protected override Response Handle(Request message)
		{
			var activities = this.dbContext.Activities
				.Include(a => a.ActivityType)
				.Where(t => t.CreatedByUserId == message.User.Value)
				.Where(a => a.PerformedOn.Value.Date == message.Day.Date || a.PerformedOn == null && a.ScheduledOn.Date == message.Day.Date)
				.OrderBy(t => t.PerformedOn)
				.ToList()
				.Select(t => new Item
				{
					Id = t.Id,
					Notes = t.Notes,
					WasPerformed = (t.PerformedOn != null).ToYesOrNoString(),
					ScheduledOn = t.ScheduledOn,
					ActivityType = t.ActivityType.Name,
					Quantity = t.Quantity,
					Points = t.Points,
					Actions = this.GetActions(t).AsActionList()
				})
				.ToList();

			return new Response
			{
				Activities = activities,
				Leave = this.dbContext.Leaves
					.Include(t => t.LeaveType)
					.Where(t => t.ScheduledOn.Date == message.Day.Date)
					.Where(t => t.CreatedByUserId == message.User.Value)
					.ToList()
					.Select(t => new LeaveRow
					{
						Id = t.Id,
						Type = t.LeaveType.Name,
						Notes = t.Notes,
						Actions = this.GetActions(t).AsActionList()
					}).ToList(),
				TotalPoints = activities.Sum(s => s.Points)
			};
		}

		private IEnumerable<FormLink> GetActions(Activity activity)
		{
			if (this.userSecurityContext.CanAccess<EditActivity>(activity.Id))
			{
				yield return EditActivity.Button(activity.Id);
			}

			if (this.userSecurityContext.CanAccess<DeleteActivity>(activity.Id))
			{
				yield return DeleteActivity.Button(activity.Id);
			}
		}

		private IEnumerable<FormLink> GetActions(Leave leave)
		{
			if (this.userSecurityContext.CanAccess<EditLeave>(leave.Id))
			{
				yield return EditLeave.Button(leave.Id);
			}

			if (this.userSecurityContext.CanAccess<DeleteLeave>(leave.Id))
			{
				yield return DeleteLeave.Button(leave.Id);
			}
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0, Required = true)]
			public DateTime Day { get; set; }

			[TypeaheadInputField(typeof(UserTypeaheadRemoteSource), Label = "User", Required = true)]
			public TypeaheadValue<int> User { get; set; }
		}

		public class Response : MyFormResponse
		{
			[OutputField(Label = "Activities", OrderIndex = 20)]
			public IList<Item> Activities { get; set; }

			[OutputField(Label = "Leave", OrderIndex = 10)]
			public IList<LeaveRow> Leave { get; set; }

			[OutputField(OrderIndex = 1, Label = "Total points")]
			public decimal TotalPoints { get; set; }
		}

		public class Item
		{
			[OutputField(Label = "", OrderIndex = 30)]
			public ActionList Actions { get; set; }

			[OutputField(OrderIndex = 2, Label = "Activity type")]
			public string ActivityType { get; set; }

			[OutputField(OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(OrderIndex = 6)]
			public string Notes { get; set; }

			[OutputField(OrderIndex = 5)]
			[Documentation(
				DocumentationPlacement.Hint,
				DocumentationSourceType.String,
				"Number of points user has received for this activity.")]
			public decimal Points { get; set; }

			[OutputField(OrderIndex = 4)]
			public decimal Quantity { get; set; }

			[OutputField(OrderIndex = 8, Label = "Scheduled on")]
			[DateTimeStyle(DateTimeStyle.Date)]
			[Documentation(
				DocumentationPlacement.Hint,
				DocumentationSourceType.String,
				"The date for which the activity was originally scheduled. It can be " +
				"different from the date when it was actually performed.")]
			public DateTime? ScheduledOn { get; set; }

			[OutputField(OrderIndex = 20, Label = "Was performed?")]
			[Documentation(
				DocumentationPlacement.Hint,
				DocumentationSourceType.String,
				"Indicates whether the activity was actually performed or just scheduled, but never performed.")]
			public string WasPerformed { get; set; }
		}

		public class LeaveRow
		{
			[OutputField(Label = "", OrderIndex = 20)]
			public ActionList Actions { get; set; }

			[OutputField(Label = "ID", OrderIndex = 1)]
			public int Id { get; set; }

			[OutputField(Label = "Notes", OrderIndex = 10)]
			public string Notes { get; set; }

			[OutputField(Label = "Leave type", OrderIndex = 5)]
			public string Type { get; set; }
		}
	}
}