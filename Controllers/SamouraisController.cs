using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BO;
using Dojo.Models;
using TP_LeDojo.Data;

namespace TP_LeDojo.Controllers
{
    public class SamouraisController : Controller
    {
        private Context db = new Context();

        // GET: Samourais
        public ActionResult Index()
        {
            return View(db.Samourais.ToList());
        }

        // GET: Samourais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // GET: Samourais/Create
        public ActionResult Create()
        {
            SamouraiVM samouraiVM = new SamouraiVM();
            samouraiVM.Armes = db.Armes.ToList();
            return View(samouraiVM);
        }

        // POST: Samourais/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                if (samouraiVM.IdSelectedArme.HasValue)
                {
                    samouraiVM.Samourai.Arme = db.Armes.FirstOrDefault(a => a.Id == samouraiVM.IdSelectedArme.Value);
                }

                // On ajoute le nouveau samouraï au DbSet, puis on enregistre les changements en base
                db.Samourais.Add(samouraiVM.Samourai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            SamouraiVM samouraiVM = new SamouraiVM();
            samouraiVM.Armes = db.Armes.ToList();
            samouraiVM.Samourai = samourai;

            if (samourai.Arme != null)
            {
                samouraiVM.IdSelectedArme = samourai.Arme.Id;
            }

            return View(samouraiVM);
        }

        // POST: Samourais/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SamouraiVM samouraiVM)
        {
            if (ModelState.IsValid)
            {
                Samourai samouraiDb = db.Samourais.Find(samouraiVM.Samourai.Id);
                samouraiDb.Force = samouraiVM.Samourai.Force;
                samouraiDb.Nom = samouraiVM.Samourai.Nom;
                samouraiDb.Arme = null;
                if (samouraiVM.IdSelectedArme.HasValue)
                {
                    samouraiDb.Arme = db.Armes.FirstOrDefault(a => a.Id == samouraiVM.IdSelectedArme.Value);
                }
                //db.Entry(samouraiVM.Samourai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(samouraiVM);
        }

        // GET: Samourais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samourai samourai = db.Samourais.Find(id);
            if (samourai == null)
            {
                return HttpNotFound();
            }
            return View(samourai);
        }

        // POST: Samourais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samourai samourai = db.Samourais.Find(id);
            db.Samourais.Remove(samourai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
