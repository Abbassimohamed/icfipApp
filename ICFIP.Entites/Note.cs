//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ICFIP.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int IdNote { get; set; }
        public double Note1 { get; set; }
        public int IdCour { get; set; }
        public int Idetudiant { get; set; }
        public Nullable<double> Note2 { get; set; }
        public Nullable<double> Note3 { get; set; }
        public Nullable<int> IdBts { get; set; }
        public Nullable<int> IdBtp { get; set; }
        public Nullable<int> IdCap { get; set; }
        public Nullable<int> IdNiveau { get; set; }
        public Nullable<System.DateTime> DateNote { get; set; }
        public Nullable<System.TimeSpan> HeurNote { get; set; }
        public Nullable<int> IdSemestre { get; set; }
        public Nullable<int> IdFormation { get; set; }
        public Nullable<int> IdSpetialite { get; set; }
    }
}
