using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace REST_APIS.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Society Table in FSCP Database
    public class SocietyRepository
    {
        Society_RegistrationRepository reg_repository = null;

        //initializing the object of required RegistrationRepository in this class.
        public SocietyRepository()
        {
            reg_repository = new Society_RegistrationRepository();
        }

        //This function will add the society in the society table
        //Test case number: 3
        public int AddSociety(SocietyModel model)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                SOCIETY scu = new SOCIETY()
                {
                    NAME = model.NAME,
                    DESCRIPTION = model.DESCRIPTION,
                    INDUCTION_STATUS = false,
                    EMAIL_LOGIN = model.EMAIL_LOGIN
                };
                //Adding the society name in the VErify_Role table so that the society can login.
                User vrf = new User()
                {
                    UserName = model.NAME,
                    Email = model.EMAIL_LOGIN,
                    Role = 1,
                    Password = "123"
                };
                if (context.SOCIETY.Any(x => x.NAME == model.NAME))
                {
                    return 0;
                }
                else
                {
                    context.SOCIETY.Add(scu);
                    context.User.Add(vrf);
                    context.SaveChanges();
                    return 1;
                }
            }
        }

        //This function will verify whether the login details meet any society data in the table
        //Test case number: 4 
        //public bool Verifying(UserModel model)
        //{
        //    using (var context = new SocietyClubPortalEntities())
        //    {
        //        bool IsValid = context.SOCIETY.Any(x => x.EMAIL_LOGIN == model.ID && x.SC_PASSWORD == model.PASSWORD);
        //        return IsValid;
        //    }
        //}

        //This function will return all details of societies in an object. 
        //Test case number: 10
        public SocietyModel getsociety(string sc_name)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                SocietyModel sc = context.SOCIETY.Where(x => x.NAME == sc_name).Select(x => new SocietyModel()
                {
                    NAME = x.NAME,
                    DESCRIPTION = x.DESCRIPTION,
                    INDUCTION_STATUS = x.INDUCTION_STATUS,
                    EMAIL_LOGIN = x.EMAIL_LOGIN,
                }).FirstOrDefault();
                return sc;
            }
        }

        //This function will return UserName of society in an object. 
        //Test case number: --
        public string getUserName(string email)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var result = (from User in context.SOCIETY where User.EMAIL_LOGIN == email select User.NAME).FirstOrDefault();
                return result;
            }
        }

        //This function will return all societies with details in a list of objects.
        //Test case number: 11
        public List<SocietyModel> get_all_society()
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<SocietyModel> sc = context.SOCIETY.Select(x => new SocietyModel()
                {
                    NAME = x.NAME,
                    DESCRIPTION = x.DESCRIPTION,
                    INDUCTION_STATUS = x.INDUCTION_STATUS,
                    EMAIL_LOGIN = x.EMAIL_LOGIN
                }).ToList();
                return sc;
            }
        }


        //This function will delete the society data from the table. Society name mentioned in the parameter.
        //Test case number: 12
        public bool delete_society(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<ScheduleModel> schedules = context.SCHEDULE.Where(x => x.SOCIETY_NAME == scname).Select(x => new ScheduleModel()
                {
                    VENUE_ID = x.VENUE_ID,
                    DATED = x.DATED,
                    START_TIME = x.START_TIME,
                    END_TIME = x.END_TIME
                }).ToList();

                //If the society has any scheduled events, then it will be deleted first.
                if (schedules != null)
                {
                    foreach (var schedule in schedules)
                    {
                        DateTime date = schedule.DATED;
                        TimeSpan std_time = schedule.START_TIME;
                        TimeSpan etd_time = schedule.END_TIME;
                        int id = schedule.VENUE_ID;
                        while (std_time < etd_time)
                        {
                            var vnue = context.AVAILABILITY_OF_VENUE.FirstOrDefault(x => x.VENUE_ID == id && x.START_TIME == std_time && x.DATED == date);
                            if (vnue != null)
                            {
                                vnue.IS_BOOK = false;
                                context.SaveChanges();
                            }
                            std_time += TimeSpan.FromHours(1);
                        }
                    }
                    var all_schedule = context.SCHEDULE.Where(x => x.SOCIETY_NAME == scname).ToList();
                    foreach (var scheduled in all_schedule)
                    {
                        if (scheduled != null)
                        {
                            context.SCHEDULE.Remove(scheduled);
                        }

                    }
                }
                context.SaveChanges();

                var allevents = context.EVENT.Where(x => x.SOCIETY_NAME == scname).ToList();

                //If the society has any events, it will be deleted .
                if (allevents != null)
                {
                    foreach (var evento in allevents)
                    {
                        if (evento != null)
                        {
                            context.EVENT.Remove(evento);
                        }

                    }
                }

                var reg_societies = context.SOCIETY_REGISTRATION.Where(x => x.SOCIETY_NAME == scname).ToList();

                //If the society has any registrations, it will be deleted.
                if (reg_societies != null)
                {
                    foreach (var reg_no in reg_societies)
                    {
                        if (reg_no != null)
                        {
                            context.SOCIETY_REGISTRATION.Remove(reg_no);
                        }

                    }
                }

                var psts = context.POST.Where(x => x.SOCIETY_NAME == scname).ToList();

                //If the society has any posts, it will be deleted.
                if (psts != null)
                {
                    foreach (var post in psts)
                    {
                        if (post != null)
                        {
                            context.POST.Remove(post);
                        }

                    }
                }
                //var email = (from User in context.SOCIETY where User.NAME == scname select User.EMAIL_LOGIN).FirstOrDefault();
                var soc = context.SOCIETY.FirstOrDefault(x => x.NAME == scname);
                context.SOCIETY.Remove(soc);
                var verify_role = context.User.FirstOrDefault(x => x.UserName == scname);
                context.User.Remove(verify_role);
                context.SaveChanges();
                return true;

            }
        }


        //This function will update the existing society details. Society name is mentioned in the parameter.
        //Test case number: 13
        public bool updatesociety(SocietyModel sco)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var scd = context.SOCIETY.FirstOrDefault(x => x.NAME == sco.NAME);
                if (scd != null)
                {
                    scd.DESCRIPTION = sco.DESCRIPTION;
                    scd.INDUCTION_STATUS = sco.INDUCTION_STATUS;

                    //If the society induction is close then remove all the registration, done for the society.
                    if (!sco.INDUCTION_STATUS)
                    {
                        reg_repository.remove_society_registration(sco.NAME);
                    }
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //This function will update the existing society description. Society name is mentioned in the parameter.
        //Test case number: 13
        public bool updatedescription(SocietyModel sco)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var scd = context.SOCIETY.FirstOrDefault(x => x.NAME == sco.NAME);
                if (scd != null)
                {
                    scd.DESCRIPTION = sco.DESCRIPTION;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //This function will return the induction status of mentioned society name in the parameter.
        //Test case number: 21
        public bool check_induction(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                SocietyModel ind = context.SOCIETY.Where(x => x.NAME == scname).Select(x => new SocietyModel()
                {
                    INDUCTION_STATUS = x.INDUCTION_STATUS
                }
                ).FirstOrDefault();
                return ind.INDUCTION_STATUS;
            }
        }

        //This function will update the induction status of the mentioned society name in the parameter.
        //Test case number: 22
        public bool update_inductions(bool status, string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var ind = context.SOCIETY.FirstOrDefault(x => x.NAME == scname);
                if (ind != null)
                {
                    ind.INDUCTION_STATUS = status;
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

