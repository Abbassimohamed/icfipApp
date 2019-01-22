using ICFIP.Common.Config;
using ICFIP.Entites;
using ICFIP.Services.Implement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using System.Net.Mail;
using System.Text;
using System.Net;
using ICFIP.Filters;



namespace ICFIP.Controllers
{
    [ExceptionFilter]
    [LoggerFilter]
    public class administrateurController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(HomeController));
        //
        // GET: /administrateur/
        Repository<Niveau> repNiveau = new Repository<Niveau>(AppConfig.DbConnexionString);
       // List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();



        Repository<Formation> repFor = new Repository<Formation>(AppConfig.DbConnexionString);
        //List<Formation> formation = repFor.GetAll(null, false).ToList();




        Repository<TypeSpecialite> repSpec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
        //List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();
        Repository<Cour> repcour = new Repository<Cour>(AppConfig.DbConnexionString);
        public ActionResult Index()
        {

            return View();

        }

        #region GestionEtudiant
        public ActionResult GetListEtudiant()
        {
            Repository<Niveau> repeNiv = new Repository<Niveau>(AppConfig.DbConnexionString);

            Repository<Formation> repeformat = new Repository<Formation>(AppConfig.DbConnexionString);

            Repository<TypeSpecialite> repespec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
          

          
       

            Repository<Etudiant> repetudiant = new Repository<Etudiant>(AppConfig.DbConnexionString);
            List<Etudiant> letudiant = new List<Etudiant>();
            letudiant = repetudiant.GetAll(null, false).ToList();
            List<int> nive = new List<int>();
            List<string> forma = new List<string>();
            List<string> section = new List<string>();
            foreach (Etudiant e in letudiant)
            {
                {
                    int niv = repeNiv.GetSingle(x => x.IdNiveau == e.IdNiveau, false).IdNiveau;

                    if (niv != null)
                    {
                        nive.Add(niv);
                    }
                }
                {
                    string s = repespec.GetSingle(x => x.IdSpetialite == e.IdSpetialite, false).LibelleSpecialite;

                    if (s != null)
                    {
                        section.Add(s);
                    }
                }
                {
                    string f = repeformat.GetSingle(x => x.IdFormation == e.IdFormation, false).LibelleFormation;

                    if (f != null)
                    {
                        forma.Add(f);
                    }
                }

            }


            ViewBag.lista = nive;
            ViewBag.listb = section;
            ViewBag.listc = forma;





            return View(letudiant);
        }
        public ActionResult CreatEtudiant()
        {
            try
            {
                Repository<Niveau> repNiveau = new Repository<Niveau>(AppConfig.DbConnexionString);
                List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();

                ViewBag.IdNiveau = new SelectList(niveau, "IdNiveau", "Libelle");

                Repository<Formation> repFor = new Repository<Formation>(AppConfig.DbConnexionString);
                List<Formation> formation = repFor.GetAll(null, false).ToList();

                ViewBag.IdFormation = new SelectList(formation, "IdFormation", "LibelleFormation");


                Repository<TypeSpecialite> repSpec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
                List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();

                ViewBag.IdSpetialite = new SelectList(specilaite, "IdSpetialite", "LibelleSpecialite");

                //log.Info(" creation d'un etudiant avec sucess: " );

            }
            catch(Exception ex)
            {
                //log.Info("erreur create etudiant: " + ex.Message);
            }
                

                return View(new Etudiant());
            
        }
        [HttpPost]
         public ActionResult CreatEtudiant(Etudiant _etudiant)
        {
           try
           {
               List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();
               List<Formation> formation = repFor.GetAll(null, false).ToList();
               List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();


               Repository<UserProfile> repuser = new Repository<UserProfile>(AppConfig.DbConnexionString);



               Repository<Etudiant> repetudiant = new Repository<Etudiant>(AppConfig.DbConnexionString);
               //   if (ModelState.IsValid)
               {


                   WebSecurity.CreateUserAndAccount(_etudiant.NCin + "-" + _etudiant.Prenom, _etudiant.MotdePasse);



                   int userid = repuser.GetSingle(v => v.UserName == _etudiant.NCin + "-" + _etudiant.Prenom, false).UserId;
                   string username = repuser.GetSingle(v => v.UserName == _etudiant.NCin + "-" + _etudiant.Prenom, false).UserName;
                   Roles.AddUserToRole(username, "Etudiant");
                   string pwd = _etudiant.MotdePasse;
                   _etudiant.UserId = userid;

                   repetudiant.Add(_etudiant);

                   string smtpAddress = "smtp.live.com";
                   int portNumber = 587;
                   bool enableSSL = true;

                   string emailFrom = "anasabassi11@outlook.fr";
                   string password = "bigboss1992";
                   string emailTo = _etudiant.Email;
                   string subject = "Hello";
                   string body = "Votre mot de passe et :" + _etudiant.MotdePasse + "Votre login est :" + _etudiant.NCin + "-" + _etudiant.Prenom;

                   using (MailMessage mail = new MailMessage())
                   {
                       mail.From = new MailAddress(emailFrom);
                       mail.To.Add(emailTo);
                       mail.Subject = subject;
                       mail.Body = body;
                       mail.IsBodyHtml = true;
                       

                       using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                       {
                           smtp.Credentials = new NetworkCredential(emailFrom, password);
                           
                           
                           smtp.EnableSsl = enableSSL;
                           smtp.Send(mail);
                           return RedirectToAction("Index");
                       }
                   }





               }

           }
             
            catch(Exception ex)
            {
                log.Info("erreur create etudiant: " + ex.Message);
                    return RedirectToAction("CreatEtudiant");
            }

             /* ViewBag.IdNiveau = new SelectList(niveau, "IdNiveau", "Libelle", _etudiant.Niveau);
              ViewBag.IdFormation = new SelectList(formation, "IdFormation", "LibelleFormation", _etudiant.IdNiveauFormation);
              ViewBag.IdSpecialite = new SelectList(specilaite, "IdSpecialite", "LibelleSpecialite", _etudiant.IdSpetialite);
            return View(_etudiant);*/
        }



        public ActionResult Pssagemodetudi(int? id)
        {
            TempData["idetu"] = id;


            return View();


        }

        public ActionResult editEtudiant()
        {
            int? id = Convert.ToInt32(TempData["idetu"]);
            
            Repository<Niveau> repNiveau = new Repository<Niveau>(AppConfig.DbConnexionString);
            List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();

            ViewBag.IdNiveau = new SelectList(niveau, "IdNiveau", "Libelle");

            Repository<Formation> repFor = new Repository<Formation>(AppConfig.DbConnexionString);
            List<Formation> formation = repFor.GetAll(null, false).ToList();

            ViewBag.IdFormation = new SelectList(formation, "IdFormation", "LibelleFormation");


            Repository<TypeSpecialite> repSpec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();

            ViewBag.IdSpetialite = new SelectList(specilaite, "IdSpetialite", "LibelleSpecialite");
            Repository<Etudiant> _repradbtn = new Repository<Etudiant>(AppConfig.DbConnexionString);
            Etudiant _etudiant = _repradbtn.GetSingle(v => v.Idetudiant == id, false);
            return View(_etudiant);
        }
        [HttpPost]
        public ActionResult editEtudiant(Etudiant _etudiant)
        {
            List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();
            List<Formation> formation = repFor.GetAll(null, false).ToList();
            List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();

            Repository<Etudiant> _repradbtn = new Repository<Etudiant>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(_etudiant);

                return RedirectToAction("Index");
            }


            return View(_etudiant);
        }

        public ActionResult DetailEtudiant(int id)
        {
            int idcou = id;

            Repository<Etudiant> repadmin3 = new Repository<Etudiant>(AppConfig.DbConnexionString);
            Etudiant lis1 = repadmin3.GetSingle(v => v.Idetudiant == idcou, false);
            Repository<TypeSpecialite> repetudiant = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            TypeSpecialite spec = repetudiant.GetSingle(v => v.IdSpetialite == lis1.IdSpetialite, false);
            ViewBag.libelspec = spec.DescriptionSpetialite;
            Repository<Formation> repfor = new Repository<Formation>(AppConfig.DbConnexionString);
            Formation cour = repfor.GetSingle(v => v.IdFormation == lis1.IdFormation, false);
            ViewBag.libelfor = cour.DescriptionFormation;


            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }

        public ActionResult Pssagdeletetudi(int? id)
        {
            TempData["idetudelet"] = id;


            return View();


        }
        public ActionResult DeleteEtudiant(bool? saveChangesError)
        {
             int? id = Convert.ToInt32(TempData["idetudelet"]);
            Repository<Etudiant> _reptoutabs1 = new Repository<Etudiant>(AppConfig.DbConnexionString);

           


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Etudiant cour = _reptoutabs1.GetSingle(v => v.Idetudiant == id, false);
            TempData["idetudelet1"] = id;
            return View(cour);

        }

        [HttpPost, ActionName("DeleteEtudiant")]
        public ActionResult DeleteConfirmed8()
        {
            int? id = Convert.ToInt32(TempData["idetudelet1"]);
            Repository<Etudiant> _reptoutabs1 = new Repository<Etudiant>(AppConfig.DbConnexionString);


            Etudiant cour = _reptoutabs1.GetSingle(v => v.Idetudiant == id, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }


        public ActionResult ModifierNote()
        {

             int? id = Convert.ToInt32(TempData["idetud"]);
           

            int idSemestre=Convert.ToInt32(Session["Semestre"].ToString());
            Repository<Cour> _repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            Notess Lnotes = new Notess();
            Repository<Note> _repradbtn = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _etudiant = _repradbtn.GetAll(v => v.Idetudiant == id && v.IdSemestre==1, false).ToList();
            Lnotes.ListeNote = _etudiant;
            List<string> cours = new List<string>();

            foreach (Note e in _etudiant)
            {
                {
                    string cour = _repcour.GetSingle(x => x.IdCour == e.IdCour, false).Libelle;

                 
                 
                        cours.Add(cour);
              
                }
                ViewBag.IDCour = cours;
            }
         
           
            return View(Lnotes);
        }
        [HttpPost]
        public ActionResult ModifierNote(Notess Lnotes)
        {



            Repository<Note> _repNote = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _notes = _repNote.GetAll(null, false).ToList();

            foreach (Note p in Lnotes.ListeNote)
            {
                _repNote.Update(p);
            }





            return RedirectToAction("Index");
        }


        public ActionResult lienn()
        {
            TempData["idetud"] = TempData["idetudmdfn"];


            return View();


        }

        public ActionResult PssagemodfNote(int? id)
        {
            TempData["idetudmdfn"] = id;


            return View();


        }






        public ActionResult ModifierNote1()
        {
            int? id = Convert.ToInt32(TempData["idetud"]);


            int idSemestre = Convert.ToInt32(Session["Semestre"].ToString());
            Repository<Cour> _repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            Notess Lnotes = new Notess();
            Repository<Note> _repradbtn = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _etudiant = _repradbtn.GetAll(v => v.Idetudiant == id && v.IdSemestre ==2, false).ToList();
            Lnotes.ListeNote = _etudiant;
            List<string> cours = new List<string>();

            foreach (Note e in _etudiant)
            {
                {
                    string cour = _repcour.GetSingle(x => x.IdCour == e.IdCour, false).Libelle;



                    cours.Add(cour);

                }
                ViewBag.IDCour = cours;
            }


            return View(Lnotes);
        }
        [HttpPost]
        public ActionResult ModifierNote1(Notess Lnotes)
        {



            Repository<Note> _repNote = new Repository<Note>(AppConfig.DbConnexionString);
            List<Note> _notes = _repNote.GetAll(null, false).ToList();

            foreach (Note p in Lnotes.ListeNote)
            {
                _repNote.Update(p);
            }





            return RedirectToAction("Index");
        }



        public ActionResult liennabs()
        {
            TempData["idetud"] = TempData["idetudabs"];


            return View();


        }

        public ActionResult Pssagemodfabs(int? id)
        {
            TempData["idetudabs"] = id;


            return View();


        }


        public ActionResult ModifierAbsence()
        {
            int? id = Convert.ToInt32(TempData["idetud"]);


           
            Repository<Cour> _repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            Absenccess Lnotes = new Absenccess();
            Repository<Presence> _repradbtn = new Repository<Presence>(AppConfig.DbConnexionString);
            List<Presence> _etudiant = _repradbtn.GetAll(v => v.Idetudiant == id && v.IdSemestre == 1, false).ToList();
            Lnotes.ListePresence = _etudiant;
            List<string> cours = new List<string>();

            foreach (Presence e in _etudiant)
            {
                {
                    string cour = _repcour.GetSingle(x => x.IdCour == e.IdCour, false).Libelle;



                    cours.Add(cour);

                }
                ViewBag.IDCour = cours;
            }


            return View(Lnotes);
        }
        [HttpPost]
        public ActionResult ModifierAbsence(Absenccess Lnotes)
        {



            Repository<Presence> _repNote = new Repository<Presence>(AppConfig.DbConnexionString);
            List<Presence> _notes = _repNote.GetAll(null, false).ToList();

            foreach (Presence p in Lnotes.ListePresence)
            {
                p.Absence = p.IsPresnece;
                _repNote.Update(p);
            }





            return RedirectToAction("Index");
        }




        public ActionResult ModifierAbsence1()
        {
            int? id = Convert.ToInt32(TempData["idetud"]);



            Repository<Cour> _repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            Absenccess Lnotes = new Absenccess();
            Repository<Presence> _repradbtn = new Repository<Presence>(AppConfig.DbConnexionString);
            List<Presence> _etudiant = _repradbtn.GetAll(v => v.Idetudiant == id && v.IdSemestre == 2, false).ToList();
            Lnotes.ListePresence = _etudiant;
            List<string> cours = new List<string>();

            foreach (Presence e in _etudiant)
            {
                {
                    string cour = _repcour.GetSingle(x => x.IdCour == e.IdCour, false).Libelle;



                    cours.Add(cour);

                }
                ViewBag.IDCour = cours;
            }


            return View(Lnotes);
        }
        [HttpPost]
        public ActionResult ModifierAbsence1(Absenccess Lnotes)
        {



            Repository<Presence> _repNote = new Repository<Presence>(AppConfig.DbConnexionString);
            List<Presence> _notes = _repNote.GetAll(null, false).ToList();

            foreach (Presence p in Lnotes.ListePresence)
            {
                p.Absence = p.IsPresnece;
                _repNote.Update(p);
            }





            return RedirectToAction("Index");
        }






      


        #endregion




        #region GestionEnseignant

     

        public ActionResult GetListEnseignant()
        {


            Repository<Enseignant> repetudiant = new Repository<Enseignant>(AppConfig.DbConnexionString);
            List<Enseignant> letudiant = new List<Enseignant>();


    

            letudiant = repetudiant.GetAll(null, false).ToList();
            return View(letudiant);
        }
        public ActionResult CreatEnseignant()
        {
            Repository<Niveau> repNiveau = new Repository<Niveau>(AppConfig.DbConnexionString);
            List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();

            ViewBag.IdNiveau = new SelectList(niveau, "IdNiveau", "Libelle");

            Repository<Formation> repFor = new Repository<Formation>(AppConfig.DbConnexionString);
            List<Formation> formation = repFor.GetAll(null, false).ToList();

            ViewBag.IdFormation = new SelectList(formation, "IdFormation", "LibelleFormation");


            Repository<TypeSpecialite> repSpec = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();

            ViewBag.IdSpetialite = new SelectList(specilaite, "IdSpetialite", "LibelleSpecialite");


            Repository<Cour> repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            List<Cour> cour = repcour.GetAll(null, false).ToList();

            ViewBag.IdCour = new SelectList(cour, "IdCour", "Libelle");






            return View(new Enseignant());
        }
        [HttpPost]
        public ActionResult CreatEnseignant(Enseignant _Enseignant)
        {

            List<Niveau> niveau = repNiveau.GetAll(null, false).ToList();
            List<Formation> formation = repFor.GetAll(null, false).ToList();
            List<TypeSpecialite> specilaite = repSpec.GetAll(null, false).ToList();
            List<Cour> cour = repcour.GetAll(null, false).ToList();


            Repository<UserProfile> repuser = new Repository<UserProfile>(AppConfig.DbConnexionString);
            Repository<Enseignant> repetudiant = new Repository<Enseignant>(AppConfig.DbConnexionString);
            //   if (ModelState.IsValid)
            {


                WebSecurity.CreateUserAndAccount(_Enseignant.NCin + "-" + _Enseignant.Prenom, _Enseignant.MotdePasse);

                int userid = repuser.GetSingle(v => v.UserName == _Enseignant.NCin + "-" + _Enseignant.Prenom, false).UserId;
                string username = repuser.GetSingle(v => v.UserName == _Enseignant.NCin + "-" + _Enseignant.Prenom, false).UserName;
                Roles.AddUserToRole(username, "Enseignant");
                string pwd = _Enseignant.MotdePasse;
                _Enseignant.UserId = userid;
                repetudiant.Add(_Enseignant);





                string smtpAddress = "smtp.live.com";
                int portNumber = 587;
                bool enableSSL = true;

                string emailFrom = "anasabassi11@outlook.fr";
                string password = "bigboss1992";
                string emailTo = _Enseignant.Email;
                string subject = "Hello";
                string body = "Votre mot de passe et :" + _Enseignant.MotdePasse + "Votre login est :" + _Enseignant.NCin + "-" + _Enseignant.Prenom;

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(emailFrom);
                    mail.To.Add(emailTo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;


                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(emailFrom, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                        return RedirectToAction("Index");
                    }
                }

            }

            /* ViewBag.IdNiveau = new SelectList(niveau, "IdNiveau", "Libelle", _etudiant.Niveau);
             ViewBag.IdFormation = new SelectList(formation, "IdFormation", "LibelleFormation", _etudiant.IdNiveauFormation);
             ViewBag.IdSpecialite = new SelectList(specilaite, "IdSpecialite", "LibelleSpecialite", _etudiant.IdSpetialite);
           return View(_etudiant);*/
        }





        public ActionResult editEnseignant(int id)
        {

            Repository<Cour> repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            List<Cour> cour = repcour.GetAll(null, false).ToList();

            ViewBag.IdCour = new SelectList(cour, "IdCour", "Libelle");

            Repository<Enseignant> _repradbtn = new Repository<Enseignant>(AppConfig.DbConnexionString);
            Enseignant _etudiant = _repradbtn.GetSingle(v => v.Idenseignant == id, false);
            return View(_etudiant);
        }
        [HttpPost]
        public ActionResult editEnseignant(Enseignant _Enseignant)
        {
            List<Cour> specilaite = repcour.GetAll(null, false).ToList();

            Repository<Enseignant> _repradbtn = new Repository<Enseignant>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(_Enseignant);

                return RedirectToAction("Index");
            }


            return View(_Enseignant);
        }

        public ActionResult DetailEnseignant(int id)
        {
            int idcou = id;

            Repository<Enseignant> repadmin3 = new Repository<Enseignant>(AppConfig.DbConnexionString);
            Enseignant lis1 = repadmin3.GetSingle(v => v.Idenseignant == idcou, false);
            Repository<Cour> repetudiant = new Repository<Cour>(AppConfig.DbConnexionString);
            Cour cour = repetudiant.GetSingle(v => v.IdCour == lis1.IdCour, false);
            ViewBag.libelcour=cour.Libelle;
            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }



        public ActionResult DeleteEnseignant(int id, bool? saveChangesError)
        {
            Repository<Enseignant> _reptoutabs1 = new Repository<Enseignant>(AppConfig.DbConnexionString);

            int idcou = id;


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Enseignant cour = _reptoutabs1.GetSingle(v => v.Idenseignant == idcou, false);
            return View(cour);
        }




        [HttpPost, ActionName("DeleteEnseignant")]
        public ActionResult DeleteConfirmed9(int id)
        {
            int idcou = id;
            Repository<Enseignant> _reptoutabs1 = new Repository<Enseignant>(AppConfig.DbConnexionString);


            Enseignant cour = _reptoutabs1.GetSingle(v => v.Idenseignant == idcou, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }









        public ActionResult Pssageditabsenseig(int? id)
        {
            TempData["absensieg"] = id;


            return View();


        }

        public ActionResult AjouterAbsEnseigant()
        {

            int? id = Convert.ToInt32(TempData["absensieg"]);
            Repository<Enseignant> _repensei = new Repository<Enseignant>(AppConfig.DbConnexionString);
            Enseignant _ensei = _repensei.GetSingle(x=>x.Idenseignant==id, false);





            return View("AjouterAbsEnseigant", _ensei);



        }

        [HttpPost]
        public ActionResult AjouterAbsEnseigant(Enseignant ensei)
        {


            Repository<PresenceEnse> _reppresence = new Repository<PresenceEnse>(AppConfig.DbConnexionString);
            PresenceEnse _Absnece = new PresenceEnse();

            DateTime dt = DateTime.Now;//.AddDays(1);
            _Absnece.Idenseignant = ensei.Idenseignant;
            _Absnece.IdCour = ensei.IdCour;
            _Absnece.absence = ensei.IsPresnece;
            _Absnece.DateAbsnece = dt;
            _Absnece.HeurAbsence = new TimeSpan(dt.Hour, dt.Minute, dt.Second);








            _reppresence.Add(_Absnece);
       

           
            return RedirectToAction("Index");
        }




        public ActionResult ModifierAbsencens(int? id)
        {

            Repository<VtPEnsig> _repcour = new Repository<VtPEnsig>(AppConfig.DbConnexionString);
            List<VtPEnsig> _absenceEnsi = _repcour.GetAll(null, false).ToList();



            return View(_absenceEnsi);
        }
        //[HttpPost]
        //public ActionResult ModifierAbsenceetud(Notess Lnotes)
        //{



        //    Repository<Note> _repNote = new Repository<Note>(AppConfig.DbConnexionString);
        //    List<Note> _notes = _repNote.GetAll(null, false).ToList();

        //    foreach (Note p in Lnotes.ListeNote)
        //    {
        //        _repNote.Update(p);
        //    }





        //    return RedirectToAction("Index");
        //}



        public ActionResult ModifierAbsenceensei(int id)
        {
           



            Repository<Cour> _repcour = new Repository<Cour>(AppConfig.DbConnexionString);
            Absencenseig Lnotes = new Absencenseig();
            Repository<PresenceEnse> _repradbtn = new Repository<PresenceEnse>(AppConfig.DbConnexionString);
            List<PresenceEnse> _etudiant = _repradbtn.GetAll(v => v.Idenseignant == id, false).ToList();
            Lnotes.ListePresenceEnseig = _etudiant;
            List<string> cours = new List<string>();

            //foreach (PresenceEnse e in _etudiant)
            //{
            //    {
            //        string cour = _repcour.GetSingle(x => x.IdCour == e.IdCour, false).Libelle;



            //        cours.Add(cour);

            //    }
            //    ViewBag.IDCour = cours;
            //}


            return View(Lnotes);
        }
        [HttpPost]
        public ActionResult ModifierAbsenceensei(Absencenseig Lnotes)
        {



            Repository<PresenceEnse> _repNote = new Repository<PresenceEnse>(AppConfig.DbConnexionString);
            List<PresenceEnse> _notes = _repNote.GetAll(null, false).ToList();

            foreach (PresenceEnse p in Lnotes.ListePresenceEnseig)
            {
                p.absence = p.IsPresnece;
                _repNote.Update(p);
            }





            return RedirectToAction("Index");
        }


        #endregion
        





        public ActionResult Gerecour()
        {
            Repository<Cour> _reptoutabs1 = new Repository<Cour>(AppConfig.DbConnexionString);
            List<Cour> _toutabs = _reptoutabs1.GetAll(null, false).ToList();


            if (_toutabs == null)
            {
                return HttpNotFound();
            }

            return View(_toutabs);

        }


        public ActionResult GereSpecialite()
        {
            Repository<TypeSpecialite> _reptoutabs1 = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            List<TypeSpecialite> _toutabs = _reptoutabs1.GetAll(null, false).ToList();


            if (_toutabs == null)
            {
                return HttpNotFound();
            }

            return View(_toutabs);

        }

        public ActionResult GereBtp()
        {
            Repository<BTP> _reptoutabs1 = new Repository<BTP>(AppConfig.DbConnexionString);
            List<BTP> _toutabs = _reptoutabs1.GetAll(null, false).ToList();


            if (_toutabs == null)
            {
                return HttpNotFound();
            }

            return View(_toutabs);

        }
        public ActionResult GereFormation()
        {
            Repository<Formation> _reptoutabs1 = new Repository<Formation>(AppConfig.DbConnexionString);
            List<Formation> _toutabs = _reptoutabs1.GetAll(null, false).ToList();


            if (_toutabs == null)
            {
                return HttpNotFound();
            }

            return View(_toutabs);

        }


        public ActionResult GereSalle()
        {
            Repository<Salle> _reptoutabs1 = new Repository<Salle>(AppConfig.DbConnexionString);
            List<Salle> _toutabs = _reptoutabs1.GetAll(null, false).ToList();


            if (_toutabs == null)
            {
                return HttpNotFound();
            }


            return View(_toutabs);

        }


        /// <summary>
        /// salle
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        public ActionResult DetailSalle(int id)
        {
            int idcou = id;

            Repository<Salle> repadmin3 = new Repository<Salle>(AppConfig.DbConnexionString);
            Salle lis1 = repadmin3.GetSingle(v => v.IdSalle == idcou, false);

            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }
        public ActionResult DeleteSalle(int id, bool? saveChangesError)
        {
            Repository<Salle> _reptoutabs1 = new Repository<Salle>(AppConfig.DbConnexionString);

            int idcou = id;


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Salle cour = _reptoutabs1.GetSingle(v => v.IdSalle == idcou, false);
            return View(cour);
        }

        [HttpPost, ActionName("DeleteSalle")]
        public ActionResult DeleteConfirmed6(int id)
        {
            int idcou = id;
            Repository<Salle> _reptoutabs1 = new Repository<Salle>(AppConfig.DbConnexionString);


            Salle cour = _reptoutabs1.GetSingle(v => v.IdSalle == idcou, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }
        public ActionResult ajoutersalle()
        {


            return View(new Salle());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ajoutersalle(Salle tt)
        {
            Repository<Salle> _repradbtn = new Repository<Salle>(AppConfig.DbConnexionString);





            if (ModelState.IsValid)
            {
                _repradbtn.Add(tt);
                return RedirectToAction("Index");



            }
            return View(tt);


        }

        public ActionResult Editsalle(int id)
        {
            Repository<Salle> _repradbtn = new Repository<Salle>(AppConfig.DbConnexionString);
            Salle cour = _repradbtn.GetSingle(v => v.IdSalle == id, false);
            return View(cour);
        }
        [HttpPost]
        public ActionResult Editsalle(Salle cour)
        {
            Repository<Salle> _repradbtn = new Repository<Salle>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(cour);

                return RedirectToAction("Index");
            }


            return View(cour);
        }



        /// <summary>
        /// Formation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult PssageditFormat(int? id)
        {
            TempData["editformation"] = id;


            return View();


        }
        public ActionResult PssagdeletFormat(int? id)
        {
            TempData["deletformation"] = id;


            return View();


        }



        public ActionResult DetailsFormation(int id)
        {
            int idcou = id;

            Repository<Formation> repadmin3 = new Repository<Formation>(AppConfig.DbConnexionString);
            Formation lis1 = repadmin3.GetSingle(v => v.IdFormation == idcou, false);

            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }
        public ActionResult DeleteFormation(bool? saveChangesError)
        {
            int? id = Convert.ToInt32(TempData["deletformation"]);
            Repository<Formation> _reptoutabs1 = new Repository<Formation>(AppConfig.DbConnexionString);

          


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Formation cour = _reptoutabs1.GetSingle(v => v.IdFormation == id, false);
            TempData["deletformation1"] = id;
            return View(cour);
        }

        [HttpPost, ActionName("DeleteFormation")]
        public ActionResult DeleteConfirmed5()
        {
            int? id = Convert.ToInt32(TempData["deletformation1"]);
            
            Repository<Formation> _reptoutabs1 = new Repository<Formation>(AppConfig.DbConnexionString);


            Formation cour = _reptoutabs1.GetSingle(v => v.IdFormation == id, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }
        public ActionResult ajouterFormation()
        {


            return View(new Formation());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ajouterFormation(Formation tt)
        {
            Repository<Formation> _repradbtn = new Repository<Formation>(AppConfig.DbConnexionString);





            if (ModelState.IsValid)
            {
                _repradbtn.Add(tt);
                return RedirectToAction("Index");



            }
            return View(tt);


        }

        public ActionResult EditFormation()
        {
            int? id = Convert.ToInt32(TempData["editformation"]);
            Repository<Formation> _repradbtn = new Repository<Formation>(AppConfig.DbConnexionString);
            Formation cour = _repradbtn.GetSingle(v => v.IdFormation == id, false);
            return View(cour);
        }
        [HttpPost]
        public ActionResult EditFormation(Formation cour)
        {
            Repository<Formation> _repradbtn = new Repository<Formation>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(cour);

                return RedirectToAction("Index");
            }


            return View(cour);
        }















        /// <summary>
        /// btp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>





        public ActionResult DetailsBtp(int id)
        {
            int idcou = id;

            Repository<BTP> repadmin3 = new Repository<BTP>(AppConfig.DbConnexionString);
            BTP lis1 = repadmin3.GetSingle(v => v.IdBtp == idcou, false);

            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }
        public ActionResult DeleteBtp(int id, bool? saveChangesError)
        {
            Repository<BTP> _reptoutabs1 = new Repository<BTP>(AppConfig.DbConnexionString);

            int idcou = id;


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            BTP cour = _reptoutabs1.GetSingle(v => v.IdBtp == idcou, false);
            return View(cour);
        }

        [HttpPost, ActionName("DeleteBtp")]
        public ActionResult DeleteConfirmed4(int id)
        {
            int idcou = id;
            Repository<BTP> _reptoutabs1 = new Repository<BTP>(AppConfig.DbConnexionString);


            BTP cour = _reptoutabs1.GetSingle(v => v.IdBtp == idcou, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }
        public ActionResult ajouterBtp()
        {


            return View(new BTP());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ajouterBtp(BTP tt)
        {
            Repository<BTP> _repradbtn = new Repository<BTP>(AppConfig.DbConnexionString);





            if (ModelState.IsValid)
            {
                _repradbtn.Add(tt);
                return RedirectToAction("Index");



            }
            return View(tt);


        }

        public ActionResult EditBtp(int id)
        {
            Repository<BTP> _repradbtn = new Repository<BTP>(AppConfig.DbConnexionString);
            BTP cour = _repradbtn.GetSingle(v => v.IdBtp == id, false);
            return View(cour);
        }
        [HttpPost]
        public ActionResult EditBtp(BTP cour)
        {
            Repository<BTP> _repradbtn = new Repository<BTP>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(cour);

                return RedirectToAction("Index");
            }


            return View(cour);
        }











        /// <summary>
        /// Specialite
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        public ActionResult DetailsSpecialite(int id)
        {
            int idcou = id;

            Repository<TypeSpecialite> repadmin3 = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            TypeSpecialite lis1 = repadmin3.GetSingle(v => v.IdSpetialite == idcou, false);

            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }

        public ActionResult Pssagdeletspec(int? id)
        {
            TempData["deletspec"] = id;


            return View();


        }
        public ActionResult DeleteSpecialite( bool? saveChangesError)
        {
            int? id = Convert.ToInt32(TempData["deletspec"]);
            Repository<TypeSpecialite> _reptoutabs1 = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);

           


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            TypeSpecialite cour = _reptoutabs1.GetSingle(v => v.IdSpetialite == id, false);
            TempData["deletspec1"] = id;
            return View(cour);
        }

        [HttpPost, ActionName("DeleteSpecialite")]
        public ActionResult DeleteConfirmed2()
        {
            int? id = Convert.ToInt32(TempData["deletspec1"]);
            
            Repository<TypeSpecialite> _reptoutabs1 = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);


            TypeSpecialite cour = _reptoutabs1.GetSingle(v => v.IdSpetialite == id, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }
        public ActionResult ajouterSpecialite()
        {


            return View(new TypeSpecialite());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ajouterSpecialite(TypeSpecialite tt)
        {
            Repository<TypeSpecialite> _repradbtn = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);





            if (ModelState.IsValid)
            {
                _repradbtn.Add(tt);
                return RedirectToAction("Index");



            }
            return View(tt);


        }
        public ActionResult Pssageditspec(int? id)
        {
            TempData["editspec"] = id;


            return View();


        }

        public ActionResult EditSpecialite()
        {
            int? id = Convert.ToInt32(TempData["editspec"]);
            Repository<TypeSpecialite> _repradbtn = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);
            TypeSpecialite cour = _repradbtn.GetSingle(v => v.IdSpetialite == id, false);
            return View(cour);
        }
        [HttpPost]
        public ActionResult EditSpecialite(TypeSpecialite cour)
        {
            Repository<TypeSpecialite> _repradbtn = new Repository<TypeSpecialite>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(cour);

                return RedirectToAction("Index");
            }


            return View(cour);
        }


       

















        /// <summary>
        /// cour
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>


        public ActionResult Pssageditcour1(int? id)
        {
            TempData["editcour"] = id;


            return View();


        }
        public ActionResult Pssagdeletcour1(int? id)
        {
            TempData["deletcour"] = id;


            return View();


        }

        

        public ActionResult Detailscour(int id)
        {
            int idcou = id;

            Repository<Cour> repadmin3 = new Repository<Cour>(AppConfig.DbConnexionString);
            Cour lis1 = repadmin3.GetSingle(v => v.IdCour == idcou, false);

            if (lis1 == null)
            {
                return HttpNotFound();
            }

            return View(lis1);


        }
        public ActionResult Deletecour(bool? saveChangesError)
        {
            int? id = Convert.ToInt32(TempData["deletcour"]);
            Repository<Cour> _reptoutabs1 = new Repository<Cour>(AppConfig.DbConnexionString);

         


            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            Cour cour = _reptoutabs1.GetSingle(v => v.IdCour == id, false);
            TempData["deletcour1"] = id;
            return View(cour);
        }

        [HttpPost, ActionName("Deletecour")]
        public ActionResult DeleteConfirmed1()
        {
            int? id = Convert.ToInt32(TempData["deletcour1"]);
            Repository<Cour> _reptoutabs1 = new Repository<Cour>(AppConfig.DbConnexionString);


            Cour cour = _reptoutabs1.GetSingle(v => v.IdCour == id, false);
            _reptoutabs1.Delete(cour);



            return RedirectToAction("Index");
        }
        public ActionResult ajoutercour()
        {


            return View(new Cour());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ajoutercour(Cour tt)
        {
            Repository<Cour> _repradbtn = new Repository<Cour>(AppConfig.DbConnexionString);



            try
            {

                if (ModelState.IsValid)
                {
                    _repradbtn.Add(tt);
                    return RedirectToAction("Index");



                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(tt);


        }

        public ActionResult Editcour()
        {
            int? id = Convert.ToInt32(TempData["editcour"]);
            Repository<Cour> _repradbtn = new Repository<Cour>(AppConfig.DbConnexionString);
            Cour cour = _repradbtn.GetSingle(v => v.IdCour == id, false);
            return View(cour);
        }
        [HttpPost]
        public ActionResult Editcour(Cour cour)
        {

            Repository<Cour> _repradbtn = new Repository<Cour>(AppConfig.DbConnexionString);

            if (ModelState.IsValid)
            {
                _repradbtn.Update(cour);

                return RedirectToAction("Index");
            }


            return View(cour);
        }


    }




}
