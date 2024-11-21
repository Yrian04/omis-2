using Microsoft.AspNetCore.Mvc;
using sem5_omis2.Models;
using sem5_omis2.Services;
using Microsoft.EntityFrameworkCore;


namespace sem5_omis2.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IEventReportGenerator _reportGenerator;

        public ReportController(ApplicationContext context, IEventReportGenerator reportGenerator)
        {
            _context = context;
            _reportGenerator = reportGenerator;
        }

        public IActionResult Index()
        {
            return View(_context.Events.ToList());
        }

        public async Task<IActionResult> GenerateReport(Guid eventId)
        {
            var eventData = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventData == null)
            {
                return NotFound();
            }

            var pdfBytes = _reportGenerator.GenerateReport(eventData);

            return File(pdfBytes, "application/pdf", $"{eventData.Name}-Report.pdf");
        }
    }
}
