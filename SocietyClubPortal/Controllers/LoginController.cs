using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocietyClubPortal.Models;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Http;
using System.ComponentModel.DataAnnotations;

namespace SocietyClubPortal.Controllers
{
    //This controller will define which interface to login
    public class LoginController : Controller
    {
        HttpClient client =  null;
        //initializing the repositories for the possible users.(Constructor)
        public LoginController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64715/api/Login/");
        }

        //Initially this functional will be accessible by everyone on screen
        //Test case number: 37
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        //This functional will recieve the information from the user and redirect them to the appropriate page according to their ID and password.
        //Test case number: 38
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel usr = new UserModel();
                var response = client.PostAsJsonAsync<UserModel>(client.BaseAddress+ "Index", model);
                response.Wait();
                var test = response.Result;
                if (test.IsSuccessStatusCode)
                {
                    var display = test.Content.ReadAsAsync<UserModel>();
                    display.Wait();
                    usr = display.Result;
                    if (usr.Role == 0)
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Home", "Administrator");
                    }
                    else if (usr.Role == 1)
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Home", "Society");
                    }
                    else if (usr.Role == 2)
                    {
                        FormsAuthentication.SetAuthCookie(usr.UserName, false);
                        return RedirectToAction("Home", "Student");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ivalid UserName or Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", response.Result.ToString());
                }

            }
            else
            {
                ModelState.AddModelError("", "");
            }
            return View();
        }

        //This functionality will signout the user'sID and Redirect to the login page.
        //Test case number: 39
        [Authorize(Roles = "Admin,Society,Student")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}