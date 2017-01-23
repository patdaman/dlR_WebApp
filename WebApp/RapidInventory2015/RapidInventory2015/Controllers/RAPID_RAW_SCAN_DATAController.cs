using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using RapidInventory2015.Models;

namespace RapidInventory2015.Controllers
{
    public class RAPID_RAW_SCAN_DATAController : ApiController
    {
        private RapidInventory db = new RapidInventory();

        // GET: api/RAPID_RAW_SCAN_DATA
        public IQueryable<RAPID_RAW_SCAN_DATA> GetRAPID_RAW_SCAN_DATA()
        {
            return db.RAPID_RAW_SCAN_DATA;
        }

        // GET: api/RAPID_RAW_SCAN_DATA/5
        [ResponseType(typeof(RAPID_RAW_SCAN_DATA))]
        public async Task<IHttpActionResult> GetRAPID_RAW_SCAN_DATA(string id)
        {
            RAPID_RAW_SCAN_DATA rAPID_RAW_SCAN_DATA = await db.RAPID_RAW_SCAN_DATA.FindAsync(id);
            if (rAPID_RAW_SCAN_DATA == null)
            {
                return NotFound();
            }

            return Ok(rAPID_RAW_SCAN_DATA);
        }

        // PUT: api/RAPID_RAW_SCAN_DATA/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRAPID_RAW_SCAN_DATA(string id, RAPID_RAW_SCAN_DATA rAPID_RAW_SCAN_DATA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rAPID_RAW_SCAN_DATA.COMPANY_NAM)
            {
                return BadRequest();
            }

            db.Entry(rAPID_RAW_SCAN_DATA).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RAPID_RAW_SCAN_DATAExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RAPID_RAW_SCAN_DATA
        [ResponseType(typeof(RAPID_RAW_SCAN_DATA))]
        public async Task<IHttpActionResult> PostRAPID_RAW_SCAN_DATA(RAPID_RAW_SCAN_DATA rAPID_RAW_SCAN_DATA)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RAPID_RAW_SCAN_DATA.Add(rAPID_RAW_SCAN_DATA);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RAPID_RAW_SCAN_DATAExists(rAPID_RAW_SCAN_DATA.COMPANY_NAM))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rAPID_RAW_SCAN_DATA.COMPANY_NAM }, rAPID_RAW_SCAN_DATA);
        }

        // DELETE: api/RAPID_RAW_SCAN_DATA/5
        [ResponseType(typeof(RAPID_RAW_SCAN_DATA))]
        public async Task<IHttpActionResult> DeleteRAPID_RAW_SCAN_DATA(string id)
        {
            RAPID_RAW_SCAN_DATA rAPID_RAW_SCAN_DATA = await db.RAPID_RAW_SCAN_DATA.FindAsync(id);
            if (rAPID_RAW_SCAN_DATA == null)
            {
                return NotFound();
            }

            db.RAPID_RAW_SCAN_DATA.Remove(rAPID_RAW_SCAN_DATA);
            await db.SaveChangesAsync();

            return Ok(rAPID_RAW_SCAN_DATA);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RAPID_RAW_SCAN_DATAExists(string id)
        {
            return db.RAPID_RAW_SCAN_DATA.Count(e => e.COMPANY_NAM == id) > 0;
        }
    }
}