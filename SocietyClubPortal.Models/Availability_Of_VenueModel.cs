using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class Availability_Of_VenueModel
    {
        public System.DateTime DATED { get; set; }
        public System.DateTime START_TIME { get; set; }
        public System.DateTime END_TIME { get; set; }
        public int TIME_SLOT { get; set; }
        public string IS_BOOK { get; set; }
        public int VENUE_ID { get; set; }

        public VenueModel VENUE{ get; set; }
    }
}
