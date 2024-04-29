using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Task7.Data;
using Task7.Dto;
using Task7.Entities;
using Task7.Enums;
using Task7.Interfaces.Services;

namespace Task7.Services
{
    public class MessageService : IMessageService
    {
        private readonly DBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MessageService(
            DBContext dbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<MessageDto> AddMessageToChatAsync(int chatId, Message message)
        {
            var chat = await _dbContext.Chats
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)
                throw new ArgumentNullException(nameof(chat));

            message.Chat = chat;

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return new MessageDto()
            {
                Id = chat.Messages.Count,
                Body = message.Body,
                MessageType = MessageType.Text,
                Status = MessageStatus.Unread,
                DateTime = DateTime.Now,
            };
        }

        public async Task<MessageDto> AddFileToChatAsync(int chatId, string username, IFormFile file)
        {
            var chat = await _dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)
                throw new ArgumentNullException(nameof(chat));

            var filePath = UploadFile(file);

            var message = new Message()
            {
                Body = filePath,
                Chat = chat,
                MessageType = MessageType.File,
                Status = MessageStatus.Unread,
                Sender = username,
                DateTime = DateTime.Now
            };

            await _dbContext.Messages.AddAsync(message);
            await _dbContext.SaveChangesAsync();

            return new MessageDto()
            {
                Id = chat.Messages.Count,
                Body = filePath,
                MessageType = MessageType.File,
                Status = MessageStatus.Unread,
                DateTime = DateTime.Now,
            };
        }

        public async Task<MessageDto> GetLastMessageByIdAsync(int chatId, string username)
        {
            var chat = await _dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)
                throw new ArgumentNullException(nameof(chat));

            return new MessageDto()
            {
                Id = chat.Messages.Last().Id,
                Body = chat.Messages.Last().Body,
                MessageType = chat.Messages.Last().MessageType,
                Status = chat.Messages.Last().Status,
                DateTime = chat.Messages.Last().DateTime,
                IsMyMessage = chat.Messages.Last().Sender == username ? true : false,
            };
        }

        public async Task<List<MessageDto>> GetMessagesByChatIdAsync(int chatId, string username)
        { 
            var chat = await _dbContext.Chats
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)
                throw new ArgumentNullException(nameof(chat));

            var result = new List<MessageDto>();    

            foreach (var message in chat.Messages)
            {
                result.Add(new MessageDto()
                {
                    Id = message.Id,
                    Body = message.Body,
                    MessageType = message.MessageType,
                    Status = message.Status,
                    DateTime = message.DateTime,
                    IsMyMessage = (message.Sender == username ? true : false),
                });
            }

            return result;
        }

        private string UploadFile(IFormFile file)
        {
            var rootPath = _webHostEnvironment.WebRootPath + "\\Files\\";
            var fileName = Guid.NewGuid().ToString() + file.FileName[file.FileName.LastIndexOf('.')..];

            if (file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(rootPath))
                    {
                        Directory.CreateDirectory(rootPath);
                    }

                    using FileStream fileStream = File.Create(rootPath + fileName);
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                    return "\\Files\\" + fileName;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                throw new Exception("Upload Files");
            }
        }
    }
}

