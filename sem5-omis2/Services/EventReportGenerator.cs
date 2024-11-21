using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using sem5_omis2.Models;

namespace sem5_omis2.Services
{
    public class EventReportGenerator : IEventReportGenerator
    {
        public byte[] GenerateReport(Event eventData)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Text("Отчёт по мероприятию").FontSize(20).SemiBold().AlignCenter();
                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Название мероприятия: {eventData.Name}").FontSize(14);
                        column.Item().Text($"Описание: {eventData.Description}").FontSize(12);
                        column.Item().Text($"Дата проведения: {eventData.Date:dd.MM.yyyy}").FontSize(12);
                        column.Item().Text($"Формат: {eventData.Format}").FontSize(12);
                        column.Item().Text($"Место: {eventData.Location}").FontSize(12);
                        column.Item().Text($"Максимальное количество участников: {eventData.MaxParticipants}").FontSize(12);
                        column.Item().Text($"Стоимость участия: {eventData.Cost:C}").FontSize(12);
                        column.Item().Text($"Организатор: {eventData.Organizer?.UserName ?? "Не указан"}").FontSize(12);
                        column.Item().Text("Список участников:").FontSize(14).SemiBold();
                        if (eventData.Participants != null && eventData.Participants.Any())
                        {
                            foreach (var participant in eventData.Participants)
                            {
                                column.Item().Text($"- {participant.UserName}").FontSize(12);
                            }
                        }
                        else
                        {
                            column.Item().Text("Нет участников").FontSize(12).Italic();
                        }
                    });
                    page.Footer().AlignCenter().Text($"Сгенерировано: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                });
            }).GeneratePdf();
        }
    }
}

