using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class ScheduleModel
    {
        public System.DateTime DATED { get; set; }
        public System.TimeSpan START_TIME { get; set; }
        public System.TimeSpan END_TIME { get; set; }
        public int VENUE_ID { get; set; }
        public string EVENT_NAME { get; set; }
        public string SOCIETY_NAME { get; set; }
        public EventModel EVENT { get; set; }
        public VenueModel VENUE { get; set; }
    }
}
