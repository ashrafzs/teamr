namespace Teamr.Infrastructure.Forms
{
	public static class UiFormConstants
	{
		public const string SearchLabel = "<i class='fa fa-search'></i> Search";
		public const string EditSubmitLabel = "Save changes";
		public const string EditLabel = "<i class='fa fa-edit'></i>";
		public const string DeleteLabel = "<i class='fa fa-times'></i>";
		public const string EditIconLabel = "<i class='far fa-edit'></i>";
		public const string ImpersonationLabel = "<i class='far fa-eye'></i>";
		public const string StopImpersonationLabel = "<i class='far fa-eye-slash'></i>";

		public static string ExcelTemplateUrl(string templateKey, int? formId) => $"/file/downloadExcelTemplate?templateName={templateKey}&formId={formId}";
		public static string FileUrl(int fileId) => $"/file/download?id={fileId}";
	}
}