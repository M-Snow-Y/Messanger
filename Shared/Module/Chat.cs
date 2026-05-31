namespace Messanger.Shared.Module
{
    public class Chat
    {
        public int Id {  get; set; }
        public string? Name { get; set; }
        public bool IsGroup {  get; set; }
        public List<User> Users { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
}
