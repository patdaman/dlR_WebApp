using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Net.Mail;
using System.Drawing;
using System.Text;

namespace Rapid_e4473.HelperClasses
{
    public class emailTemplate
    {
        public static string emailBody(string barcode, string replacement, bool image)
        {
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"App_Data\emailTemplate.html");
            //string htmlBody = File.ReadAllText(path);
            if (image)
            {
                string htmlPath = HttpContext.Current.Server.MapPath("~/App_Data/emailTemplate.html");
                string htmlBody = File.ReadAllText(htmlPath);
                StringBuilder sb = new StringBuilder(htmlBody);
                sb.Replace("<IMAGE_PLACEHOLDER>", "<img src=\"cid:barcodeImage\" />");
                sb.Replace("<CUSTBARCODE>",barcode);
                return sb.ToString();
            }
            else
            {
                string txtPath = HttpContext.Current.Server.MapPath("~/App_Data/emailTemplate.txt");
                string txtBody = File.ReadAllText(txtPath);
                StringBuilder sbt = new StringBuilder(txtBody);
                sbt.Replace("<CUSTBARCODE>", barcode);
                return sbt.ToString();
            }
        }

        public static void sendMail(string barcode, string imageurl, string email)
        {
           // Email the barcode to the user

            // Read in emailSettings document for SMTP variables
            string xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/emailSettings.xml");
            var xml = XElement.Load(xmlPath);
            
            string fromAddress = xml.Element("from").Value.ToString();
            string hostAddress = xml.Element("host").Value.ToString();
            int hostPort = (int)xml.Element("port");
            string emailUsername = xml.Element("username").Value.ToString();
            string emailPassword = xml.Element("password").Value.ToString();
            string emailSubject = xml.Element("subject").Value.ToString();

            // Instantiate new email message object
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);
            mail.To.Add(email);

            // SMTP connection variables
            SmtpClient client = new SmtpClient();
            client.Port = hostPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential(emailUsername, emailPassword);
            client.Host = hostAddress;
                
            // Details within the message
            string emailBody = emailTemplate.emailBody(barcode, " ", false);
            string emailBodyImage = emailTemplate.emailBody(barcode, imageurl, true);
            mail.Subject = emailSubject;
            mail.IsBodyHtml = true;

            // Create two views: HTML and Plain Text
            AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(emailBody, null, "text/plain");
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(emailBodyImage, null, "text/html");
                
            // Create Linked Resource to include binary data of image
            LinkedResource barcodeImage = GetLinkedResource(barcode);
            htmlView.LinkedResources.Add(barcodeImage);

            // Add the two views to the message
            mail.AlternateViews.Add(plainTextView);
            mail.AlternateViews.Add(htmlView);

            // Send the message
            client.Send(mail);
        }

        private static LinkedResource GetLinkedResource(string barcode)
        {
            // BarcodeLib.dll (Barcode) Solution
            BarcodeLib.Barcode newbarcode = new BarcodeLib.Barcode()
            {
                IncludeLabel = false,
                Alignment = BarcodeLib.AlignmentPositions.CENTER,
                Width = 450,
                Height = 100,
                RotateFlipType = System.Drawing.RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            };
            Image image = newbarcode.Encode(BarcodeLib.TYPE.CODE39, barcode);
            ImageConverter ic = new ImageConverter();
            Byte[] ba = (Byte[])ic.ConvertTo(image, typeof(Byte[]));
            MemoryStream barcodeImg = new MemoryStream(ba);
            var barcodeImage = new LinkedResource(barcodeImg, "image/jpg");
            barcodeImage.ContentType.Name = "barcode.jpg";
            barcodeImage.ContentId = "barcodeImage";
            return barcodeImage;
        }
    }
}