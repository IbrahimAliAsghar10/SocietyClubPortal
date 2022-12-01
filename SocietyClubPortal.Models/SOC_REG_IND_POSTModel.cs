using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class SOC_REG_IND_POSTModel
    {
        public string SOCIETY_NAME { get; set; }
        public string SOCIETY_DESCRIPTION { get; set; }
        public bool induction_status { get; set; }
        public bool registrationstatus { get; set; }
        public bool post_status { get; set; }
    }
}
