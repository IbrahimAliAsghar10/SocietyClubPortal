using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class Society_RegistrationModel
    {
        public int REGISTRATION_NO { get; set; }
        public string STUDENT_ID { get; set; }
        public string SOCIETY_NAME { get; set; }

        public SocietyModel SOCIETY { get; set; }
        public StudentModel student { get; set; }
    }
}
