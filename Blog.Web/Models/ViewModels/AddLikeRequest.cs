namespace Blog.Web.Models.ViewModels
{
	public class AddLikeRequest
	{
		public Guid PostId { get; set; }
		public Guid UserId { get; set; }
	}
}
