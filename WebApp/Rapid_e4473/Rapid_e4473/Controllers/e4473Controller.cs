using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Rapid_e4473.Models;
using Rapid_e4473.HelperClasses;

namespace Rapid_e4473.Controllers
{
    public class e4473Controller : Controller
    {
        public CUSTOMER GetCustomer()
        {
            if (this.Session["customer"] == null)
            {
                this.Session["customer"] = new CUSTOMER();
            }
            return (CUSTOMER)this.Session["customer"];
        }

        public void RemoveCustomer()
        {
            this.Session.Remove("customer");
        }

        private SimpleAES Crypto = new SimpleAES();

        public ActionResult Index()
        {
            if (this.Session["customer"] != null)
            {
                RemoveCustomer();
            }
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel DetailsData, string BtnNext)
        {
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CUSTOMER CustObj = GetCustomer(); 
                    CustObj.EMAIL = DetailsData.EMAIL;
                    return RedirectToAction("_1a");
                }
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel DetailsData, string BtnNext)
        {
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustomerData Cust = new CustomerData();
                    getBarcodeObjects bc = new getBarcodeObjects();

                    IEnumerable<CUSTOMER> temp = Cust.fillCustEmailList(DetailsData.EMAIL).DefaultIfEmpty();

                    if (temp.Any())
                    {
                        // Return List of barcode associated with email to resend  
                        foreach (CUSTOMER CustObj in temp)
                        {
                            // Create Barcode Image based on barcodes in DB
                            var BARCODEIMG = bc.getBarcodeImage(CustObj.BARCODE);
                            var IMAGEURL = "data:image/jpg;base64," + Convert.ToBase64String((byte[])BARCODEIMG);
                            // resend email(s)
                            emailTemplate.sendMail(CustObj.BARCODE, IMAGEURL, DetailsData.EMAIL);
                        }
                    }
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
            }
            return View(DetailsData);
        }

        [HttpPost]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        public ActionResult Login(string Barcode)
        {
            LoginViewModel DetailsObj = new LoginViewModel();
            DetailsObj.BARCODE = Barcode;
            return View(DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel DetailsData, string BtnNext)
        {
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustomerData Cust = new CustomerData();
                    passwordHandler pwHandler = new passwordHandler();
                    CUSTOMER CustObj = GetCustomer();
                    var temp = Cust.fillCust(DetailsData.BARCODE);
                    if (!pwHandler.passwordMatch(DetailsData.PASSWORD, temp.PASSWORD))
                    {
                        ViewData["ErrorMessage"] = "Barcode / password incorect";
                        return View(DetailsData);
                    }
                    if (!string.IsNullOrWhiteSpace(temp.BARCODE))
                    {
                        CustObj.CUST_ID = temp.CUST_ID;
                        CustObj.BARCODE = temp.BARCODE;
                        CustObj.PASSWORD = temp.PASSWORD;
                        CustObj.FIRST_NAME = temp.FIRST_NAME;
                        CustObj.LAST_NAME = temp.LAST_NAME;
                        CustObj.MIDDLE_NAME = temp.MIDDLE_NAME;
                        CustObj.EMAIL = temp.EMAIL;
                        CustObj.TIMESTAMP = temp.TIMESTAMP;
                        CustObj.ADDRS_1 = temp.ADDRS_1;
                        CustObj.ADDRS_2 = temp.ADDRS_2;
                        CustObj.CITY = temp.CITY;
                        CustObj.COUNTY = temp.COUNTY;
                        CustObj.STATE = temp.STATE;
                        CustObj.ZIP_COD = temp.ZIP_COD;
                        CustObj.PLACE_OF_BIRTH_CITY = temp.PLACE_OF_BIRTH_CITY;
                        CustObj.PLACE_OF_BIRTH_STATE = temp.PLACE_OF_BIRTH_STATE;
                        CustObj.PLACE_OF_BIRTH_FOREIGN = temp.PLACE_OF_BIRTH_FOREIGN;
                        CustObj.HEIGHT_FT = temp.HEIGHT_FT;
                        CustObj.HEIGHT_IN = temp.HEIGHT_IN;
                        CustObj.WEIGHT = temp.WEIGHT;
                        CustObj.GENDER = temp.GENDER;
                        CustObj.BIRTHDATE = temp.BIRTHDATE;
                        CustObj.SOC_SEC_NUM = temp.SOC_SEC_NUM;
                        CustObj.UPIN = temp.UPIN;
                        CustObj.ETHNICITY = temp.ETHNICITY;
                        CustObj.RACE = temp.RACE;
                        CustObj.C11A = temp.C11A;
                        CustObj.C11B = temp.C11B;
                        CustObj.C11C = temp.C11C;
                        CustObj.C11D = temp.C11D;
                        CustObj.C11E = temp.C11E;
                        CustObj.C11F = temp.C11F;
                        CustObj.C11G = temp.C11G;
                        CustObj.C11H = temp.C11H;
                        CustObj.C11I = temp.C11I;
                        CustObj.C11J = temp.C11J;
                        CustObj.C11K = temp.C11K;
                        CustObj.C11L = temp.C11L;
                        CustObj.ALIEN = temp.ALIEN;
                        CustObj.RESIDENCE_STATE = temp.RESIDENCE_STATE;
                        CustObj.CITIZENSHIP = temp.CITIZENSHIP;
                        CustObj.ALIEN_NUM = temp.ALIEN_NUM;
                        return View("summary");
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Barcode / password incorect";
                        return View(DetailsData);
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _1a()
        {
            CUSTOMER CustObj = GetCustomer();
            _1a_Name DetailsObj = new _1a_Name();
            DetailsObj.FIRST_NAME = Crypto.DecryptString(CustObj.FIRST_NAME);
            DetailsObj.LAST_NAME = Crypto.DecryptString(CustObj.LAST_NAME);
            return View("_1a_Name", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _1a_Name(_1a_Name DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.FIRST_NAME = Crypto.EncryptToString(DetailsData.FIRST_NAME);
                    CustObj.LAST_NAME = Crypto.EncryptToString(DetailsData.LAST_NAME);
                    return RedirectToAction("_1b");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _1b()
        {
            CUSTOMER CustObj = GetCustomer();
            _1b_Name DetailsObj = new _1b_Name();
            var mname = Crypto.DecryptString(CustObj.MIDDLE_NAME);
            if (!String.IsNullOrEmpty(mname) && (mname != "NMN"))
            {
                DetailsObj.MIDDLE_NAME_CNFRM = "Y";
            }
            if (!String.IsNullOrEmpty(mname) && (mname == "NMN"))
            {
                DetailsObj.MIDDLE_NAME_CNFRM = "N";
            }
            return View("_1b_Name", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _1b_Name(_1b_Name DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_1a");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.MIDDLE_NAME_CNFRM.Equals("N"))
                    {
                        CustObj.MIDDLE_NAME = Crypto.EncryptToString("NMN");
                        return RedirectToAction("_2");
                    }
                    else
                    {
                        CustObj.MIDDLE_NAME = "";
                        return RedirectToAction("_1c");
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _1c()
        {
            CUSTOMER CustObj = GetCustomer();
            _1c_Name DetailsObj = new _1c_Name();
            var mname = Crypto.DecryptString(CustObj.MIDDLE_NAME);
            if (!String.IsNullOrEmpty(mname)&&(mname == "NMN"))
            {
                return RedirectToAction("_1b");
            } 
            else
            {
                DetailsObj.MIDDLE_NAME = mname;
                return View("_1c_Name", DetailsObj);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _1c_Name(_1c_Name DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_1b");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.MIDDLE_NAME = Crypto.EncryptToString(DetailsData.MIDDLE_NAME);
                    return RedirectToAction("_2");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _2()
        {
            CUSTOMER CustObj = GetCustomer();
            _2_Address DetailsObj = new _2_Address();
            DetailsObj.ADDRS_1 = Crypto.DecryptString(CustObj.ADDRS_1);
            var address2 = Crypto.DecryptString(CustObj.ADDRS_2);
            if (address2 == "*")
            {
                DetailsObj.ADDRS_2 = "";
            }
            else
            {
                DetailsObj.ADDRS_2 = Crypto.DecryptString(CustObj.ADDRS_2);
            }
            DetailsObj.CITY = Crypto.DecryptString(CustObj.CITY);
            DetailsObj.COUNTY = Crypto.DecryptString(CustObj.COUNTY);
            DetailsObj.STATE = Crypto.DecryptString(CustObj.STATE);
            DetailsObj.ZIP_COD = Crypto.DecryptString(CustObj.ZIP_COD);
            return View("_2_Address", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _2_Address(_2_Address DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_1c");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.ADDRS_1 = Crypto.EncryptToString(DetailsData.ADDRS_1);
                    if (DetailsData.ADDRS_2 == null)
                    {
                        CustObj.ADDRS_2 = Crypto.EncryptToString("*");
                    }
                    else
                    {
                        CustObj.ADDRS_2 = Crypto.EncryptToString(DetailsData.ADDRS_2);
                    }
                    CustObj.CITY = Crypto.EncryptToString(DetailsData.CITY);
                    CustObj.STATE = Crypto.EncryptToString(DetailsData.STATE);
                    CustObj.COUNTY = Crypto.EncryptToString(DetailsData.COUNTY);
                    CustObj.ZIP_COD = Crypto.EncryptToString(DetailsData.ZIP_COD);
                    return RedirectToAction("_3a");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _3a()
        {
            CUSTOMER CustObj = GetCustomer();
            _3a_Place_of_Birth DetailsObj = new _3a_Place_of_Birth();
            var pob = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_FOREIGN);
            if (!String.IsNullOrEmpty(pob)&&(pob != "*"))
            {
                DetailsObj.PLACE_OF_BIRTH_CNFRM = "N";
            }
            else
            {
                var us = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_CITY);
                if (!String.IsNullOrEmpty(us)&&(us != "*"))
                {
                    DetailsObj.PLACE_OF_BIRTH_CNFRM = "Y";
                } 
            }
            return View("_3a_Place_of_Birth", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _3a_Place_of_Birth(_3a_Place_of_Birth DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer(); 
            if (BtnPrevious != null)
            {
                return RedirectToAction("_2");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.PLACE_OF_BIRTH_CNFRM.Equals("Y"))
                    {
                        var city = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_CITY);
                        if (!String.IsNullOrEmpty(city)&&(city == "*"))
                        {
                            CustObj.PLACE_OF_BIRTH_CITY = "";
                            CustObj.PLACE_OF_BIRTH_STATE = "";
                        }
                        return RedirectToAction("_3b");
                    }
                    else
                    {
                        var state = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_FOREIGN);
                        if (!String.IsNullOrEmpty(state) && (state == "*"))
                        {
                            CustObj.PLACE_OF_BIRTH_FOREIGN = "";
                        }
                        return RedirectToAction("_3c");
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _3b()
        {
            CUSTOMER CustObj = GetCustomer();
            _3b_Place_of_Birth DetailsObj = new _3b_Place_of_Birth();
            if (Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_CITY) == "*")
            {
                return RedirectToAction("_3a");
            }
            DetailsObj.PLACE_OF_BIRTH_CITY = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_CITY);
            DetailsObj.PLACE_OF_BIRTH_STATE = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_STATE);
            return View("_3b_Place_of_Birth", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _3b_Place_of_Birth(_3b_Place_of_Birth DetailsData, string BtnPrevious, string BtnNext)
        {
            if (BtnPrevious != null)
            {
                return RedirectToAction("_3a");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CUSTOMER CustObj = GetCustomer();
                    CustObj.PLACE_OF_BIRTH_CITY = Crypto.EncryptToString(DetailsData.PLACE_OF_BIRTH_CITY);
                    CustObj.PLACE_OF_BIRTH_STATE = Crypto.EncryptToString(DetailsData.PLACE_OF_BIRTH_STATE);
                    CustObj.PLACE_OF_BIRTH_FOREIGN = Crypto.EncryptToString("*");
                    return RedirectToAction("_4");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _3c()
        {
            CUSTOMER CustObj = GetCustomer();
            _3c_Place_of_Birth DetailsObj = new _3c_Place_of_Birth();
            var foreign = Crypto.DecryptString(CustObj.PLACE_OF_BIRTH_FOREIGN);
            if (foreign == "*")
            {
                return RedirectToAction("_3b");
            }
            DetailsObj.PLACE_OF_BIRTH_FOREIGN = foreign;
            return View("_3c_Place_of_Birth", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _3c_Place_of_Birth(_3c_Place_of_Birth DetailsData, string BtnPrevious, string BtnNext)
        {
            if (BtnPrevious != null)
            {
                return RedirectToAction("_3a");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CUSTOMER CustObj = GetCustomer();
                    CustObj.PLACE_OF_BIRTH_CITY = Crypto.EncryptToString("*");
                    CustObj.PLACE_OF_BIRTH_STATE = Crypto.EncryptToString("*");
                    CustObj.PLACE_OF_BIRTH_FOREIGN = Crypto.EncryptToString(DetailsData.PLACE_OF_BIRTH_FOREIGN);
                    return RedirectToAction("_4");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _4()
        {
            CUSTOMER CustObj = GetCustomer();
            _4_Height DetailsObj = new _4_Height();
            DetailsObj.HEIGHT_FT = Crypto.DecryptString(CustObj.HEIGHT_FT);
            DetailsObj.HEIGHT_IN = Crypto.DecryptString(CustObj.HEIGHT_IN);
            return View("_4_Height", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _4_Height(_4_Height DetailsData, string BtnPrevious, string BtnNext)
        {
            if (BtnPrevious != null)
            {
                return RedirectToAction("_3c");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CUSTOMER CustObj = GetCustomer();
                    CustObj.HEIGHT_FT = Crypto.EncryptToString(DetailsData.HEIGHT_FT);
                    CustObj.HEIGHT_IN = Crypto.EncryptToString(DetailsData.HEIGHT_IN);
                    return RedirectToAction("_5");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _5()
        {
            CUSTOMER CustObj = GetCustomer();
            _5_Weight DetailsObj = new _5_Weight();
            DetailsObj.WEIGHT = Crypto.DecryptString(CustObj.WEIGHT);
            return View("_5_Weight", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _5_Weight(_5_Weight DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_4");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.WEIGHT = Crypto.EncryptToString(DetailsData.WEIGHT);
                    return RedirectToAction("_6");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _6()
        {
            CUSTOMER CustObj = GetCustomer();
            _6_Gender DetailsObj = new _6_Gender();
            DetailsObj.GENDER = Crypto.DecryptString(CustObj.GENDER);
            return View("_6_Gender", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _6_Gender(_6_Gender DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_5");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.GENDER = Crypto.EncryptToString(DetailsData.GENDER);
                    return RedirectToAction("_7");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _7()
        {
            CUSTOMER CustObj = GetCustomer();
            _7_Birth_Date DetailsObj = new _7_Birth_Date();
            var birth = (Crypto.DecryptString(CustObj.BIRTHDATE));
            if (!String.IsNullOrWhiteSpace(birth))
            {
                DetailsObj.BIRTHDATE = Convert.ToDateTime(birth);
            }
            return View("_7_Birth_Date", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _7_Birth_Date(_7_Birth_Date DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_6");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.BIRTHDATE == null)
                    {
                        return View("_7_Birth_Date");
                    }
                    else
                    {
                        try
                        {
                            CustObj.BIRTHDATE = Crypto.EncryptToString(DetailsData.BIRTHDATE.ToString());
                            return RedirectToAction("_8");
                        }
                        catch
                        {
                            return View("_7_Birth_Date");
                        }
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _8()
        {
            CUSTOMER CustObj = GetCustomer();
            _8_Social_Security DetailsObj = new _8_Social_Security();
            var soc_sec = Crypto.DecryptString(CustObj.SOC_SEC_NUM);
            if (!String.IsNullOrEmpty(soc_sec)&&(soc_sec != "*"))
            {
                DetailsObj.SOC_SEC_NUM = soc_sec;
            }
            else
            {
                DetailsObj.SOC_SEC_NUM = "";
            }
            return View("_8_Social_Security", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _8_Social_Security(_8_Social_Security DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_7");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.SOC_SEC_NUM == null)
                    {
                        CustObj.SOC_SEC_NUM = Crypto.EncryptToString("*");
                    }
                    else
                    {
                        CustObj.SOC_SEC_NUM = Crypto.EncryptToString(DetailsData.SOC_SEC_NUM);
                    }
                    return RedirectToAction("_9");
                }
            }
            return View(DetailsData);
        }
        public ActionResult _9()
        {
            CUSTOMER CustObj = GetCustomer();
            _9_UPIN DetailsObj = new _9_UPIN();
            var upin = Crypto.DecryptString(CustObj.UPIN);
            if (!String.IsNullOrEmpty(upin) && (upin != "*"))
            {
                DetailsObj.UPIN = upin;
            }
            else
            {
                DetailsObj.UPIN = "";
            }
            return View("_9_UPIN", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _9_UPIN(_9_UPIN DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_8");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.UPIN == null)
                    {
                        CustObj.UPIN = Crypto.EncryptToString("*");
                    }
                    else
                    {
                        CustObj.UPIN = Crypto.EncryptToString(DetailsData.UPIN);
                    }
                    return RedirectToAction("_10a");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _10a()
        {
            CUSTOMER CustObj = GetCustomer();
            _10a_Ethnicity DetailsObj = new _10a_Ethnicity();
            DetailsObj.ETHNICITY = Crypto.DecryptString(CustObj.ETHNICITY);
            return View("_10a_Ethnicity", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _10a_Ethnicity(_10a_Ethnicity DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_9");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.ETHNICITY = Crypto.EncryptToString(DetailsData.ETHNICITY);
                    return RedirectToAction("_10b");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _10b()
        {
            CUSTOMER CustObj = GetCustomer();
            _10b_Race DetailsObj = new _10b_Race();
            // DetailsObj.RACE = Crypto.DecryptString(CustObj.RACE);
            var races = Crypto.DecryptString(CustObj.RACE);
            if (races.Contains("A"))
            {
                DetailsObj.RACE_A = true;
            }
            if (races.Contains("N"))
            {
                DetailsObj.RACE_N = true;
            }
            if (races.Contains("B"))
            {
                DetailsObj.RACE_B = true;
            }
            if (races.Contains("H"))
            {
                DetailsObj.RACE_H = true;
            }
            if (races.Contains("W"))
            {
                DetailsObj.RACE_W = true;
            }
            return View("_10b_Race", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _10b_Race(_10b_Race DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_10a");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    var AllRace = "*";
                    if (DetailsData.RACE_A.Equals(true))
                    {   AllRace = "A"; }
                    if (DetailsData.RACE_N.Equals(true))
                    {
                        if (AllRace.Equals("*"))
                        {   AllRace = "N";}
                        else
                        {   AllRace = AllRace + "N"; }
                    }
                    if (DetailsData.RACE_B.Equals(true))
                    {
                        if (AllRace.Equals("*"))
                        {   AllRace = "B";}
                        else
                        {   AllRace = AllRace + "B";}
                    }
                    if (DetailsData.RACE_H.Equals(true))
                    {
                        if (AllRace.Equals("*"))
                        {   AllRace = "H"; }
                        else
                        {   AllRace = AllRace + "H";}
                    }
                    if (DetailsData.RACE_W.Equals(true))
                    {
                        if (AllRace.Equals("*"))
                        {   AllRace = "W"; }
                        else
                        {   AllRace = AllRace + "W"; }
                    }
                    if (AllRace.Equals("*"))
                    {
                        ViewData["Validation"] = "Check required of one or more.";
                        return View("_10b_Race");
                    }
                    else
                    {
                        CustObj.RACE = Crypto.EncryptToString(AllRace);
                        return RedirectToAction("_a11");
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _a11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11a DetailsObj = new _11a();
            DetailsObj.C11A = Crypto.DecryptString(CustObj.C11A);
            return View("_11a", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11a(_11a DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_10b");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11A = Crypto.EncryptToString(DetailsData.C11A);
                    return RedirectToAction("_b11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _b11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11b DetailsObj = new _11b();
            DetailsObj.C11B = Crypto.DecryptString(CustObj.C11B);
            return View("_11b", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11b(_11b DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {

                return RedirectToAction("_a11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11B = Crypto.EncryptToString(DetailsData.C11B);
                    return RedirectToAction("_c11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _c11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11c DetailsObj = new _11c();
            DetailsObj.C11C = Crypto.DecryptString(CustObj.C11C);
            return View("_11c", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11c(_11c DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_b11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11C = Crypto.EncryptToString(DetailsData.C11C);
                    return RedirectToAction("_d11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _d11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11d DetailsObj = new _11d();
            DetailsObj.C11D = Crypto.DecryptString(CustObj.C11D);
            return View("_11d", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11d(_11d DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_c11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11D = Crypto.EncryptToString(DetailsData.C11D);
                    return RedirectToAction("_e11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _e11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11e DetailsObj = new _11e();
            DetailsObj.C11E = Crypto.DecryptString(CustObj.C11E);
            return View("_11e", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11e(_11e DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_d11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11E = Crypto.EncryptToString(DetailsData.C11E);
                    return RedirectToAction("_f11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _f11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11f DetailsObj = new _11f();
            DetailsObj.C11F = Crypto.DecryptString(CustObj.C11F);
            return View("_11f", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11f(_11f DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_e11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11F = Crypto.EncryptToString(DetailsData.C11F);
                    return RedirectToAction("_g11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _g11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11g DetailsObj = new _11g();
            DetailsObj.C11G = Crypto.DecryptString(CustObj.C11G);
            return View("_11g", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11g(_11g DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_f11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11G = Crypto.EncryptToString(DetailsData.C11G);
                    return RedirectToAction("_h11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _h11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11h DetailsObj = new _11h();
            DetailsObj.C11H = Crypto.DecryptString(CustObj.C11H);
            return View("_11h", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11h(_11h DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_g11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11H = Crypto.EncryptToString(DetailsData.C11H);
                    return RedirectToAction("_i11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _i11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11i DetailsObj = new _11i();
            DetailsObj.C11I = Crypto.DecryptString(CustObj.C11I);
            return View("_11i", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11i(_11i DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                 return RedirectToAction("_h11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11I = Crypto.EncryptToString(DetailsData.C11I);
                    return RedirectToAction("_j11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _j11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11j DetailsObj = new _11j();
            DetailsObj.C11J = Crypto.DecryptString(CustObj.C11J);
            return View("_11j", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11j(_11j DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_i11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11J = Crypto.EncryptToString(DetailsData.C11J);
                    return RedirectToAction("_k11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _k11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11k DetailsObj = new _11k();
            DetailsObj.C11K = Crypto.DecryptString(CustObj.C11K);
            return View("_11k", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11k(_11k DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_j11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11K = Crypto.EncryptToString(DetailsData.C11K);
                    return RedirectToAction("_l11");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _l11()
        {
            CUSTOMER CustObj = GetCustomer();
            _11l DetailsObj = new _11l();
            DetailsObj.C11L = Crypto.DecryptString(CustObj.C11L);
            return View("_11l", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _11l(_11l DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_k11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.C11L = Crypto.EncryptToString(DetailsData.C11L);
                    if (DetailsData.C11L.Equals("N"))
                    {
                        CustObj.ALIEN = Crypto.EncryptToString("*");
                        return RedirectToAction("_13");
                    }
                    else
                    {
                        return RedirectToAction("_12");
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _12()
        {
            CUSTOMER CustObj = GetCustomer();
            _12_Alien_Status DetailsObj = new _12_Alien_Status();
            DetailsObj.ALIEN = Crypto.DecryptString(CustObj.ALIEN);
            return View("_12_Alien_Status", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _12_Alien_Status(_12_Alien_Status DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_l11");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.ALIEN = Crypto.EncryptToString(DetailsData.ALIEN);
                    return RedirectToAction("_13");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _13()
        {
            CUSTOMER CustObj = GetCustomer();
            _13_State_of_Residence DetailsObj = new _13_State_of_Residence();
            DetailsObj.RESIDENCE_STATE = Crypto.DecryptString(CustObj.RESIDENCE_STATE);
            return View("_13_State_of_Residence", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _13_State_of_Residence(_13_State_of_Residence DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                if (String.Equals(Crypto.DecryptString(CustObj.ALIEN),'Y'))
                {
                    return RedirectToAction("_12");
                }
                else
                {
                    return RedirectToAction("_l11");
                }
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.RESIDENCE_STATE = Crypto.EncryptToString(DetailsData.RESIDENCE_STATE);
                    return RedirectToAction("_14a");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _14a()
        {
            CUSTOMER CustObj = GetCustomer();
            _14a_Country_Citizenship DetailsObj = new _14a_Country_Citizenship();
            DetailsObj.CITIZEN = Crypto.DecryptString(CustObj.CITIZENSHIP);
            if (!String.IsNullOrWhiteSpace(DetailsObj.CITIZEN)  && DetailsObj.CITIZEN != "USA")
            {
                DetailsObj.CITIZEN = "Other";
            }
            return View("_14a_Country_Citizenship", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _14a_Country_Citizenship(_14a_Country_Citizenship DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_13");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    if (DetailsData.CITIZEN.Equals("USA"))
                    {
                        CustObj.ALIEN_NUM = Crypto.EncryptToString("*");
                        CustObj.CITIZENSHIP = Crypto.EncryptToString("USA");
                        return View("summary");
                    }
                    else
                    {
                        var citizen_check = Crypto.DecryptString(CustObj.CITIZENSHIP);
                        if (!String.IsNullOrWhiteSpace(citizen_check) && citizen_check == "USA")
                        {
                            if (Crypto.DecryptString(CustObj.ALIEN_NUM) == "*")
                            {
                                CustObj.ALIEN_NUM = "";
                            }
                            CustObj.CITIZENSHIP = "";
                        }
                        return RedirectToAction("_14b");
                    }
                }
            }
            return View(DetailsData);
        }

        public ActionResult _14b()
        {
            CUSTOMER CustObj = GetCustomer();
            _14b_Country_Citizenship DetailsObj = new _14b_Country_Citizenship();
            var country = Crypto.DecryptString(CustObj.CITIZENSHIP);
            if (!String.IsNullOrEmpty(country) && (country == "USA"))
            {
                RedirectToAction("_14a");
            }
            DetailsObj.FOREIGNCITIZENSHIP = Crypto.DecryptString(CustObj.CITIZENSHIP);
            return View("_14b_Country_Citizenship", DetailsObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _14b_Country_Citizenship(_14b_Country_Citizenship DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_14a");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.CITIZENSHIP = Crypto.EncryptToString(DetailsData.FOREIGNCITIZENSHIP);
                    return RedirectToAction("_15");
                }
            }
            return View(DetailsData);
        }

        public ActionResult _15()
        {
            CUSTOMER CustObj = GetCustomer();
            _15_Alien_Number DetailsObj = new _15_Alien_Number();
            var alien = Crypto.DecryptString(CustObj.ALIEN_NUM);
            if (!String.IsNullOrEmpty(alien) && (alien == "*"))
            {
                return RedirectToAction("_14a");
            }
            DetailsObj.ALIEN_NUM = Crypto.DecryptString(CustObj.ALIEN_NUM);
            return View("_15_Alien_Number", DetailsObj);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _15_Alien_Number(_15_Alien_Number DetailsData, string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_14b");
            }
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    CustObj.ALIEN_NUM = Crypto.EncryptToString(DetailsData.ALIEN_NUM);
                    return View("summary");
                }
            }
            return View(DetailsData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult summary(string BtnPrevious, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnPrevious != null)
            {
                return RedirectToAction("_15");
            }
            if (BtnNext != null)
            {
                CustomerData nullCheck = new CustomerData();
                if (nullCheck.CheckCustForNull(CustObj))
                {
                    ViewData["ErrorMessage"] = "Data is missing.  Please fill out the following forms:";
                    var missingColumns = nullCheck.CheckCustForEmptyValues(CustObj);
                    ViewData["MissingColumns"] = nullCheck.ColumnArrayToString(missingColumns);
                    return View("summary");
                }
                if (CustObj.PASSWORD != null)
                {
                    RedirectToAction("complete");
                }
                return RedirectToAction("submit");
            }
            return View();
        }

        public ActionResult submit()
        {
            submit psswd = new submit();
            return View("submit", psswd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult submit(submit psswd, string BtnNext)
        {
            CUSTOMER CustObj = GetCustomer();
            if (BtnNext != null)
            {
                if (ModelState.IsValid)
                {
                    passwordHandler pwHandler = new passwordHandler();
                    if (CustObj.PASSWORD != null)
                    {
                        if (!pwHandler.passwordMatch(psswd.PASSWORD, CustObj.PASSWORD))
                        {
                            ViewData["ErrorMessage"] = "Password Incorrect";
                            return View(psswd);
                        }
                    }
                    else
                    {
                        CustObj.PASSWORD = pwHandler.hashPassword(psswd.PASSWORD);
                    } 
                    return RedirectToAction("complete");
                }
            }
            return View(psswd);
        }

        public ActionResult complete()
        {
            CUSTOMER CustObj = GetCustomer();
            barcode model = new barcode();
            getBarcodeObjects bc = new getBarcodeObjects();
            if (string.IsNullOrWhiteSpace(CustObj.BARCODE))
            {
                CustObj.BARCODE = bc.getBarcode();
            }
            model.BARCODE = CustObj.BARCODE;
            model.BARCODEIMG = bc.getBarcodeImage(CustObj.BARCODE);
            model.IMAGEURL = "data:image/jpg;base64," + Convert.ToBase64String((byte[])model.BARCODEIMG); 
            emailTemplate.sendMail(CustObj.BARCODE, model.IMAGEURL, CustObj.EMAIL);

            // Check Database for existing barcode
            //  if not exist, insert
            //  all within this parent wrapper
            try
            {
                CustomerData cust = new CustomerData();
                cust.updateCustDB(CustObj);
                return View("barcode", model);
            }
            // Catch error on Database Update / Insert
                // and display on custom error page
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "This is the message";
                ModelState.AddModelError("", ex);
                return View("error");
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult barcode(barcode model)
        {
            RemoveCustomer();
            return View(model);
        }
    }
}