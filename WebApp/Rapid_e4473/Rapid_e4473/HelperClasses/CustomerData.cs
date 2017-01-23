using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web.Mvc;
using Rapid_e4473;
using Rapid_e4473.Models;

namespace Rapid_e4473.HelperClasses
{
    public class CustomerData
    {
        public void updateCustDB(CUSTOMER CustObj)
        {
            // Check Database for existing barcode
            using (RAPID_e4473Entities db = new RAPID_e4473Entities())
            {
                if (CustObj.CUST_ID == null || CustObj.CUST_ID.ToString() != "0")
                {
                    db.CUSTOMERS.Attach(CustObj);
                    var entry = db.Entry(CustObj);
                    entry.Property(c => c.EMAIL).IsModified = true;
                    entry.Property(c => c.FIRST_NAME).IsModified = true;
                    entry.Property(c => c.LAST_NAME).IsModified = true;
                    entry.Property(c => c.MIDDLE_NAME).IsModified = true;
                    entry.Property(c => c.ADDRS_1).IsModified = true;
                    entry.Property(c => c.ADDRS_2).IsModified = true;
                    entry.Property(c => c.CITY).IsModified = true;
                    entry.Property(c => c.COUNTY).IsModified = true;
                    entry.Property(c => c.STATE).IsModified = true;
                    entry.Property(c => c.ZIP_COD).IsModified = true;
                    entry.Property(c => c.PLACE_OF_BIRTH_CITY).IsModified = true;
                    entry.Property(c => c.PLACE_OF_BIRTH_STATE).IsModified = true;
                    entry.Property(c => c.PLACE_OF_BIRTH_FOREIGN).IsModified = true;
                    entry.Property(c => c.HEIGHT_FT).IsModified = true;
                    entry.Property(c => c.HEIGHT_IN).IsModified = true;
                    entry.Property(c => c.WEIGHT).IsModified = true;
                    entry.Property(c => c.GENDER).IsModified = true;
                    entry.Property(c => c.BIRTHDATE).IsModified = true;
                    entry.Property(c => c.SOC_SEC_NUM).IsModified = true;
                    entry.Property(c => c.UPIN).IsModified = true;
                    entry.Property(c => c.ETHNICITY).IsModified = true;
                    entry.Property(c => c.RACE).IsModified = true;
                    entry.Property(c => c.C11A).IsModified = true;
                    entry.Property(c => c.C11B).IsModified = true;
                    entry.Property(c => c.C11C).IsModified = true;
                    entry.Property(c => c.C11D).IsModified = true;
                    entry.Property(c => c.C11E).IsModified = true;
                    entry.Property(c => c.C11F).IsModified = true;
                    entry.Property(c => c.C11G).IsModified = true;
                    entry.Property(c => c.C11H).IsModified = true;
                    entry.Property(c => c.C11I).IsModified = true;
                    entry.Property(c => c.C11J).IsModified = true;
                    entry.Property(c => c.C11K).IsModified = true;
                    entry.Property(c => c.C11L).IsModified = true;
                    entry.Property(c => c.ALIEN).IsModified = true;
                    entry.Property(c => c.RESIDENCE_STATE).IsModified = true;
                    entry.Property(c => c.CITIZENSHIP).IsModified = true;
                    entry.Property(c => c.ALIEN_NUM).IsModified = true;
                    db.SaveChanges();
                }
                else
                {
                    // Insert new Cust data in database
                    CustObj.TIMESTAMP = DateTime.Now;
                    db.CUSTOMERS.Add(CustObj);
                    db.SaveChanges();
                }
            }
        }

        public CUSTOMER fillCust(string Barcode)
        {
            //Querying with LINQ to Entities 
            using (var context = new RAPID_e4473Entities())
            {
                var Query = from c in context.CUSTOMERS
                            where c.BARCODE == Barcode
                            select c;

                var cust = Query.FirstOrDefault<CUSTOMER>();
                return cust;
            }
        }

        public IEnumerable<CUSTOMER> fillCustEmailList(string Email)
        {
            using (var dc = new RAPID_e4473Entities())
            {
                var Query = from c in dc.CUSTOMERS
                                    where c.EMAIL == Email
                                    select c;
                IEnumerable<CUSTOMER> CustObjList = Query.DefaultIfEmpty().ToList().Cast<CUSTOMER>();
                if (!CustObjList.Any())
                {
                    return Enumerable.Empty<CUSTOMER>();
                }
                try
                {
                    return CustObjList;
                }
                catch
                {
                    return Enumerable.Empty<CUSTOMER>();
                }
            }
        }

