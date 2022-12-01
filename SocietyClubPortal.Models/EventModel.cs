using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class EventModel
    {
        [Required]
        public string NAME { get; set; }
        [Required]
        public string DESCRIPTION { get; set; }
        public string SOCIETY_NAME { get; set; }
        public SocietyModel SOCIETY { get; set; }
    }
}
