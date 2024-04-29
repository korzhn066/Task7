using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using Task7.Hubs;
using Task7.Interfaces.Services;
using Task7.Models;

namespace Task7.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        public IActionResult Index(string roomId)
        {
            ViewBag.RoomId = roomId;
            return View();
        }
    }
}
