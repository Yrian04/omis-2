using lab2.Models;

namespace lab2.Services
{
    public interface IEventReportGenerator
    {
        byte[] GenerateReport(Event eventData);
    }
}