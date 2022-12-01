using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Security;

namespace SocietyClubPortal
{
    public class Interface_Role_Provider : RoleProvider
    {
        HttpClient client = null;
        public Interface_Role_Provider()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:64715/api/Login/");
        }
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            int result;

            string[] arr = new string[1];
            var response = client.GetAsync(client.BaseAddress + "GetRole/" + username);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<int>();
                result = display.Result;
                if ((result == 0))
                {
                    arr[0] = "Admin";
                }
                else if (result == 1)
                {
                    arr[0] = "Society";
                }
                else
                {
                    arr[0] = "Student";
                }
            }
            return arr;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}