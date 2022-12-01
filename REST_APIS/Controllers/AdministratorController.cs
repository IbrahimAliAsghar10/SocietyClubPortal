using REST_APIS.DbOperations;
using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace REST_APIS.Controllers
{
    public class AdministratorController : ApiController
    {
        StudentRepository st_repository = null;
        SocietyRepository sc_repository = null;

        //initializing the repositories for the possible users.(Constructor)
        public AdministratorController()
        {
            st_repository = new StudentRepository();
            sc_repository = new SocietyRepository();
        }
        [HttpPost]
        public IHttpActionResult AddStudent([FromBody] string Email)
        {
            int opt = st_repository.AddStudent(Email);
            return Ok(opt);
        }
        [HttpPost]
        public IHttpActionResult AddSociety([FromBody] SocietyModel model)
        {
            int opt = sc_repository.AddSociety(model);
            return Ok(opt);
        }

        [HttpGet]
        public IHttpActionResult Students_Data()
        {
            List<StudentModel> std = st_repository.Get_All_Students();
            return Ok(std);
        }

        [HttpGet]
        public IHttpActionResult Societies_Data()
        {
            List<SocietyModel> sc = sc_repository.get_all_society();
            return Ok(sc);
        }
        [HttpDelete]
        public IHttpActionResult delete_society([FromUri] string id)
        {
            bool delete_cos = sc_repository.delete_society(id);
            return Ok(delete_cos);
        }
        [HttpGet]
        public IHttpActionResult Update_Society([FromUri] string id)
        {
            SocietyModel sc = sc_repository.getsociety(id);
            return Ok(sc);
        }

        [HttpPut]
        public IHttpActionResult Update_Society([FromBody] SocietyModel sco)
        {
            bool updated;
            updated = sc_repository.updatesociety(sco);
            return Ok(updated);
        }
    }
    
}
