using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.db.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Availablity of Venue Table in FSCP Database
    public class Availability_Of_VenueRepository
    {

        //This function will return all the dates in the FSCP table
        //Test case number: 30
        public List<Availability_Of_VenueModel> bring()
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<Availability_Of_VenueModel> result = context.AVAILABILITY_OF_VENUE.Where(x => x.VENUE_ID == 3 && x.TIME_SLOT == 1).Select(x => new Availability_Of_VenueModel()
                {
                    DATED = x.DATED
                }).ToList();
                return result;
            }
        }


        //This function will update the Availability_Of _Venue Table as the new event is scheduled
        //Test case number: 31
        public bool update_booking(ScheduleModel sch)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                TimeSpan std_time = sch.START_TIME;
                TimeSpan etd_time = sch.END_TIME;
                DateTime date = sch.DATED;
                int id = sch.VENUE_ID;
                //Run this loop until all the slots are book from start_time to end_time.
                while (std_time < etd_time)
                {
                    var vnue = context.AVAILABILITY_OF_VENUE.FirstOrDefault(x => x.VENUE_ID == id && x.START_TIME == std_time && x.DATED == date);
                    if (vnue != null)
                    {
                        vnue.IS_BOOK = true;
                        context.SaveChanges();
                    }
                    std_time += TimeSpan.FromHours(1);
                }
                return true;
            }
        }
    }
}


