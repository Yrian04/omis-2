using Microsoft.AspNetCore.Mvc;
using sem5_omis2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace sem5_omis2.Controllers
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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        public async Task<IActionResult> Groups()
        {
            var eventGroups = await _context.EventGroups.Include(g => g.Events).ToListAsync();
            return View(eventGroups);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            var currentUser = await _userManager.GetUserAsync(User);

            if (@event is null || currentUser is null)
            {
                return NotFound();
            }

            var eventVM = new EventViewModel
            {
                Event = @event,
                IsSubscribed = @event.Participants.Contains(currentUser),
                IsOrganizer = @event.Organizer?.Id == currentUser.Id
            };

            return View(eventVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (!ModelState.IsValid)
            {
                return View(@event);
            }

            var currentUser = await _userManager.GetUserAsync(User);

            @event.Organizer = currentUser;

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Subscribe(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var @event = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event is null || currentUser is null)
            {
                return NotFound();
            }

            @event.AddParticipants(currentUser);

            if (@event.Participants.Count > @event.MaxParticipants)
            {
                TempData["ErrorMessage"] = "Мест нет";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            _context.Update(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Unsubscribe(Guid id)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id);

            if (@event is null || currentUser is null)
            {
                return NotFound();
            }

            @event.RemoveParticipant(currentUser);

            _context.Update(@event);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (@event.Organizer?.Id != currentUser?.Id)
            {
                return Forbid();
            }

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event updatedEvent)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == updatedEvent.Id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (@event.Organizer?.Id != currentUser?.Id)
            {
                return Forbid(); // Запретить доступ, если текущий пользователь не организатор
            }

            if (ModelState.IsValid)
            {
                @event.Name = updatedEvent.Name;
                @event.Description = updatedEvent.Description;
                @event.Date = updatedEvent.Date;
                @event.Format = updatedEvent.Format;
                @event.Location = updatedEvent.Location;
                @event.MaxParticipants = updatedEvent.MaxParticipants;
                @event.Cost = updatedEvent.Cost;

                _context.Update(@event);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Мероприятие успешно обновлено!";
                return RedirectToAction(nameof(Details), new { id = @event.Id });
            }

            return View(updatedEvent);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (@event.Organizer?.Id != currentUser?.Id)
            {
                return Forbid();
            }

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var @event = await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (@event.Organizer?.Id != currentUser?.Id)
            {
                return Forbid(); // Запретить удаление, если текущий пользователь не организатор
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Мероприятие успешно удалено!";
            return RedirectToAction(nameof(Index));
        }
    }
}

