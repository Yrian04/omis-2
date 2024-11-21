using sem5_omis2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace sem5_omis2.Controllers
{
    [Authorize]
    public class EventGroupController : Controller
    {
        private readonly ApplicationContext _context;

        public EventGroupController(ApplicationContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var eventGroups = _context.EventGroups.ToList();
            return View(eventGroups);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EventGroup eventGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(eventGroup);
            }
            _context.EventGroups.Add(eventGroup);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid id)
        {
                var eventGroup = await _context.EventGroups
                .Include(e => e.Events)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventGroup == null)
            {
                return NotFound();
            }

            return View(eventGroup);
        }

        public async Task<IActionResult> AddEvent(Guid groupId)
        {
            var eventGroup = await _context.EventGroups
                .Include(e => e.Events)
                .FirstOrDefaultAsync(m => m.Id == groupId);
            if (eventGroup == null)
            {
                return NotFound();
            }

            ViewBag.GroupId = groupId;
            ViewBag.GroupName = eventGroup.GroupName;

            var availableEvents = await _context.Events
                .Where(e => !eventGroup.Events.Contains(e))
                .ToListAsync();

            return View(availableEvents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(Guid groupId, Guid eventId)
        {
            var eventGroup = await _context.EventGroups
                .Include(g => g.Events)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            var existingEvent = await _context.Events.FindAsync(eventId);

            if (eventGroup == null || existingEvent == null)
            {
                return NotFound();
            }

            eventGroup.Events.Add(existingEvent);
            _context.Update(eventGroup);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = groupId });
        }
    }
}
