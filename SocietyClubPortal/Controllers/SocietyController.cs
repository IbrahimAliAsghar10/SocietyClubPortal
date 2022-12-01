using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SocietyClubPortal.Controllers
{
    //This controller manages all functions necessary for the society head to execute.
    public class SocietyController : Controller
    {
        HttpClient client = null;

        //initializing the repositories for the possible users.(Constructor)
        public SocietyController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64715/api/Society/");
        }


        //This function will display a page to society for their personal information.
        //Test case number: 59
        [Authorize(Roles = "Society")]
        public ActionResult Home()
        {
            SocietyModel sc = new SocietyModel();
            var response = client.GetAsync(client.BaseAddress + "GetSociety/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<SocietyModel>();
                sc = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(sc);
        }


        //This function will bring all the schedule events of User Society.
        //Test case number: 60
        [Authorize(Roles = "Society")]
        public List<ScheduleModel> get_Schedule_events()
        {
            List<ScheduleModel> Sch = new List<ScheduleModel>();
            var response = client.GetAsync(client.BaseAddress + "get_Schedule_events/"+ User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<ScheduleModel>>();
                Sch = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return Sch;
        }


        //This function will bring all the unschedule events of User Society.
        //Test case number: 61
        [Authorize(Roles = "Society")]
        public List<EventModel> get_UnSchedule_events()
        {
            List<EventModel> Sch = new List<EventModel>();
            var response = client.GetAsync(client.BaseAddress + "get_UnSchedule_events/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<EventModel>>();
                Sch = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return Sch;
        }


        //This function will bring all the scheduled and unscheduled events in a virtual model 'EventsVM' and display to User Society.
        //Test case number: 62
        [Authorize(Roles = "Society")]
        public ActionResult Events()
        {
            EventVM model = new EventVM();
            model.Schedule_VM = get_Schedule_events();
            model.NonSchedule_VM = get_UnSchedule_events();
            return View(model);
        }


        //This function will display the new event form to User Scoiety.
        //Test case number: 63
        [Authorize(Roles = "Society")]
        public ActionResult AddEvent()
        {
            return View();
        }

        //This function will recieve new event's information and will add it to the database.
        //Test case number: 64
        [HttpPost]
        [Authorize(Roles = "Society")]
        public ActionResult AddEvent(EventModel ev, string Schedule)
        {
            if (ModelState.IsValid)
            {

                //Used to convey the same information to any other function of the same class only once.
                TempData["Event_Name_from_events"] = ev.NAME;

                //User.Identity.Name contains the current User Society name. 
                ev.SOCIETY_NAME = User.Identity.Name;

                //If you want to schedule the current event also 
                if (Schedule == "Add Details And Continue To Schedule")
                {
                    int opt;
                    var response = client.PostAsJsonAsync<EventModel>(client.BaseAddress + "AddEvent", ev);
                    response.Wait();
                    var test = response.Result;
                    if (test.IsSuccessStatusCode)
                    {
                        var display = test.Content.ReadAsAsync<int>();
                        opt = display.Result;
                        if (opt == 1)
                        {
                            ModelState.Clear();
                            return RedirectToAction("Schedule", "Society");
                        }
                        else
                        {
                            ModelState.AddModelError("", "This Event already exists for your society");
                            return View(ev);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Result.ToString());
                    }
                }
                //else If you want to only add th details of the event so that it can be scheduled later anytime Society wants to
                else if (Schedule == "Add Details")
                {
                    int opt;
                    var response = client.PostAsJsonAsync<EventModel>(client.BaseAddress + "AddEvent", ev);
                    response.Wait();
                    var test = response.Result;
                    if (test.IsSuccessStatusCode)
                    {
                        var display = test.Content.ReadAsAsync<int>();
                        opt = display.Result;
                        if (opt == 1)
                        {
                            ModelState.Clear();
                            return RedirectToAction("Events", "Society");
                        }
                        else
                        {
                            ModelState.AddModelError("", "This Event already exists for your society");
                            return View(ev);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Result.ToString());
                    }
                }
            }
            return View();
        }


        //To delete particular event of society though, its scheduled or not.
        //Test case number: 65
        [Authorize(Roles = "Society")]
        public ActionResult delete_event(string ename)
        {
            if (ModelState.IsValid)
            {
                bool del_event;
                var response = client.DeleteAsync(client.BaseAddress + "delete_event/" + User.Identity.Name+"/"+ ename);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<bool>();
                    del_event = display.Result;
                    if (del_event)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Events", "Society");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This event does not exists");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            return View();
        }


        //This function will display the new post form to User Scoiety.
        //Test case number: 66
        [Authorize(Roles = "Society")]
        public ActionResult Add_new_post()
        {
            return View();
        }

        //This function will recieve new post's information and will add it to the database.
        //Test case number: 67
        [Authorize(Roles = "Society")]
        [HttpPost]
        public ActionResult Add_new_post(PostModel pst)
        {
            pst.SOCIETY_NAME = User.Identity.Name;
            if (ModelState.IsValid)
            {
                bool added;
                var response = client.PostAsJsonAsync<PostModel>(client.BaseAddress + "Add_new_post", pst);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<bool>();
                    added = display.Result;
                    if (added)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Inductions", "Society");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This Post already exists for your society");
                        return View(pst);
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            return View();
        }


        //To delete particular post of society although the post is already filled by a person in a hierarchy.
        //Test case number: 68
        [Authorize(Roles = "Society")]
        public ActionResult Delete_Post(string pname)
        {
            if (ModelState.IsValid)
            {
                bool del_post;
                var response = client.DeleteAsync(client.BaseAddress + "Delete_Post/" + User.Identity.Name + "/" + pname);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<bool>();
                    del_post = display.Result;
                    if (del_post)
                    {
                        ModelState.Clear();
                        return RedirectToAction("Inductions", "Society");
                    }
                    else
                    {
                        ModelState.AddModelError("", "This Post does not exists");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            return View();
        }


        //Thus function will send the available venues on the required time slot.
        //Test case number: 69
        [Authorize(Roles = "Society")]
        [HttpPost]
        public JsonResult abc(Dummy hi)
        {
            List<VenueModel> Venues = new List<VenueModel>();
            var response = client.PostAsJsonAsync(client.BaseAddress + "abc",hi);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<VenueModel>>();
                Venues = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            ViewBag.something = Venues;//Directly sent to the display page.
            return Json(Venues, JsonRequestBehavior.AllowGet);
        }


        //This function will display schedule event form event and help to select the available slotsa and venue.
        //Test case number: 70
        [Authorize(Roles = "Society")]
        public ActionResult Schedule(string ename)
        {
            ScheduleModel sch = new ScheduleModel();
            if (ename != null)//If the event scheduling is done separately sometime later.
            {
                sch.EVENT_NAME = ename;
            }
            else //If the scheduling is done at the time of event creation then the event name is carried through temp Data from the function AddEvent.
            {
                sch.EVENT_NAME = (string)TempData["Event_Name_from_events"];
            }
            var start = new List<string>() { "08:00:00", "09:00:00", "10:00:00", "11:00:00", "12:00:00", "13:00:00", "14:00:00", "15:00:00" };

            ViewBag.start = start; //ViewBag directly sends data to the 'start' variable on .cshtml file.

            var end = new List<string>() { "09:00:00", "10:00:00", "11:00:00", "12:00:00", "13:00:00", "14:00:00", "15:00:00", "16:00:00" };
            ViewBag.end = end;
            List<Availability_Of_VenueModel> dates = new List<Availability_Of_VenueModel>();
            var response = client.GetAsync(client.BaseAddress + "Dates");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Availability_Of_VenueModel>>();
                dates = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            List<DateTime> datee = new List<DateTime>();
            foreach (var dat in dates)
            {
                datee.Add(dat.DATED);
            }
            ViewBag.dater = datee;
            return View(sch);
        }

        //All these schedule detail will be recieved in this function and will be added to the database.
        //Test case number: 71
        [Authorize(Roles = "Society")]
        [HttpPost]
        public ActionResult Schedule(ScheduleModel sch)
        {
            sch.SOCIETY_NAME = User.Identity.Name;
            if (ModelState.IsValid)
            {
                int  opt;
                var response = client.PostAsJsonAsync<ScheduleModel>(client.BaseAddress + "Scheduling", sch);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<int>();
                    opt = display.Result;
                    if (opt == 1)
                    {
                        bool booking;
                        response = client.PostAsJsonAsync<ScheduleModel>(client.BaseAddress + "UpdateBooking", sch);
                        test = response.Result;
                        if (test.IsSuccessStatusCode)
                        {
                            var displayed = test.Content.ReadAsAsync<bool>();
                            booking = displayed.Result;
                            ModelState.Clear();
                            if (booking)
                            {
                                return RedirectToAction("Events", "Society");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Scheduling failed");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", response.Result.ToString());
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Scheduling failed");
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Ivalid Details");
            }
            return View();
        }


        //This function will display socity description update form with the previous details which can be edited.
        //Test case number: 72
        [Authorize(Roles = "Society")]
        [HttpGet]
        public ActionResult Update_Description(string scname)
        {
            SocietyModel sc = new SocietyModel();
            var response = client.GetAsync(client.BaseAddress + "GetSociety/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<SocietyModel>();
                sc = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(sc);
        }

        //Updated detail of society description will be recieved here and added to the database.
        //Test case number: 73
        [Authorize(Roles = "Society")]
        [HttpPost]
        public ActionResult Update_Description(SocietyModel sco)
        {
            sco.NAME = User.Identity.Name;
            bool updated;
            var response = client.PostAsJsonAsync(client.BaseAddress + "Update_Description", sco);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                updated = display.Result;
                if (updated)
                {
                    return RedirectToAction("Home", "Society");
                }
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            
            return View();
        }
        [HttpGet]
        public ActionResult Change_Password(string username)
        {
            UserModel usr = new UserModel();
            var response = client.GetAsync(client.BaseAddress + "GetUser/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<UserModel>();
                usr = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(usr);
        }

        //Updated detail of society description will be recieved here and added to the database.
        //Test case number: 73
        [Authorize(Roles = "Society")]
        [HttpPost]
        public ActionResult Change_Password(UserModel sco)
        {
            sco.UserName = User.Identity.Name;
            bool updated;
            var response = client.PostAsJsonAsync(client.BaseAddress + "Change_Password", sco);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                updated = display.Result;
                if (updated)
                {
                    return RedirectToAction("Home", "Society");
                }
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View();
        }


        //This function wil get all those post which are taken by any of the students.
        //Test case number: 74
        [Authorize(Roles = "Society")]
        public List<PostModel> get_filled_posts()
        {
            List<PostModel> f_psts = new List<PostModel>();
            var response = client.GetAsync(client.BaseAddress + "get_filled_posts/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<PostModel>>();
                f_psts = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return f_psts;
        }


        //This function wil get all those post which are not taken by any of the students.
        //Test case number: 75
        [Authorize(Roles = "Society")]
        public List<PostModel> get_unfilled_posts()
        {
            List<PostModel> uf_psts = new List<PostModel>();
            var response = client.GetAsync(client.BaseAddress + "get_unfilled_posts/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<PostModel>>();
                uf_psts = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return uf_psts;
        }


        //This function will display the information of filled and unfilled posts of society using Virtual Model 'InductionsVM' to the society.
        //Test case number: 76
        [Authorize(Roles = "Society")]
        [HttpGet]
        public ActionResult Inductions()
        {
            InductionsVM model = new InductionsVM();
            var response = client.GetAsync(client.BaseAddress + "Inductions/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                ViewBag.Inductions_Open = display.Result;
                model.filled_VM = get_filled_posts();
                model.Unfilled_VM = get_unfilled_posts();
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(model);
        }


        //This function will open or close the inductions of the society.
        //Test case number: 77
        [Authorize(Roles = "Society")]
        public ActionResult Induction_status(bool status)
        {
            if (!status)
            {
                bool removed;
                var response = client.GetAsync(client.BaseAddress + "RemoveRegistrations/" + User.Identity.Name);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<bool>();
                    removed = display.Result;
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            bool updated;
            var response2 = client.GetAsync(client.BaseAddress + "UpdateInductionStatus/" + User.Identity.Name+ "/" + status);
            response2.Wait();
            var test2 = response2.Result;
            if (test2.IsSuccessStatusCode)
            {
                var display = test2.Content.ReadAsAsync<bool>();
                updated = display.Result;
                if (!updated)
                {
                    ModelState.AddModelError("", "Induction table is not updated");
                }
            }
            else
            {
                ModelState.AddModelError("", response2.Result.ToString());
            }
            
            return RedirectToAction("Inductions", "Society");
        }


        //This function is used to display options for changing existing student from the post.
        //Test case number: 78
        [Authorize(Roles = "Society")]
        public ActionResult Change_Student_on_post(string stid, string pname)
        {
            TempData["stid"] = stid;
            TempData["p_upd_name"] = pname;
            ViewBag.postname = pname;
            List<Society_RegistrationModel> reg_std = new List<Society_RegistrationModel>();
            var response = client.GetAsync(client.BaseAddress + "Change_Student_on_post/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Society_RegistrationModel>>();
                reg_std = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(reg_std);
        }


        //This function will change the existing student from post and the post will be updated to the database by new student
        //Test case number: 79
        [Authorize(Roles = "Society")]
        public ActionResult Changing_Student_on_post(string st_new, int reg_std)
        {
            string stid = TempData["stid"].ToString();
            string pname = TempData["p_upd_name"].ToString();
            bool updated;
            var response = client.GetAsync(client.BaseAddress + "Changing_Student_on_post/" + User.Identity.Name + "/" + pname + "/" + stid + "/" + st_new);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                updated = display.Result;
                if (updated)
                {
                    bool removed;
                    response = client.GetAsync(client.BaseAddress + "RemoveFromRegistrations/" + reg_std);
                    response.Wait();
                    test = response.Result;
                    if (test.IsSuccessStatusCode)
                    {
                        var displayed = test.Content.ReadAsAsync<bool>();
                        removed = displayed.Result;
                        if (removed)
                        {
                            return RedirectToAction("Inductions", "Society");
                        }
                        else
                        {
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Result.ToString());
                    }
                    
                }
                else
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View();
        }


        //This function is used to display options for adding student on Post
        //Test case number: 80
        [Authorize(Roles = "Society")]
        public ActionResult Add_Student_on_post(string pname)
        {
            TempData["p_add_name"] = pname;
            ViewBag.postname = pname;
            List<Society_RegistrationModel> reg_std = new List<Society_RegistrationModel>();
            var response = client.GetAsync(client.BaseAddress + "Add_Student_on_post/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Society_RegistrationModel>>();
                reg_std = display.Result;
                }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }

            return View(reg_std);
        }


        //This function will recieve students induction on specific post and add the selected student on post to the database
        //Test case number: 81
        [Authorize(Roles = "Society")]
        public ActionResult Adding_Student_on_post(string stid, int reg_std)
        {
            string pname = TempData["p_add_name"].ToString();
            bool updated;
            var response = client.GetAsync(client.BaseAddress + "Adding_Student_on_post/" + User.Identity.Name + "/" + pname + "/" + stid);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                updated = display.Result;
                if (updated)
                {
                    bool removed;
                    response = client.GetAsync(client.BaseAddress + "RemoveFromRegistrations/" + reg_std);
                    response.Wait();
                    test = response.Result;
                    if (test.IsSuccessStatusCode)
                    {
                        var displayed = test.Content.ReadAsAsync<bool>();
                        removed = displayed.Result;
                        if (removed)
                        {
                            return RedirectToAction("Inductions", "Society");
                        }
                        else
                        {
                            return RedirectToAction("Adding_Student_on_post", "Society");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Result.ToString());
                    }

                }
                else
                {
                    return RedirectToAction("Adding_Student_on_post", "Society");
                }
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View();
        }
    }
}
