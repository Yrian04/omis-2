using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2.Models
{
    public class Participant : User
    {
        readonly List<Event> _registeredEvents = [];

        public void RegisteredForEvent(Event _event) => _registeredEvents.Add(_event);
        public void CancelRegistration(Event _event) => _registeredEvents.Remove(_event);
    }
}