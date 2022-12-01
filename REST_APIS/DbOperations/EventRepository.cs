using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_APIS.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Events Table in FSCP Database
    public class EventRepository
    {
        //This function will add the event to the database collected from the object of class 'EventsModel'.
        ////Test case number: 27
        public int AddEvent(EventModel model)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                EVENT ev = new EVENT()
                {
                    NAME = model.NAME,
                    DESCRIPTION = model.DESCRIPTION,
                    SOCIETY_NAME = model.SOCIETY_NAME
                };
                if (context.EVENT.Any(x => x.SOCIETY_NAME == model.SOCIETY_NAME && x.NAME == model.NAME))
                {
                    return 0;
                }
                else
                {
                    context.EVENT.Add(ev);
                    context.SaveChanges();
                    return 1;
                }
            }
        }


        //This function will delete the event from the table 'events'.Society and event name mentioned in parameter.
        //Test case number: 28
        public bool delete_event(string scname, string ename)
        {
            using (var context = new SocietyClubPortalEntities())
            {

                ScheduleModel schedule = (ScheduleModel)context.SCHEDULE.Where(x => x.SOCIETY_NAME == scname && x.EVENT_NAME == ename).Select(x => new ScheduleModel()
                {
                    VENUE_ID = x.VENUE_ID,
                    DATED = x.DATED,
                    START_TIME = x.START_TIME,
                    END_TIME = x.END_TIME
                }).FirstOrDefault();


                //First we have to unscheduled the event from the Availablity_Of_Venue_table table if it is scheduled.
                if (schedule != null)
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

                    //Then delete it froms cheduled table
                    var scheduled = context.SCHEDULE.FirstOrDefault(x => x.SOCIETY_NAME == scname && x.EVENT_NAME == ename);
                    context.SCHEDULE.Remove(scheduled);
                }

                //Atlast then deleting it from events table
                var en = context.EVENT.FirstOrDefault(x => x.NAME == ename && x.SOCIETY_NAME == scname);
                context.EVENT.Remove(en);
                context.SaveChanges();
                return true;
            }
        }


        //This function will return a list of unscheduled events of society mentioned in parameter.
        //Test case number: 29
        public List<EventModel> get_UnSc_Events(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<EventModel> evns = (from evn in context.EVENT
                                          where !(from sch in context.SCHEDULE select sch.EVENT_NAME).Contains(evn.NAME) && evn.SOCIETY_NAME == scname
                                          select new EventModel()
                                          {
                                              NAME = evn.NAME,
                                              DESCRIPTION = evn.DESCRIPTION
                                          }).ToList();
                return evns;
            }
        }
    }
}