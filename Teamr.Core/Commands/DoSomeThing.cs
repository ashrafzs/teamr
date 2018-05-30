namespace Teamr.Core.Commands
{
	using CPermissions;
	using MediatR;
	using Teamr.Core.Menus;
	using Teamr.Core.Security;
	using Teamr.Infrastructure.Forms;
	using Teamr.Infrastructure.Security;
	using UiMetadataFramework.Core;
	using UiMetadataFramework.MediatR;

	[MyForm(Menu = CoreMenus.Main, Id = "do-something", Label = "Do something")]
	public class DoSomeThing : IForm<DoSomeThing.Request, DoSomeThing.Response>,
		ISecureHandler
	{
		public Response Handle(Request message)
		{
			return new Response();
		}

		public UserAction GetPermission()
		{
			return CoreActions.UseTools;
		}

		public class Response : FormResponse
		{
		}

		public class Request : IRequest<Response>
		{
		}
	}
}
