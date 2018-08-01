namespace TeamR.Infrastructure.Forms
{
	using MediatR;
	using UiMetadataFramework.Basic.Input.Typeahead;
	using UiMetadataFramework.MediatR;
	using TeamR.Infrastructure.Forms.Typeahead;

	public abstract class TypeaheadRemoteSource<TRequest, TKey> : Form<TRequest, TypeaheadResponse<TKey>>, ITypeaheadRemoteSource
		where TRequest : IRequest<TypeaheadResponse<TKey>>
	{
	}
}
