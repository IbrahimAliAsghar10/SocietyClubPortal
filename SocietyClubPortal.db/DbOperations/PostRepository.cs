using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.db.DbOperations
{
    //This class is used to perform Read,Update,Delete operations of Posts Table in FSCP Database
    public class PostRepository
    {

        //This function will add the new post of the society in the post table using given info in the parameter.
        //Test case number: 14
        public bool AddPost(PostModel model)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                POST pst = new POST()
                {
                    SOCIETY_NAME = model.SOCIETY_NAME,
                    NAME = model.NAME
                };
                if (context.POST.Any(x => x.SOCIETY_NAME == model.SOCIETY_NAME && x.NAME == model.NAME))
                {
                    return false;
                }
                else
                {
                    context.POST.Add(pst);
                    context.SaveChanges();
                    return true;
                }
            }
        }


        //This function will add the students info to the post of the society in the post table using info in the parameters of the function.
        //Test case number: 15
        public bool student_on_post_added(string st, string scname, string pname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var psts = context.POST.FirstOrDefault(x => x.SOCIETY_NAME == scname && x.NAME == pname);
                if (psts != null)
                {
                    psts.STUDENT_ID = st;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //This function will update the mentioned post of given society name in the post table by removing st_prev students info from the table to the st_new students info to the same post.
        //Test case number: 16
        public bool student_on_post_updated(string st_prev, string st_new, string scname, string pname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var psts = context.POST.FirstOrDefault(x => x.SOCIETY_NAME == scname && x.STUDENT_ID == st_prev && x.NAME == pname);
                if (psts != null)
                {
                    psts.STUDENT_ID = st_new;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //This function will delete the post of the society from the post table using given info in the parameter.
        //Test case number: 17
        public bool DeletePost(string pname, string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var psts = context.POST.FirstOrDefault(x => x.NAME == pname && x.SOCIETY_NAME == scname);
                if (psts != null)
                {
                    context.POST.Remove(psts);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //This function will return all the posts of the society name mentioned in the parameter that are in the table and info of the person if it is already taken by the student.
        //Test case number: 18
        public List<PostModel> show_hierarchy(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<PostModel> psts = context.POST.Where(x => x.SOCIETY_NAME == scname).Select(x => new PostModel()
                {
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    NAME = x.NAME,
                    STUDENT_ID = x.STUDENT_ID,
                    student = new StudentModel()
                    {
                        FNAME= x.student.fname
                    }
                }
                ).ToList();
                return psts;
            }
        }


        //This function will return all the posts that are unfilled in the 'posts' table of the society name mentioned in the parameter.
        //Test case number: 19
        public List<PostModel> unfilled_posts(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<PostModel> psts = context.POST.Where(x => x.SOCIETY_NAME == scname && x.STUDENT_ID == null).Select(x => new PostModel()
                {
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    NAME = x.NAME,
                    STUDENT_ID = x.STUDENT_ID,
                    student = new StudentModel()
                    {
                        FNAME = x.student.fname,
                        STUDENT_BATCH = x.student.student_batch
                    }
                }
                ).ToList();
                return psts;
            }
        }


        //This function will return all the posts that are filled in the 'posts' table of the society name mentioned in the parameter.
        //Test case number: 20
        public List<PostModel> filled_posts(string scname)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<PostModel> psts = context.POST.Where(x => x.SOCIETY_NAME == scname && x.STUDENT_ID != null).Select(x => new PostModel()
                {
                    SOCIETY_NAME = x.SOCIETY_NAME,
                    NAME = x.NAME,
                    STUDENT_ID = x.STUDENT_ID,
                    student = new StudentModel()
                    {
                        FNAME = x.student.fname,
                        STUDENT_BATCH = x.student.student_batch
                    }
                }
                ).ToList();
                return psts;
            }
        }
    }

}
