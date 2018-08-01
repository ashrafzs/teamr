namespace TeamR.Infrastructure.Forms
{
	using MediatR;
	using UiMetadataFramework.Basic.Input.Dropdown;
	using UiMetadataFramework.MediatR;
	using TeamR.Infrastructure.Forms.Dropdown;

	public abstract class DropdownRemoteSource<TRequest> :
		Form<TRequest, DropdownResponse>, IDropdownRemoteSource
		where TRequest : IRequest<DropdownResponse>
	{
	}
}
