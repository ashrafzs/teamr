namespace Teamr.Core.Pickers
{
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using TeamR.Core.DataAccess;
	using TeamR.Infrastructure.Forms;
	using TeamR.Infrastructure.Forms.Typeahead;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Core.Binding;

	[Form]
	public class LeaveTypeTypeaheadRemoteSource : TypeaheadRemoteSource<LeaveTypeTypeaheadRemoteSource.Request, int>
	{
		private readonly CoreDbContext dbContext;

		public LeaveTypeTypeaheadRemoteSource(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		protected override TypeaheadResponse<int> Handle(Request message)
		{
			var types = message.GetByIds
				? this.dbContext.LeaveTypes.Where(t => message.Ids.Items.Contains(t.Id))
				: this.dbContext.LeaveTypes.Where(t => t.Id.ToString() == message.Query || t.Name.ToLower().Contains(message.Query.ToLower()));

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