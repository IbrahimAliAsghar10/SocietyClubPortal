using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.db.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Schedule Table in FSCP Database
    public class ScheduleRepository
    {
        //This function will add the schedule detail in the table using object of class 'ScheduleModel'.
        //Test case number: 33
        public int Schedule(ScheduleModel model)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                SCHEDULE sch = new SCHEDULE()
                {
                    EVENT_NAME = model.EVENT_NAME,
                    VENUE_ID = model.VENUE_ID,
                    DATED = model.DATED,
                    START_TIME = model.START_TIME,
                    END_TIME = model.END_TIME,
                    SOCIETY_NAME = model.SOCIETY_NAME
                };
                context.SCHEDULE.Add(sch);
                context.SaveChanges();
                return 1;
            }
        }

        //This function will return all scheduled events of a society in a list.Society name mentioned in the parameter.
        //Test case number: 34
        public List<ScheduleModel> get_Sc_Events(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<ScheduleModel> sch = context.SCHEDULE.Where(x => x.SOCIETY_NAME == scname).Select(x => new ScheduleModel()
                {
                    VENUE_ID = x.VENUE_ID,
                    DATED = x.DATED,
                    EVENT_NAME = x.EVENT_NAME,
                    START_TIME = x.START_TIME,
                    END_TIME = x.END_TIME,
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    EVENT = new EventModel()
                    {
                        DESCRIPTION = x.EVENT.DESCRIPTION
                    },
                    VENUE = new VenueModel()
                    {
                        NAME = x.VENUE.NAME
                    }

                }).ToList();
                sch = sch.OrderBy(x => x.DATED).ToList();
                return sch;
            }
        }

        //This function will return all scheduled events of every Society in a list.
        //Test case number: 35
        public List<ScheduleModel> get_All_Sc_Events()
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<ScheduleModel> sch = context.SCHEDULE.Select(x => new ScheduleModel()
                {
                    VENUE_ID = x.VENUE_ID,
                    DATED = x.DATED,
                    EVENT_NAME = x.EVENT_NAME,
                    START_TIME = x.START_TIME,
                    END_TIME = x.END_TIME,
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    EVENT = new EventModel()
                    {
                        DESCRIPTION = x.EVENT.DESCRIPTION
                    },
                    VENUE = new VenueModel()
                    {
                        NAME = x.VENUE.NAME
                    }
                }).ToList();
                sch = sch.OrderBy(x => x.DATED).ToList();
                return sch;
            }
        }
    }
}

