namespace Teamr.Core.Commands.Activity
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CPermissions;
	using MediatR;
	using Teamr.Core.DataAccess;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Basic.Output;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;
	using UiMetadataFramework.MediatR;

	[MyForm(Id = "user-profile", PostOnLoad = true, Label = "User profile")]
	public class UserProfile : IForm<UserProfile.Request, UserProfile.Response>, ISecureHandler
	{
		private readonly CoreDbContext dbContext;

		public UserProfile(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public Response Handle(Request message)
		{
			var query = this.dbContext.Activities
				.Where(a => a.CreatedByUserId == message.UserId && a.PerformedOn != null);

			var totalPoints = query.Sum(s => s.Points);
			var firstDate = query.OrderBy(a => a.PerformedOn).FirstOrDefault()?.PerformedOn;
			int totalDays = 0;
			if (firstDate != null)
			{
				totalDays = DateTime.UtcNow.Subtract(firstDate.Value).Days / 7 * 5;
			}

			var user = this.dbContext.Users.FindOrException(message.UserId);

			return new Response
			{
				Name = user.Name,
				TotalPoints = totalPoints,
				AveragePerDay = totalDays > 0 ? totalPoints / totalDays : 0
			};
		}

		public UserAction GetPermission()
		{
			return CoreActions.ViewUserProfile;
		}

		public static FormLink Button(int userId, string label)
		{
			return new FormLink
			{
				Label = label,
				Form = typeof(UserProfile).GetFormId(),
				InputFieldValues = new Dictionary<string, object>
				{
					{ nameof(Request.UserId), userId }
				}
			};
		}

		public class Request : IRequest<Response>
		{
			[InputField(OrderIndex = 0, Required = true, Hidden = true)]
			public int UserId { get; set; }
		}

		public class Response : FormResponse
		{
			[OutputField(OrderIndex = 2, Label = "Average per day")]
			public decimal AveragePerDay { get; set; }

			[OutputField(OrderIndex = 1, Label = "Name")]
			public string Name { get; set; }

			[OutputField(OrderIndex = 2, Label = "Total points")]
			public decimal TotalPoints { get; set; }
		}
	}
}