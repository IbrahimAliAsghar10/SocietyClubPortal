using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public  class StudentModel
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public StudentModel()
        //{
        //    this.POST = new HashSet<POST>();
        //    this.SOCIETY_REGISTRATION = new HashSet<SOCIETY_REGISTRATION>();
        //}

        public string ID { get; set; }
        public string FNAME { get; set; }
        public string DEPART { get; set; }
        public string STUDENT_BATCH{ get; set; }
        public string CURRENT_SEM { get; set; }
        public string GENDER { get; set; }
        public string EMAIL { get; set; }
        public string CONTACT_NUMBER { get; set; }
        public string SECTION { get; set; }
        public string CGPA { get; set; }
        //public Nullable<System.DateTime> created_at { get; set; }
        //public Nullable<System.DateTime> updated_at { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<POST> POST { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<SOCIETY_REGISTRATION> SOCIETY_REGISTRATION { get; set; }
    }
}
