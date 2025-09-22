namespace TravelSite.Models.Notification
{
	public class NotificationViewModel
	{
		public Guid Id { get; set; }
		public string Content { get; set; }=String.Empty;
		public bool Delivered { get; set; }=false;
		public string SenderId { get; set; }= String.Empty;
		public string RecipientId {  get; set; }= String.Empty;
		public Guid BookingId {  get; set; }
	}
}
