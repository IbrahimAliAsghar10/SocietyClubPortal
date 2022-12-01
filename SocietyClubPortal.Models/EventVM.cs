using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class EventVM
    {
        public List<ScheduleModel> Schedule_VM { get; set; }
        public List<EventModel> NonSchedule_VM { get; set; }
    }
}
