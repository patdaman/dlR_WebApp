using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RapidInventory2015.Models;

namespace RapidInventory2015.Controllers
{
    public class VarianceCRUDsController : Controller
    {
        private RapidInventory db = new RapidInventory();

        // GET: VarianceCRUDs
        public async Task<ActionResult> Index(string CompanyName, string LocId)
        {
            return View(await (from c in db.RAPID_RAW_SCAN_DATA
                               join b in db.RAPID_IM_BARCOD on new { A = c.BARCOD, B = c.COMPANY_NAM, C = c.LOC_ID, D = c.ITEM_NO }
                                             equals new { A = b.BARCOD, B = b.COMPANY_NAM, C = b.LOC_ID, D = b.ITEM_NO }
                               join t in db.RAPID_IM_CNT_TRX on new { A = c.ITEM_NO, B = c.COMPANY_NAM, C = c.LOC_ID }
                                             equals new { A = t.ITEM_NO, B = t.COMPANY_NAM, C = t.LOC_ID }
                               where c.COMPANY_NAM == CompanyName
                               where c.LOC_ID == LocId
                               select new VarianceCRUD
                               {
                                   COMPANY_NAM = c.COMPANY_NAM,
                                   LOC_ID = c.LOC_ID,
                                   SECTION_ID = c.SECTION_ID,
                                   UNIT = b.UNIT,
                                   CNT_QTY = t.CNT_QTY_1,
                                   FRZ_QTY_ON_HND = t.FRZ_QTY_ON_HND,
                                   BARCOD = c.BARCOD,
                                   ITEM_NO = c.ITEM_NO,
                               }).ToListAsync());
        }

