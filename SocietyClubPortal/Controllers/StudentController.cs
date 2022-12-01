using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SocietyClubPortal.Controllers
{
    //This controller is used to manage students functions.
    public class StudentController : Controller
    {

        HttpClient client = null;


        //initializing the repositories for the possible users.(Constructor)
        public StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64715/api/Student/");
        }


        //This function will display a page to student for their personal information.
        //Test case number: 53
        [Authorize(Roles = "Student")]
        public ActionResult Home()
        {
            StudentModel stu = new StudentModel();
            var response = client.PostAsJsonAsync(client.BaseAddress + "GetStudent" ,User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<StudentModel>();
                stu = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(stu);
        }


        //This function will display all societies Name, their induction status towards the individual student.
        //Test case number:54
        [Authorize(Roles = "Student")]
        public ActionResult View_Society()
        {
            List<SOC_REG_IND_POSTModel> sc = new List<SOC_REG_IND_POSTModel>();
            var response = client.GetAsync(client.BaseAddress + "View_Society_with_inductions/" + User.Identity.Name);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<SOC_REG_IND_POSTModel>>();
                sc = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(sc);
        }


        //This function will let studen to register for the inductions in interested society.
        //Test case number:55
        [Authorize(Roles = "Student")]
        public ActionResult Register_Student(string scname)
        {
            bool registered;
            var response = client.GetAsync(client.BaseAddress + "Register_Student/" + User.Identity.Name + "/" + scname);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                registered = display.Result;
                if (registered)
                {
                    return RedirectToAction("View_Society", "Student");
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


        //This function will display the events of society whose name is recieved.
        //Test case number:56
        [Authorize(Roles = "Student")]
        public ActionResult View_Events(string scname)
        {
            ViewBag.Society_Name = scname;
            List<ScheduleModel> Sch = new List<ScheduleModel>();
            var response = client.GetAsync(client.BaseAddress + "View_Events/" + scname);
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
            return View(Sch);
        }


        //This function will Display all Societies events in order by date.
        //Test case number:57
        [Authorize(Roles = "Student")]
        public ActionResult View_All_Events()
        {
            List<ScheduleModel> Sch = new List<ScheduleModel>();
            var response = client.GetAsync(client.BaseAddress + "View_All_Events");
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
            return View(Sch);
        }


        //This function will display the hierarchy of recieved society name.
        //Test case number:58
        [Authorize(Roles = "Student")]
        public ActionResult View_Hierarchy(string scname)
        {
            ViewBag.scname = scname;
            List<PostModel> psts = new List<PostModel>();
            var response = client.GetAsync(client.BaseAddress + "View_Hierarchy/" + scname);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<PostModel>>();
                psts = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            
            return View(psts);
        }
        [Authorize(Roles = "Student")]
        [HttpGet]
        public ActionResult Change_Password(string username)
        {
            UserModel usr = new UserModel();
            var response = client.GetAsync(client.BaseAddress + "GetUser/" + username);
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
        [Authorize(Roles = "Student")]
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
                    return RedirectToAction("Home", "Student");
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