namespace Messanger.Shared.Module
{
    public class Chat
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsGroup {  get; set; }
        public List<User> Users { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
    }
}
