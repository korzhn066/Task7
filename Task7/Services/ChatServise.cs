using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using Task7.Data;
using Task7.Dto;
using Task7.Entities;
using Task7.Enums;
using Task7.Interfaces.Services;

namespace Task7.Services
{
    public class ChatServise : IChatService
    {
        private readonly DBContext _dbContext;

        public ChatServise(
            DBContext dBContext) 
        { 
            _dbContext = dBContext;
        }

        public async Task StartChatAsync(string username, string companionUsername)
        {
            var user = await _dbContext.Users
                .Include(u => u.ApplicationUserChats)
                .ThenInclude(ac => ac.Chat)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var companion = await _dbContext.Users
                .Include(u => u.ApplicationUserChats)
                .ThenInclude(ac => ac.Chat)
                .FirstOrDefaultAsync(u => u.UserName == companionUsername);

            if (companion is null)
            {
                throw new ArgumentNullException(nameof(companion));
            }

            var chat = new Chat()
            {
                Messages = new List<Message>() 
                {
                    new Message()
                    {
                        Body = "Hello!!!",
                        Status = MessageStatus.Unread,
                        MessageType = MessageType.Text,
                        DateTime = DateTime.Now,
                        Sender = username
                    }
                },
            };

            var applicationUserChats = new List<ApplicationUserChat>()
                {
                    new ApplicationUserChat()
                    {
                        ApplicationUser = user,
                        Chat = chat
                    },
                    new ApplicationUserChat()
                    {
                        ApplicationUser = companion,
                        Chat = chat
                    }
                };

            chat.ApplicationUserChats = applicationUserChats;

            await _dbContext.Chats.AddAsync(chat);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ChatDto>> GetUserChatsAsync(string username)
        {
            var user = await _dbContext.Users
                .Include(u => u.ApplicationUserChats)
                .ThenInclude(au => au.Chat)
                .ThenInclude(c => c.ApplicationUserChats)
                .ThenInclude(au => au.ApplicationUser)
                .FirstOrDefaultAsync(u => u.UserName == username);

            if (user is null)
                throw new ArgumentNullException();

            var result = new List<ChatDto>();

            foreach (var applicationUserChat in user.ApplicationUserChats)
            {
                foreach (var item in applicationUserChat.Chat.ApplicationUserChats)
                {
                    if (item.ApplicationUser is null)
                        continue;

                    if (item.ApplicationUser == user)
                        continue;

                    result.Add(new ChatDto
                    {
                        Id = item.ChatId,
                        CompanionName = item.ApplicationUser.Name,
                        CompanionUserName = item.ApplicationUser.UserName,
                    });
                }
            }

            return result;
        }

        public async Task<List<string>> GetUsersByChatId(int chatId)
        {
            var chat = await _dbContext.Chats
                .Include(c => c.ApplicationUserChats)
                .ThenInclude(au => au.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == chatId);

            if (chat is null)   
                throw new ArgumentNullException();

            var users = new List<string>();

            return chat.ApplicationUserChats.Select(ac => ac.ApplicationUser.UserName).ToList();
        }
    }
}
