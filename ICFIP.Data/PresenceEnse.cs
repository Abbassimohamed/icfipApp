//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICFIP.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PresenceEnse
    {
        public int IdPresence { get; set; }
        public int absence { get; set; }
        public int Idenseignant { get; set; }
        public Nullable<int> nbdabsence { get; set; }
        public Nullable<System.DateTime> DateAbsnece { get; set; }
        public Nullable<System.TimeSpan> HeurAbsence { get; set; }
        public Nullable<int> IdCour { get; set; }
        public Nullable<int> Idmois { get; set; }
    }
}
