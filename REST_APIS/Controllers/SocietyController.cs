using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using REST_APIS.DbOperations;
using SocietyClubPortal.Models;

namespace REST_APIS.Controllers
{
    public class SocietyController : ApiController
    {

        EventRepository ev_repository = null;
        VenueRepository vn_repository = null;
        Availability_Of_VenueRepository aov_repository = null;
        ScheduleRepository sch_repository = null;
        SocietyRepository sc_repository = null;
        PostRepository pst_repository = null;
        Society_RegistrationRepository reg_repository = null;
        UserRepository usr_repository = null;

        //initializing the repositories for the possible users.(Constructor)
        public SocietyController()
        {
            sc_repository = new SocietyRepository();
            ev_repository = new EventRepository();
            vn_repository = new VenueRepository();
            aov_repository = new Availability_Of_VenueRepository();
            sch_repository = new ScheduleRepository();
            pst_repository = new PostRepository();
            reg_repository = new Society_RegistrationRepository();
            usr_repository = new UserRepository();
        }
        [HttpGet]
        public IHttpActionResult GetSociety([FromUri] string id)
        {
            SocietyModel sc = new SocietyModel();
            sc = sc_repository.getsociety(id);
            return Ok(sc);
        }
        [HttpGet]
        public IHttpActionResult get_Schedule_events([FromUri] string id)
        {
            List<ScheduleModel> Sch = sch_repository.get_Sc_Events(id);
            return Ok(Sch);
        }
        [HttpGet]
        public IHttpActionResult get_UnSchedule_events([FromUri] string id)
        {
            List<EventModel> Sch = ev_repository.get_UnSc_Events(id);
            return Ok(Sch);
        }
        [HttpPost]
        public IHttpActionResult AddEvent([FromBody] EventModel ev)
        {
            int opt = ev_repository.AddEvent(ev);
            return Ok(opt);
        }
        [HttpDelete]
        public IHttpActionResult delete_event(string id,string sec_param)
        {
            bool deleted = ev_repository.delete_event(id, sec_param);
            return Ok(deleted);
        }
        [HttpPost]
        public IHttpActionResult Add_new_post([FromBody] PostModel pst)
        {
            bool added = pst_repository.AddPost(pst);
            return Ok(added);
        }
        [HttpDelete]
        public IHttpActionResult Delete_Post([FromUri] string id, string sec_param)
        {
            bool deleted = pst_repository.DeletePost(id, sec_param);
            return Ok(deleted);
        }
        [HttpPost]
        public IHttpActionResult abc([FromBody] Dummy hi)
        {
            List<VenueModel> Venues = vn_repository.find_Venues(hi);
            return Ok(Venues);
        }
        [HttpGet]
        public IHttpActionResult Dates()
        {
            List<Availability_Of_VenueModel> dates = aov_repository.bring();
            return Ok(dates);
        }
        [HttpPost]
        public IHttpActionResult Scheduling([FromBody] ScheduleModel sch)
        {
            int opt = sch_repository.Schedule(sch);
            return Ok(opt);
        }
        [HttpPost]
        public IHttpActionResult UpdateBooking([FromBody] ScheduleModel sch)
        {
            bool updated = aov_repository.update_booking(sch);
            return Ok(updated);
        }
        [HttpPost]
        public IHttpActionResult Update_Description([FromBody] SocietyModel sco)
        {
            bool updated = sc_repository.updatedescription(sco);
            return Ok(updated);
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
        [HttpGet]
        public IHttpActionResult get_unfilled_posts([FromUri] string id)
        {
            List<PostModel> uf_psts = pst_repository.unfilled_posts(id);
            return Ok(uf_psts);
        }
        [HttpGet]
        public IHttpActionResult get_filled_posts([FromUri] string id)
        {
            List<PostModel> f_psts = pst_repository.filled_posts(id);
            return Ok(f_psts);
        }

        [HttpGet]
        public IHttpActionResult Inductions([FromUri] string id)
        {
            bool status = sc_repository.check_induction(id);
            return Ok(status);
        }

        [HttpGet]
        public IHttpActionResult RemoveRegistrations([FromUri] string id)
        {
            bool result = reg_repository.remove_society_registration(id);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult UpdateInductionStatus([FromUri] string id, bool sec_param)
        {
            bool result = sc_repository.update_inductions(sec_param, id);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult Change_Student_on_post([FromUri] string id)
        {
            List<Society_RegistrationModel>  reg_std = reg_repository.Get_Registered_Students(id);
            return Ok(reg_std);
        }
        [HttpGet]
        public IHttpActionResult Changing_Student_on_post([FromUri] string id, string sec_param,string third_param,string fourth_param)
        {
            bool result = pst_repository.student_on_post_updated(third_param, fourth_param, id, sec_param);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult RemoveFromRegistrations([FromUri] int id)
        {
            bool status = reg_repository.remove_from_registration(id);
            return Ok(status);
        }
        [HttpGet]
        public IHttpActionResult Add_Student_on_post([FromUri] string id)
        {
            List<Society_RegistrationModel>  reg_std = reg_repository.Get_Registered_Students(id);
            return Ok(reg_std);
        }
        [HttpGet]
        public IHttpActionResult Adding_Student_on_post([FromUri] string id, string sec_param, string third_param)
        {
            bool result = pst_repository.student_on_post_added(third_param, id, sec_param);
            return Ok(result);
        }
    }



}
