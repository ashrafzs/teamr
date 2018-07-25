namespace Teamr.Core.Commands.Activity
{
	using System.Collections.Generic;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.Core.Binding;

	[OutputFieldType("calendar")]
	public class TeamCalendar<TItem >
	{
		public TeamCalendar(IEnumerable<TItem> items, MetadataBinder binder)
		{
			this.Items = items;
			this.ItemMetadata = binder.BindOutputFields(typeof(TItem));

		}
		public IEnumerable<TItem> Items { get; set; }
		public IEnumerable<OutputFieldMetadata> ItemMetadata { get; set; }

	}
}