using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using Task7.Data;
using Task7.Dto;
using Task7.Entities;
using Task7.Enums;
using Task7.Interfaces.Services;
using Task7.Services;

namespace Task7.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatServise;
        private readonly IMessageService _messageService;

        public ChatHub(
            IChatService chatServise,
            IMessageService messageService)
        {
            _chatServise = chatServise;
            _messageService = messageService;
        }

        public async Task SendMessage(string chatId, string message)
        {
            var messageDto = await _messageService.AddMessageToChatAsync(int.Parse(chatId), new Message()
            {
                Body = message,
                Status = MessageStatus.Unread,
                MessageType = MessageType.Text,
                DateTime = DateTime.UtcNow,
                Sender = Context!.User!.Identity!.Name!
            });

            var users = await _chatServise.GetUsersByChatId(int.Parse(chatId));

            foreach (var user in users)
            {
                messageDto.IsMyMessage = user == Context!.User!.Identity!.Name ? true : false;

                await Clients.User(user).SendAsync(
                    "Chat",
                    messageDto
                );
            }
        }

        public async Task SendInvite(string chatId)
        {
            var users = await _chatServise.GetUsersByChatId(int.Parse(chatId));

            foreach (var user in users)
            {
                await Clients.User(user).SendAsync(
                "Invite", 
                string.Join("", users)
                );
            }
        }
    }
}
