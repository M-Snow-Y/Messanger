using Messanger.Shared.Module;

namespace Shared.Interfaces
{
    public interface IMessanger
    {
        Task<List<Chat>> GetAllMessageGroupAsync(int userId);
        Task<List<Chat>> GetAllMessagePeopleAsync(int userId);
        Task<List<Message>> GetChatMessageAsync(int chatId);
        Task<List<Message>> SendMessageAsync(Message message);
    }
}
