using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace lab2.Models
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public EventFormat Format { get; set; } = EventFormat.OFFLINE;
        public string Location { get; set; } = "";
        public int MaxParticipants { get; set; }
        public decimal Cost { get; set; }

        public IdentityUser? Organizer { get; set; }

        public readonly List<IdentityUser> _participants = new List<IdentityUser>();

        public void AddParticipants(IdentityUser participant) => _participants.Add(participant);
        public void RemoveParticipant(IdentityUser participant) => _participants.Remove(participant);
    }
}