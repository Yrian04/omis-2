using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace sem5_omis2.Models
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

        public List<IdentityUser> Participants { get; set; } = new List<IdentityUser>();

        public void AddParticipants(IdentityUser participant) => Participants.Add(participant);
        public void RemoveParticipant(IdentityUser participant) => Participants.Remove(participant);
    }
}