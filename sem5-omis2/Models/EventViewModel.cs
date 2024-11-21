using sem5_omis2.Models;

namespace sem5_omis2.Models
{
    public class EventViewModel
    {   
        public required Event Event { get; set; } 
        public required bool IsSubscribed { get; set; }
        public required bool IsOrganizer { get; set; }
    }
}
