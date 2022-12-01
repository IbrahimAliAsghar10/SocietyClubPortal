using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SocietyClubPortal.Controllers
{
    //This controller helps admin to perform their respective functions
    public class AdministratorController : Controller
    {
        HttpClient client = null;

        //initializing the repositories for the possible users.(Constructor)
        public AdministratorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64715/api/Administrator/");
        }

        // This function will display all the possible functions that admin can operate
        //Test case number: 40
        [Authorize(Roles = "Admin")]
        public ActionResult Home()
        {
            return View();
        }

        // This function will display a form to fill of student information to the admin.
        //Test case number: 41
        [Authorize(Roles = "Admin")]
        public ActionResult Addstudent()
        {
            return View();
        }


        //All student information form detail will be directed here and data will be added to the database
        //Test case number: 42
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddStudent(UserModel std)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                int opt;
                var response = client.PostAsJsonAsync<string>(client.BaseAddress + "AddStudent",std.Email);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {

                    var display = test.Content.ReadAsAsync<int>();
                    opt = display.Result;
                    if (opt == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Issucces = "Student Registered";
                    }
                    else if (opt == 2)
                    {
                        ModelState.AddModelError("", "Student already registered");
                    }
                    else
                    {
                        ModelState.AddModelError("", "There is no such email id exists");
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Data");
            }
            return View();
        }


        // This function will display a form to fill of society information to the admin.
        //Test case number: 43
        [Authorize(Roles = "Admin")]
        public ActionResult AddSociety()
        {
            return View();
        }

        //All student information form detail form will be directed here and data will be added to the database
        //Test case number: 44
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddSociety(SocietyModel sc)
        {
            if (ModelState.IsValid)
            {
                int opt;
                var response = client.PostAsJsonAsync<SocietyModel>(client.BaseAddress + "AddSociety", sc);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<int>();
                    opt = display.Result;
                    if (opt == 1)
                    {
                        ModelState.Clear();
                        ViewBag.Issucces = "Data Added";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Society already exists");
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Data");
            }

            return View();
        }


        //This function will gather all current students' information and display to the admin.
        //Test case number: 45
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Students_Data()
        {
            List<StudentModel> std = new List<StudentModel>();
            var response = client.GetAsync(client.BaseAddress + "Students_Data");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<StudentModel>>();
                std = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(std);
        }

        //To delete specific students' existence from the database.
        //Test case number: 46
        //[Authorize(Roles = "Admin")]
        //public ActionResult delete_student(string stid)
        //{
        //    if (st_repository.delete_student(stid))
        //    {
        //        return RedirectToAction("Students_Data", "Administrator");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Student tuple is not deleted");
        //        return RedirectToAction("Students_Data", "Administrator");
        //    }
        //}

        ////Dislay an update form for specific student's information.
        ////Test case number: 47
        //[Authorize(Roles = "Admin")]
        //public ActionResult Update_Student(string stid)
        //{
        //    StudentModel sc = new StudentModel();
        //    sc = st_repository.getstudent(stid);
        //    return View(sc);
        //}

        ////Student's updated information will be recieved here and this function will update into the database.
        ////Test case number: 48
        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //public ActionResult Update_Student(StudentModel sco)
        //{
        //    if (st_repository.updatestudent(sco))
        //    {
        //        return RedirectToAction("Students_Data", "Administrator");
        //    }
        //    return View();
        //}


        //This function will gather all current societies' information and display to the admin
        //Test case number: 49
        [Authorize(Roles = "Admin")]
        public ActionResult Societies_Data()
        {
            List<SocietyModel> sc = new List<SocietyModel>();
            var response = client.GetAsync(client.BaseAddress + "Societies_Data");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<SocietyModel>>();
                sc = display.Result;
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
            }
            return View(sc);
        }


        //To delete specific societies' existence from the database.
        //Test case number: 50
        [Authorize(Roles = "Admin")]
        public ActionResult delete_society(string scname)
        {
            bool del_society;
            var response = client.DeleteAsync(client.BaseAddress + "delete_society/"+ scname);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                del_society = display.Result;
                if (del_society)
                {
                    return RedirectToAction("Societies_Data", "Administrator");
                }
                else
                {
                    ModelState.AddModelError("", "Society tuple is not deleted");
                    return RedirectToAction("Societies_Data", "Administrator");
                }
            }
            else
            {
                ModelState.AddModelError("", response.Result.ToString());
                return RedirectToAction("Societies_Data", "Administrator");
            }
            
        }


        //Dislay an update form for specific society's information.
        //Test case number: 51
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Update_Society(string scname)
        {
            SocietyModel sc = new SocietyModel();

            TempData["scnamededo"] = scname;
            var response = client.GetAsync(client.BaseAddress + "Update_Society/"+ scname);
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

        //Society's updated information will be recieved here and this function will update into the database.
        //Test case number: 52
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Update_Society(SocietyModel sco)
        {
            bool opt;
            sco.NAME = TempData["scnamededo"].ToString();
            var response = client.PutAsJsonAsync<SocietyModel>(client.BaseAddress + "Update_Society", sco);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<bool>();
                opt = display.Result;
                if (opt)
                {
                    return RedirectToAction("Societies_Data", "Administrator");
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