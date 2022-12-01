using REST_APIS.DbOperations;
using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace REST_APIS.Controllers
{
    public class LoginController : ApiController
    {
        UserRepository usr_repository = null;
        public LoginController()
        {
            usr_repository = new UserRepository();
        }
        [HttpPost]
        public IHttpActionResult Index([FromBody] UserModel usr)
        {
            if (usr_repository.Verifying(usr))
            {
                usr = usr_repository.getUser(usr.Email, "Email");
            }
            else
            {
                usr.Role = -1;
            }
            return Ok(usr);
        }

        public IHttpActionResult GetRole([FromUri] string id)
        {
            int result = usr_repository.get_role(id);
            return Ok(result);
        }
        
    }
}
