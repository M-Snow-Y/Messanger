using System;
using System.Collections.Generic;
using System.Text;

namespace Messanger.Shared.Module;
    public class User
    {
        public int Id { get; set; }
        public string UserName {  get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Avatar { get; set; }
        public string? About { get;set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt {  get; set; } = DateTime.Now;
        public List<Chat> Chats { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
