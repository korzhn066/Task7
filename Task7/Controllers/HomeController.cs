using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using Task7.Hubs;
using Task7.Interfaces.Services;
using Task7.Models;
using Task7.Services;

namespace Task7.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _chatHub;

        public HomeController(
            IChatService chatService,
            IMessageService messageService,
            IHubContext<ChatHub> chatHub)
        {
            _chatService = chatService;
            _messageService = messageService;
            _chatHub = chatHub;
        }

        public async Task<IActionResult> GetMessagesById(int chatId)
        {
            var messages = await _messageService.GetMessagesByChatIdAsync(chatId, HttpContext!.User!.Identity!.Name!);

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendFile([FromQuery]int chatId, IFormFile file)
        {
            var message = await _messageService.AddFileToChatAsync(chatId, HttpContext!.User!.Identity!.Name!, file);

            var users = await _chatService.GetUsersByChatId(chatId);

            foreach (var user in users)
            {
                message.IsMyMessage = user == HttpContext!.User!.Identity!.Name ? true : false;

                await _chatHub.Clients.User(user).SendAsync(
                    "Chat",
                    message
                );
            }

            return NoContent();
        }

        public async Task<IActionResult> GetUserChats()
        {
            var chats = await _chatService.GetUserChatsAsync(HttpContext!.User!.Identity!.Name!);

            return Json(chats);
        }

        public async Task<IActionResult> StartChat(string companionUsername)
        {
            await _chatService.StartChatAsync(HttpContext!.User!.Identity!.Name!, companionUsername);

            return NoContent();
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}