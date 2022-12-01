using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class UserModel
    {
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
