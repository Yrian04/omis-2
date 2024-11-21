using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using lab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace lab2.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public EventController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Login", "Account"); // Перенаправляем на страницу входа
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        // Метод для отображения всех групп мероприятий
        public async Task<IActionResult> Groups()
        {
            // Получаем все группы мероприятий с их событиями
            var eventGroups = await _context.EventGroups.Include(g => g.Events).ToListAsync();
            return View(eventGroups);
        }

        // Метод для отображения информации о конкретном мероприятии
        public async Task<IActionResult> Details(Guid id)
        {
            // Ищем мероприятие по ID
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound(); // Если мероприятие не найдено, возвращаем ошибку 404
            }

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventModel)
        {
            if (!ModelState.IsValid)
            {
                // В случае ошибки, повторно отображаем форму
                return View(eventModel);
            }

            // Получаем текущего пользователя
            var currentUser = await _userManager.GetUserAsync(User);

            // Устанавливаем организатора мероприятия
            eventModel.Organizer = currentUser;

            // Сохраняем событие в базе данных
            _context.Events.Add(eventModel);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List)); // Переход на страницу списка мероприятий
        }
    }
}

