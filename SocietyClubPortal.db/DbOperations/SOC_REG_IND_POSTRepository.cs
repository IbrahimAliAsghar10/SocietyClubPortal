using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.db.DbOperations
{

    //Joint repository of Society,Registration,Induction and post table.
    public class SOC_REG_IND_POSTRepository
    {

        //This function will return combine data of multiple table using List<SOC_REG_IND_POSTModel.
        //Test case number: 36
        public List<SOC_REG_IND_POSTModel> combine_data(string stid)
        {
            List<SOC_REG_IND_POSTModel> result_list = new List<SOC_REG_IND_POSTModel>();
            using (var context = new SocietyClubPortalEntities())
            {
                List<SocietyModel> societies = context.SOCIETY.Select(x => new SocietyModel()
                {
                    NAME = x.NAME,
                    DESCRIPTION = x.DESCRIPTION,
                    INDUCTION_STATUS = x.INDUCTION_STATUS
                }).ToList();
                foreach (var society in societies)
                {
                    SOC_REG_IND_POSTModel result = new SOC_REG_IND_POSTModel()
                    {
                        //Taking society name,Description and Induction Status
                        SOCIETY_NAME = society.NAME,
                        SOCIETY_DESCRIPTION = society.DESCRIPTION,
                        induction_status = society.INDUCTION_STATUS
                    };
                    //Then taking registration status,Post Status by checking if the particular student and society name is in registration table.
                    result.registrationstatus = context.SOCIETY_REGISTRATION.Any(x => x.STUDENT_ID == stid && x.SOCIETY_NAME == society.NAME);
                    result.post_status = context.POST.Any(x => x.STUDENT_ID == stid && x.SOCIETY_NAME == society.NAME);
                    result_list.Add(result);
                }
                return result_list;
            }
        }
    }
}