        public bool CheckCustForNull(CUSTOMER CustObj)
        {
            var nullCheck = CheckCustForEmptyValues(CustObj);
            if (nullCheck.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ColumnArrayToString(string[] array)
        {
	        // Use string Join to concatenate the string elements.
	        string result = string.Join("<br />", array);
	        return result;
        }

        public string[] CheckCustForEmptyValues(CUSTOMER CustObj)
        {
            List<string> missing = new List<string>();
            if (string.IsNullOrWhiteSpace(CustObj.FIRST_NAME))
            {
                missing.Add("<a href=\"_1a\">First Name</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.LAST_NAME))
            {
                missing.Add("<a href=\"_1a\">Last Name</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.MIDDLE_NAME))
            {
                missing.Add("<a href=\"_1b\">Middle Name</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ADDRS_1))
            {
                missing.Add("<a href=\"_2\">Address 1</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ADDRS_2))
            {
                missing.Add("<a href=\"_2\">Address 2</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.CITY))
            {
                missing.Add("<a href=\"_2\">City</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.COUNTY))
            {
                missing.Add("<a href=\"_2\">County</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.STATE))
            {
                missing.Add("<a href=\"_2\">State</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ZIP_COD))
            {
                missing.Add("<a href=\"_2\">Zip Code</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.PLACE_OF_BIRTH_CITY))
            {
                missing.Add("<a href=\"_3b\">City of Birth</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.PLACE_OF_BIRTH_STATE))
            {
                missing.Add("<a href=\"_3b\">State of Birth</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.PLACE_OF_BIRTH_FOREIGN))
            {
                missing.Add("<a href=\"_3c\">Foreign Place of Birth</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.HEIGHT_FT))
            {
                missing.Add("<a href=\"_4\">Height - Ft</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.HEIGHT_IN))
            {
                missing.Add("<a href=\"_4\">Height - In</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.WEIGHT))
            {
                missing.Add("<a href=\"_5\">Weight</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.GENDER))
            {
                missing.Add("<a href=\"_6\">Gender</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.BIRTHDATE))
            {
                missing.Add("<a href=\"_7\">Birthdate</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.SOC_SEC_NUM))
            {
                missing.Add("<a href=\"_8\">Social Security Number</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.UPIN))
            {
                missing.Add("<a href=\"_9\">UPIN</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ETHNICITY))
            {
                missing.Add("<a href=\"_10a\">Ethnicity</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.RACE))
            {
                missing.Add("<a href=\"_10b\">Race</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11A))
            {
                missing.Add("<a href=\"_a11\">11A</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11B))
            {
                missing.Add("<a href=\"_b11\">11B</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11C))
            {
                missing.Add("<a href=\"_c11\">11C</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11D))
            {
                missing.Add("<a href=\"_d11\">11D</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11E))
            {
                missing.Add("<a href=\"_e11\">11E</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11F))
            {
                missing.Add("<a href=\"_f11\">11F</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11G))
            {
                missing.Add("<a href=\"_g11\">11G</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11H))
            {
                missing.Add("<a href=\"_h11\">11H</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11I))
            {
                missing.Add("<a href=\"_i11\">11I</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11J))
            {
                missing.Add("<a href=\"_j11\">11J</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11K))
            {
                missing.Add("<a href=\"_k11\">11K</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.C11L))
            {
                missing.Add("<a href=\"_l11\">11L</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ALIEN))
            {
                missing.Add("<a href=\"_12\">Alien Status</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.RESIDENCE_STATE))
            {
                missing.Add("<a href=\"_13\">State of Residence</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.CITIZENSHIP))
            {
                missing.Add("<a href=\"_14\">Citizenship</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.ALIEN_NUM))
            {
                missing.Add("<a href=\"_15\">Alien Number</a>");
            }
            if (string.IsNullOrWhiteSpace(CustObj.EMAIL))
            {
                missing.Add("<a href=\"submit\">Email</a>");
            }
            return missing.ToArray();

        }

        public string displayRaceList(string raceText)
        {
            string displayRace = " ";
            if (raceText.Contains("A"))
            {
                displayRace = "American Indian or Alaska Native";
            }
            if (raceText.Contains("N"))
            {
                if (!string.IsNullOrWhiteSpace(displayRace))
                {
                    displayRace = displayRace + " / ";
                }
                displayRace = displayRace + "Asian";
            }
            if (raceText.Contains("B"))
            {
                if (!string.IsNullOrWhiteSpace(displayRace))
                {
                    displayRace = displayRace + " / ";
                }
                displayRace = displayRace + "Black or African American";
            }
            if (raceText.Contains("H"))
            {
                if (!string.IsNullOrWhiteSpace(displayRace))
                {
                    displayRace = displayRace + " / ";
                }
                displayRace = displayRace + "Native Hawaiian or Other Pacific Islander";
            }
            if (raceText.Contains("W"))
            {
                if (!string.IsNullOrWhiteSpace(displayRace))
                {
                    displayRace = displayRace + " / ";
                }
                displayRace = displayRace + "White";
            }
            return displayRace;
        }

        internal object ColumnArrayToString(List<string> missingColumns)
        {
            throw new NotImplementedException();
        }
    }
}