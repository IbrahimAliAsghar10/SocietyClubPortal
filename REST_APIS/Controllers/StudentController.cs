using Newtonsoft.Json;
using REST_APIS.DbOperations;
using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Xml.Linq;

namespace REST_APIS.Controllers
{
    public class StudentController : ApiController
    {
        VenueRepository vn_repository = null;
        ScheduleRepository sch_repository = null;
        SocietyRepository sc_repository = null;
        StudentRepository st_repository = null;
        PostRepository pst_repository = null;
        Society_RegistrationRepository reg_repository = null;
        SOC_REG_IND_POSTRepository srip_repository = null;
        UserRepository usr_repository = null;


        //initializing the repositories for the possible users.(Constructor)
        public StudentController()
        {
            sc_repository = new SocietyRepository();
            vn_repository = new VenueRepository();
            sch_repository = new ScheduleRepository();
            st_repository = new StudentRepository();
            pst_repository = new PostRepository();
            reg_repository = new Society_RegistrationRepository();
            srip_repository = new SOC_REG_IND_POSTRepository();
            usr_repository = new UserRepository();
        }
        [HttpPost]
        public IHttpActionResult GetStudent([FromBody] string id){
        
            StudentModel stu = new StudentModel();
            stu = st_repository.getstudent(id);
            return Ok(stu);
        }
        [HttpGet]
        public IHttpActionResult View_Society_with_inductions([FromUri] string id)
        {
            List<SOC_REG_IND_POSTModel> sc = srip_repository.combine_data(id);
            return Ok(sc);
        }

        [HttpGet]
        public IHttpActionResult Register_Student([FromUri] string id,string sec_param)
        {
            bool registered;
            registered = reg_repository.register_student(sec_param, id);
            return Ok(registered);
        }

        [HttpGet]
        public IHttpActionResult View_Events([FromUri] string id)
        {
            List<ScheduleModel> Sch = sch_repository.get_Sc_Events(id);
            return Ok(Sch);
        }
        [HttpGet]
        public IHttpActionResult View_All_Events()
        {
            List<ScheduleModel> Sch = sch_repository.get_All_Sc_Events();
            return Ok(Sch);
        }


        [HttpGet]
        public IHttpActionResult View_Hierarchy([FromUri] string id)
        {
            List<PostModel> Sch = pst_repository.show_hierarchy(id);
            return Ok(Sch);
        }

        [HttpGet]
        public IHttpActionResult GetUser([FromUri] string id)
        {
            UserModel usr = usr_repository.getUser(id, "UserName");
            return Ok(usr);
        }
        [HttpPost]
        public IHttpActionResult Change_Password([FromBody] UserModel sco)
        {
            bool updated = usr_repository.updateUser(sco);
            return Ok(updated);
        }
    }
}
