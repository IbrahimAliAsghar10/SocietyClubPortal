//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SocietyClubPortal.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class AVAILABILITY_OF_VENUE
    {
        public System.DateTime DATED { get; set; }
        public System.TimeSpan START_TIME { get; set; }
        public System.TimeSpan END_TIME { get; set; }
        public int TIME_SLOT { get; set; }
        public bool IS_BOOK { get; set; }
        public int VENUE_ID { get; set; }
    
        public virtual VENUE VENUE { get; set; }
    }
}
