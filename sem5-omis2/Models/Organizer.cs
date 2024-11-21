using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2.Models
{
    public class Organizer(string company, string position) : User, IEventManager
    {
        public IList<Event> ManagedEvents = [];
        public IList<EventGroup> ManagedGroup = [];

        public void CreateEvent(Event _event)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(Event _event)
        {
            throw new NotImplementedException();
        }

        public void DeleteEvent(Event _event)
        {
            throw new NotImplementedException();
        }

        public void GenerateReport()
        {
            throw new NotImplementedException();
        }

        public void CreateEventGroup(EventGroup eventGroup)
        {
            throw new NotImplementedException();
        }

        public void DeleteEventFromGroup(EventGroup eventGroup, Event _event)
        {
            throw new NotImplementedException();
        }
    }
}