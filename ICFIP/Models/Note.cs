using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eficProject.Models
{
    public class Note
    {
        public int note { get; set; }
        public int IdEnseignant { get; set; }
        public int IdEtudiant { get; set; }
        public int IdCour { get; set; }
    }
}