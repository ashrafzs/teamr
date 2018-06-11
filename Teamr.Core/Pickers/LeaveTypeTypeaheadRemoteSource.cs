namespace Teamr.Core.Pickers
{
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Teamr.Core.DataAccess;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Forms.Typeahead;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.Core.Binding;

	[Form]
	public class LeaveTypeTypeaheadRemoteSource : ITypeaheadRemoteSource<LeaveTypeTypeaheadRemoteSource.Request, int>
	{
		private readonly CoreDbContext dbContext;

		public LeaveTypeTypeaheadRemoteSource(CoreDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public TypeaheadResponse<int> Handle(Request message)
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