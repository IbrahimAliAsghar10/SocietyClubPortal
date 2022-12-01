using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_APIS.DbOperations
{

    //This class is used to perform Read,Update,Delete operations of SOCIETY_REGISTRATION Table in FSCP Database
    public class Society_RegistrationRepository
    {
        //This function will register a student in particular society using st_id and scname mentioned in the parameter.
        //Test case number: 23
        public bool register_student(string scname, string stid)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                SOCIETY_REGISTRATION reg_std = new SOCIETY_REGISTRATION()
                {
                    SOCIETY_NAME = scname,
                    STUDENT_ID = stid
                };
                if (context.SOCIETY_REGISTRATION.Any(x => x.SOCIETY_NAME == scname && x.STUDENT_ID == stid))
                {
                    return false;
                }
                else
                {
                    context.SOCIETY_REGISTRATION.Add(reg_std);
                    context.SaveChanges();
                    return true;
                }
            }
        }


        //This function will return all the students registered for the society, which is mentioned in the parameter.
        //Test case number: 24
        public List<Society_RegistrationModel> Get_Registered_Students(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List< Society_RegistrationModel> reg_std = context.SOCIETY_REGISTRATION.Where(x => x.SOCIETY_NAME == scname).Select(x => new Society_RegistrationModel()
                {
                    REGISTRATION_NO = x.REGISTRATION_NO,
                    STUDENT_ID = x.STUDENT_ID,
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    student = new StudentModel()
                    {
                        FNAME = x.student.fname,
                        CGPA = x.student.cgpa
                    }
                }).ToList();
                reg_std = reg_std.OrderByDescending(x => x.student.CGPA).ToList();
                return reg_std;
            }
        }

        //This function will remove the SOCIETY_REGISTRATION of the student reg_no mentioned in the parameter.
        //Test case number: 25
        public bool remove_from_registration(int regno)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var psts = context.SOCIETY_REGISTRATION.FirstOrDefault(x => x.REGISTRATION_NO == regno);
                if (psts != null)
                {
                    context.SOCIETY_REGISTRATION.Remove(psts);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // This function will remove SOCIETY_REGISTRATION of all students who registered for the society,which is mentioned in the parameter
        //Test case number: 26
        public bool remove_society_registration(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var psts = context.SOCIETY_REGISTRATION.Where(x => x.SOCIETY_NAME == scname).ToList();
                foreach (var item in psts)
                {
                    if (item != null)
                    {
                        context.SOCIETY_REGISTRATION.Remove(item);
                        context.SaveChanges();
                    }
                }
                return true;
            }
        }
    }
}
