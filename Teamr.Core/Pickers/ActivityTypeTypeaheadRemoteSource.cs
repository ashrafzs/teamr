namespace Teamr.Core.Pickers
{
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Infrastructure;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.Typeahead;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Core.Binding;

	[Form]
	public class ActivityTypeTypeaheadRemoteSource : ITypeaheadRemoteSource<ActivityTypeTypeaheadRemoteSource.Request, int>
	{
		private readonly CoreDbContext dbContext;

		public ActivityTypeTypeaheadRemoteSource(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public TypeaheadResponse<int> Handle(Request message)
		{
			var types = message.GetByIds
				? this.dbContext.ActivityTypes.Where(t => message.Ids.Items.Contains(t.Id))
				: this.dbContext.ActivityTypes.Where(t => t.Id.ToString() == message.Query || t.Name.ToLower().Contains(message.Query.ToLower()));

			return new TypeaheadResponse<int>
			{
				Items = types
					.AsNoTracking()
					.Take(Request.ItemsPerRequest)
					.ToList()
					.Select(t => new TypeaheadItem<int>
					{
						Label = t.Name,
						Value = t.Id
					}).ToList(),
				TotalItemCount = types.Count()
			};
		}

		public class Request : TypeaheadRequest<int>
		{
		}

		public class Response
		{
		}
	}
}