using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eficProject.Models
{
    public class Enseignant
    {
        public int IdEnseignant { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Tel { get; set; }
        public string Email { get; set; }
        public string Speialité { get; set; }
        public string Matier { get; set; }
    }
}