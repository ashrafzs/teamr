// ReSharper disable UnusedMember.Local

namespace TeamR.Core.Domain.Jira
{
	using System;
	using TeamR.Infrastructure.Domain;

	public class Issue : DomainEntityWithKeyInt32
	{
		private Issue(int projectId,
			DateTime createdOn,
			DateTime? inProgressOn,
			DateTime? codeReviewOn,
			DateTime? readyForDeployOn,
			DateTime? resolvedOn,
			int createdByUserId,
			int? assignedToUserId,
			int? reviewedByUserId,
			IssueStatus status,
			decimal storyPoints)
		{
			this.ProjectId = projectId;
			this.CreatedOn = createdOn;
			this.InProgressOn = inProgressOn;
			this.CodeReviewOn = codeReviewOn;
			this.ReadyForDeployOn = readyForDeployOn;
			this.ResolvedOn = resolvedOn;
			this.CreatedByUserId = createdByUserId;
			this.AssignedToUserId = assignedToUserId;
			this.ReviewedByUserId = reviewedByUserId;
			this.Status = status;
			this.StoryPoints = storyPoints;
		}

		internal Issue(string key,
			int projectId,
			DateTime createdOn,
			DateTime? inProgressOn,
			DateTime? codeReviewOn,
			DateTime? readyForDeployOn,
			DateTime? resolvedOn,
			int createdByUserId,
			int? assignedToUserId,
			int? reviewedByUserId,
			IssueStatus status,
			decimal storyPoints)
		{
			this.IssueKey = key;
			this.ProjectId = projectId;
			this.CreatedOn = createdOn;
			this.InProgressOn = inProgressOn;
			this.CodeReviewOn = codeReviewOn;
			this.ReadyForDeployOn = readyForDeployOn;
			this.ResolvedOn = resolvedOn;
			this.CreatedByUserId = createdByUserId;
			this.AssignedToUserId = assignedToUserId;
			this.ReviewedByUserId = reviewedByUserId;
			this.Status = status;
			this.StoryPoints = storyPoints;
		}

		public int? AssignedToUserId { get; private set; }

		public DateTime? CodeReviewOn { get; private set; }
		public int CreatedByUserId { get; private set; }
		public DateTime CreatedOn { get; private set; }
		public DateTime? InProgressOn { get; private set; }

		/// <summary>
		/// Gets name of the project.
		/// </summary>
		public string IssueKey { get; private set; }

		public int ProjectId { get; private set; }
		public DateTime? ReadyForDeployOn { get; private set; }
		public DateTime? ResolvedOn { get; private set; }
		public int? ReviewedByUserId { get; private set; }
		public IssueStatus Status { get; private set; }
		public decimal StoryPoints { get; private set; }
		public IssueType Type { get; set; }
		public virtual RegisteredUser CreatedByUser { get; set; }
		public virtual RegisteredUser ReviewedByUser { get; set; }
		public virtual RegisteredUser AssignedToUser { get; set; }
		public virtual Project Project { get; set; }

		internal void Edit(string key,
			int projectId,
			int? assignedToUserId,
			DateTime? codeReviewOn,
			int createdByUserId,
			DateTime? resolvedOn,
			DateTime createdOn,
			DateTime? inProgressOn,
			DateTime? readyForDeployOn,
			int? reviewedByUserId,
			IssueStatus status,
			decimal storyPoints,
			IssueType type)
		{
			this.AssignedToUserId = assignedToUserId;
			this.CodeReviewOn = codeReviewOn;
			this.CreatedByUserId = createdByUserId;
			this.CreatedOn = createdOn;
			this.InProgressOn = inProgressOn;
			this.ReadyForDeployOn = readyForDeployOn;
			this.ProjectId = projectId;
			this.IssueKey = key;
			this.ResolvedOn = resolvedOn;
			this.ReviewedByUserId = reviewedByUserId;
			this.Status = status;
			this.StoryPoints = storyPoints;
			this.Type = type;
		}
	}

	public enum IssueStatus
	{
		Done = 1
	}

	public enum IssueType
	{
		Bug = 1,
		Story = 2,
	}
}