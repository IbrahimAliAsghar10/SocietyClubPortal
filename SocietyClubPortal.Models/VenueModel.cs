using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocietyClubPortal.Models
{
    public class VenueModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public VENUE()
        //{
        //    this.AVAILABILITY_OF_VENUE = new HashSet<AVAILABILITY_OF_VENUE>();
        //    this.SCHEDULE = new HashSet<SCHEDULE>();
        //}

        public int ID { get; set; }
        public string NAME { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<AVAILABILITY_OF_VENUE> AVAILABILITY_OF_VENUE { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<SCHEDULE> SCHEDULE { get; set; }
    }
}
