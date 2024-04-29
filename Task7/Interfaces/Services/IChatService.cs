using Task7.Dto;

namespace Task7.Interfaces.Services
{
    public interface IChatService
    {
        Task<List<ChatDto>> GetUserChatsAsync(string username);
        Task StartChatAsync(string username, string companionUsername);
        Task<List<string>> GetUsersByChatId(int chatId);
    }
}
