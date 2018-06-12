namespace Teamr.Core.Commands
{
	using System.Collections.Generic;
	using System.Linq;
	using Teamr.Core.Domain;

	public static class Extensions
	{
		public static IQueryable<T> NotDeleted<T>(this IQueryable<T> queryable)
			where T : class, IDeletable
		{
			return queryable.Where(a => !a.IsDeleted);
		}

		public static IList<T> NotDeleted<T>(this IEnumerable<T> queryable)
			where T : class, IDeletable
		{
			return queryable.Where(a => !a.IsDeleted).ToList();
		}
	}
}