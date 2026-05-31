namespace Messanger.Shared.Module
{
    public class Message
    {
        public int Id {  get; set; }
        public string Text {  get; set; } = string.Empty;
        public string? ImageUrl {  get; set; }

        public int SenderId {  get; set; }
        public User? Sender { get; set; }

        public int ChatId {  get; set; }
        public Chat? Chat { get; set; }

        public DateTime CreatedAt {  get; set; } = DateTime.Now;
    }
}
