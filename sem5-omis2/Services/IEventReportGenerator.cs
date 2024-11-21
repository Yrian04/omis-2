using sem5_omis2.Models;

namespace sem5_omis2.Services
{
    public interface IEventReportGenerator
    {
        byte[] GenerateReport(Event eventData);
    }
}