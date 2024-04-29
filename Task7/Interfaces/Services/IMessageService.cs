using Task7.Dto;
using Task7.Entities;

namespace Task7.Interfaces.Services
{
    public interface IMessageService
    {
        Task<List<MessageDto>> GetMessagesByChatIdAsync(int chatId, string username);
        Task<MessageDto> GetLastMessageByIdAsync(int chatId, string username);
        Task<MessageDto> AddMessageToChatAsync(int chatId, Message message);
        Task<MessageDto> AddFileToChatAsync(int chatId, string username, IFormFile file);
    }
}
