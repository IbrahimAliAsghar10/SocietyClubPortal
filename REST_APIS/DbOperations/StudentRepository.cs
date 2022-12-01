using SocietyClubPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_APIS.DbOperations
{

    //This class is used to perform Read,Update,Delete operations of Student Table in FSCP Database
    public class StudentRepository
    {
        //This function will add new student information to the 'students' table
        //Test case number:1
        public int AddStudent(String Email)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                
                //Student ID and password information will also be separately stored in the VERIFY_ROLE table so they can login.
                
                if (context.student.Any(x => x.email == Email))
                {
                    if (context.User.Any(y=>y.Email == Email))
                    {
                        return 2;
                    }
                    else
                    {
                        User vrf = new User()
                        {

                            Email = Email,
                            Role = 2,
                            Password = "123",
                            UserName = getUserName(Email)
                        };
                        context.User.Add(vrf);
                        context.SaveChanges();
                        return 1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }

        //This function will return UserName of student in an object. 
        //Test case number: --
        public string getUserName(string email)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var result = (from User in context.student where User.email == email select User.id).FirstOrDefault();
                return result;
            }
        }

        //This function will return all details of student in an object.
        //Test case number: 6
        public StudentModel getstudent(string Student_ID)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                StudentModel stu = context.student.Where(x => x.id == Student_ID).Select(x => new StudentModel()
                {
                    ID = x.id,
                    FNAME = x.fname,
                    DEPART = x.depart,
                    STUDENT_BATCH = x.student_batch,
                    CURRENT_SEM = x.current_sem,
                    CGPA = x.cgpa,
                    GENDER = x.gender,
                    EMAIL = x.email,
                    CONTACT_NUMBER = x.contact_number,
                    SECTION = x.section
                }).FirstOrDefault();
                return stu;
            }
        }

        //This function will return all students with details in a list of objects.
        //Test case number: 7
        public List<StudentModel> Get_All_Students()
        {
            using (var context = new SocietyClubPortalEntities())
            {
                List<StudentModel> stu = context.student.Select(x => new StudentModel()
                {
                    ID = x.id,
                    FNAME = x.fname,
                    DEPART = x.depart,
                    STUDENT_BATCH = x.student_batch,
                    CURRENT_SEM = x.current_sem,
                    CGPA = x.cgpa,
                    GENDER = x.gender,
                    EMAIL = x.email,
                    CONTACT_NUMBER = x.contact_number,
                    SECTION = x.section
                }).ToList();

                stu = stu.OrderBy(x => int.Parse(x.STUDENT_BATCH)).ToList();
                return stu;
            }
        }

        //This function will delete the student data from the table. Student ID is mentioned in the parameter.
        //Test case number: 8
        //public bool delete_student(string stid)
        //{
        //    using (var context = new SocietyClubPortalEntities())
        //    {
        //        var std_reg = context.SOCIETY_REGISTRATION.FirstOrDefault(x => x.STUDENT_ID == stid);
        //        if (std_reg != null)
        //        {
        //            context.SOCIETY_REGISTRATION.Remove(std_reg);
        //        }
        //        var std_posts = context.POST.Where(x => x.ST_ID == stid).ToList();
        //        foreach (var std_post in std_posts)
        //        {
        //            if (std_post != null)
        //            {
        //                std_post.ST_ID = null;
        //            }
        //        }
        //        var std = context.STUDENT.FirstOrDefault(x => x.ST_ID == stid);
        //        context.STUDENT.Remove(std);
        //        var verify_role = context.VERIFY_ROLE.FirstOrDefault(x => x.USERNAME == stid);
        //        context.VERIFY_ROLE.Remove(verify_role);
        //        context.SaveChanges();
        //        return true;
        //    }
        //}

        //This function will update the existing students' details. Student name is mentioned in the parameter.
        //Test case number: 9
        public bool updatestudent(StudentModel sco)
        {
            using (var context = new SocietyClubPortalEntities())
            {
                var std = context.student.FirstOrDefault(x => x.id == sco.ID);
                if (std != null)
                {
                    std.id = sco.ID;
                    std.fname = sco.FNAME;
                    std.depart = sco.DEPART;
                    std.cgpa = sco.CGPA;
                    std.student_batch = sco.STUDENT_BATCH;
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