        // GET: VarianceCRUDs/Details/5
        public async Task<ActionResult> Details(string ItemNo, string CompanyName, string LocId, string SectionId)
        {
            if ((ItemNo == null) || (CompanyName == null) || (LocId == null) || (SectionId == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var CRUD = await (from c in db.RAPID_RAW_SCAN_DATA
                                      join b in db.RAPID_IM_BARCOD on new { A = c.BARCOD, B = c.COMPANY_NAM, C = c.LOC_ID, D = c.ITEM_NO }
                                                    equals new { A = b.BARCOD, B = b.COMPANY_NAM, C = b.LOC_ID, D = b.ITEM_NO }
                                      join t in db.RAPID_IM_CNT_TRX on new { A = c.ITEM_NO, B = c.COMPANY_NAM, C = c.LOC_ID }
                                                    equals new { A = t.ITEM_NO, B = t.COMPANY_NAM, C = t.LOC_ID }
                                      where c.COMPANY_NAM == CompanyName
                                      where c.LOC_ID == LocId
                                      where c.ITEM_NO == ItemNo
                                      where c.SECTION_ID == SectionId
                                      select new VarianceCRUD
                                      {
                                          COMPANY_NAM = c.COMPANY_NAM,
                                          LOC_ID = c.LOC_ID,
                                          SECTION_ID = c.SECTION_ID,
                                          UNIT = b.UNIT,
                                          CNT_QTY = t.CNT_QTY_1,
                                          FRZ_QTY_ON_HND = t.FRZ_QTY_ON_HND,
                                          BARCOD = c.BARCOD,
                                          ITEM_NO = c.ITEM_NO,
                                      }).FirstOrDefaultAsync();
            if (CRUD == null)
            {
                return HttpNotFound();
            }
            return View(CRUD);
        }

        // GET: VarianceCRUDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VarianceCRUDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "COMPANY_NAM,LOC_ID,SECTION_ID,BARCOD,ITEM_NO,UNIT,CNT_QTY,FRZ_QTY_ON_HND")] VarianceCRUD varianceCRUD)
        {
            if (ModelState.IsValid)
            {
                db.VarianceCRUDs.Add(varianceCRUD);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(varianceCRUD);
        }

        // GET: VarianceCRUDs/Edit/5
        public async Task<ActionResult> Edit(string ItemNo, string CompanyName, string LocId, string SectionId)
        {
            if ((ItemNo == null) || (CompanyName == null) || (LocId == null) || (SectionId == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var varianceCRUD = await (from c in db.RAPID_RAW_SCAN_DATA
                                        join b in db.RAPID_IM_BARCOD on new { A = c.BARCOD, B = c.COMPANY_NAM, C = c.LOC_ID, D = c.ITEM_NO }
                                                                equals new { A = b.BARCOD, B = b.COMPANY_NAM, C = b.LOC_ID, D = b.ITEM_NO }
                                        join t in db.RAPID_IM_CNT_TRX on new { A = c.ITEM_NO, B = c.COMPANY_NAM, C = c.LOC_ID }
                                                                equals new { A = t.ITEM_NO, B = t.COMPANY_NAM, C = t.LOC_ID }
                                        where c.COMPANY_NAM == CompanyName
                                        where c.LOC_ID == LocId
                                        where c.ITEM_NO == ItemNo
                                        where c.SECTION_ID == SectionId
                                        select new VarianceCRUD
                                        { COMPANY_NAM = c.COMPANY_NAM,
                                            LOC_ID = c.LOC_ID,
                                            SECTION_ID = c.SECTION_ID,
                                            UNIT = b.UNIT,
                                            CNT_QTY = t.CNT_QTY_1,
                                            FRZ_QTY_ON_HND = t.FRZ_QTY_ON_HND,
                                            BARCOD = c.BARCOD,
                                            ITEM_NO = c.ITEM_NO,
                                        }).FirstOrDefaultAsync();
            if (varianceCRUD == null)
            {
                return HttpNotFound();
            }
            return View(varianceCRUD);
        }

        // POST: VarianceCRUDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "COMPANY_NAM,LOC_ID,SECTION_ID,BARCOD,ITEM_NO,UNIT,CNT_QTY,FRZ_QTY_ON_HND")] VarianceCRUD varianceCRUD)
        {
            if (ModelState.IsValid)
            {
                var SCAN = await (from c in db.RAPID_RAW_SCAN_DATA
                            where c.COMPANY_NAM == varianceCRUD.COMPANY_NAM
                            where c.LOC_ID == varianceCRUD.LOC_ID
                            where c.SECTION_ID == varianceCRUD.SECTION_ID
                            where c.ITEM_NO == varianceCRUD.ITEM_NO
                            select c).FirstOrDefaultAsync();

                db.Entry(SCAN).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(varianceCRUD);
        }

        // GET: VarianceCRUDs/Delete/5
        public async Task<ActionResult> Delete(string ItemNo, string CompanyName, string LocId, string SectionId)
        {
            if ((ItemNo == null) || (CompanyName == null) || (LocId == null) || (SectionId == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var varianceCRUD = await (from c in db.RAPID_RAW_SCAN_DATA
                                      join b in db.RAPID_IM_BARCOD on new { A = c.BARCOD, B = c.COMPANY_NAM, C = c.LOC_ID, D = c.ITEM_NO }
                                                               equals new { A = b.BARCOD, B = b.COMPANY_NAM, C = b.LOC_ID, D = b.ITEM_NO }
                                      join t in db.RAPID_IM_CNT_TRX on new { A = c.ITEM_NO, B = c.COMPANY_NAM, C = c.LOC_ID }
                                                               equals new { A = t.ITEM_NO, B = t.COMPANY_NAM, C = t.LOC_ID }
                                      where c.COMPANY_NAM == CompanyName
                                      where c.LOC_ID == LocId
                                      where c.ITEM_NO == ItemNo
                                      where c.SECTION_ID == SectionId
                                      select new VarianceCRUD
                                      {
                                          COMPANY_NAM = c.COMPANY_NAM,
                                          LOC_ID = c.LOC_ID,
                                          SECTION_ID = c.SECTION_ID,
                                          UNIT = b.UNIT,
                                          CNT_QTY = t.CNT_QTY_1,
                                          FRZ_QTY_ON_HND = t.FRZ_QTY_ON_HND,
                                          BARCOD = c.BARCOD,
                                          ITEM_NO = c.ITEM_NO,
                                      }).FirstOrDefaultAsync();
            if (varianceCRUD == null)
            {
                return HttpNotFound();
            }
            return View(varianceCRUD);
        }

        // POST: VarianceCRUDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string ItemNo, string CompanyName, string LocId, string SectionId)
        {
            var SCAN = await (from c in db.RAPID_RAW_SCAN_DATA
                              where c.COMPANY_NAM == CompanyName
                              where c.LOC_ID == LocId
                              where c.ITEM_NO == ItemNo
                              where c.SECTION_ID == SectionId
                              select c).FirstOrDefaultAsync();
            db.RAPID_RAW_SCAN_DATA.Remove(SCAN);
            await db.SaveChangesAsync();
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
