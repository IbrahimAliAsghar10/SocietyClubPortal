using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_APIS.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Venue Table in FSCP Database
    public class VenueRepository
    {
        //This function will return a list of all available venues using Dummy model
        //Test case number: 32
        public List<VenueModel> find_Venues(Dummy model)
        {


            List<VenueModel> result = new List<VenueModel>();
            using (var context = new SocietyClubPortalEntities())
            {
                var std_time = TimeSpan.Parse(model.A);
                var etd_time = TimeSpan.Parse(model.B);
                var date = DateTime.Parse(model.DATE);
                var std = context.VENUE.Select(x => new VenueModel()
                {
                    ID = x.ID
                }
                ).ToList();

                //Making a list of all venues
                var v_ids = std.Select(VenueModel => VenueModel.ID).ToList();

                //Then checking each venue if it is available for the specified time slot or not.
                foreach (var vid in v_ids)
                {
                    std_time = TimeSpan.Parse(model.A);
                    int count = 0;
                    while (std_time < etd_time && context.AVAILABILITY_OF_VENUE.Any(x => x.VENUE_ID == vid && x.START_TIME == std_time && x.IS_BOOK == false && x.DATED == date))
                    {
                        count = 1;
                        //std_time = std_time.Add();
                        std_time += TimeSpan.FromHours(1);
                    }
                    //If venue is available for every required hours, then the venue is appended to the list 'ResultValue' containing all available venues.
                    if (etd_time == std_time && count == 1)
                    {
                        VenueModel resultvalue = (VenueModel)context.VENUE.Where(x => x.ID == vid).Select(x => new VenueModel()
                        {
                            ID = x.ID,
                            NAME = x.NAME
                        }
                        ).SingleOrDefault();
                        result.Add(resultvalue);
                    }
                }
                return result;
            }
        }
    }
}
