using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class SocietyModel
    {
        [Required]
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        [Required]
        public string EMAIL_LOGIN { get; set; }
        public bool INDUCTION_STATUS { get; set; }
    }
}