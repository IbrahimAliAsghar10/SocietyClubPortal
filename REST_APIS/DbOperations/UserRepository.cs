using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocietyClubPortal.Models;

namespace REST_APIS.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Verify_Role Table in FSCP Database
    public class UserRepository
    {
        //Getting role information (Only ID )
        // Test case number:5
        public int get_role(string username)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var result = (from User in context.User where User.UserName == username select User.Role).FirstOrDefault();
                return result;
            }
        }
        public bool Verifying(UserModel model)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                bool IsValid = context.User.Any(x => x.Email == model.Email && x.Password == model.Password);
                return IsValid;
            }
        }

        public UserModel getUser(string source, string sourceName)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                if (sourceName == "Email")
                {
                    UserModel user = context.User.Where(x => x.Email == source).Select(x => new UserModel()
                    {
                        UserName = x.UserName,
                        Role = x.Role,
                        Password = x.Password,
                        Email = x.Email
                    }).FirstOrDefault();
                    return user;
                }
                else
                {
                    UserModel user = context.User.Where(x => x.UserName == source).Select(x => new UserModel()
                    {
                        UserName = x.UserName,
                        Role = x.Role,
                        Password = x.Password,
                        Email = x.Email
                    }).FirstOrDefault();
                    return user;
                }
            }
        }


        public bool updateUser(UserModel usr)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var scd = context.User.FirstOrDefault(x => x.UserName == usr.UserName);
                if (scd != null)
                {
                    scd.Password = usr.Password;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
