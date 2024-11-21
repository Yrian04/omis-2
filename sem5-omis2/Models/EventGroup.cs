namespace lab2.Models
{
    public class EventGroup
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = "";
        public string Theme { get; set; } = "";
        public List<Event> Events { get; set; } = new List<Event>();

        public void AddEvent(Event _event) => Events.Add(_event);
        public void RemoveEvent(Event _event) => Events.Remove(_event);
    }
}