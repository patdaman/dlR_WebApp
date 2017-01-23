using System.Windows.Forms;

//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace RapidInventoryWebApp
{
	public partial class Login : System.Web.UI.Page
	{

		private bool InstanceFieldsInitialized = false;

		public Login()
		{
			if (!InstanceFieldsInitialized)
			{
				InitializeInstanceFields();
				InstanceFieldsInitialized = true;
			}
		}

			private void InitializeInstanceFields()
			{
				dbConn = new SqlConnection(constring);
			}

		private string constring = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
		private SqlConnection dbConn;

		private string companyName;
		private string location;

		protected void Page_Load(object sender, EventArgs e)
		{
			//RegisterHyperLink.NavigateUrl = "Register"
			//OpenAuthLogin.ReturnUrl = Request.QueryString("ReturnUrl")
			if (!Page.IsPostBack)
			{

				fillDropDownList();

				//Response.Write("<script LANGUAGE='JavaScript' >alert('Hello');document.location='" + ResolveClientUrl("~/Login.aspx") + "';</script>")
			}


			var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
			//If Not String.IsNullOrEmpty(returnUrl) Then
			//RegisterHyperLink.NavigateUrl &= "?ReturnUrl=" & returnUrl
			//End If
		}

		public void fillDropDownList()
		{

			try
			{

				dbConn.Open();

				string cmd = "SELECT DISTINCT COMPANY_NAM + '-' + LOC_ID AS COMP_LOC FROM RAPID_USR_ACCT";

				SqlCommand sqlCmd = new SqlCommand(cmd, dbConn);

				SqlDataReader dataReader = sqlCmd.ExecuteReader();

				while (dataReader.Read())
				{

					compLocDropDown.Items.Add(dataReader["COMP_LOC"].ToString());

				}

				//clean up
				dataReader.Close();
				sqlCmd.Dispose();

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);

			}
			finally
			{

				dbConn.Close();

			}

		}

		private bool ValidateCredentials(string userName, string password, string companyLoc)
		{

			bool returnValue = false;

			//Split the company and location string
			var companyLocArray = companyLoc.Split('-');

			//assign the company name from the array
			companyName = companyLocArray[0].ToString().Trim(' ');

			//assign the locationId from the array
			location = companyLocArray[1].ToString().Trim(' ');

			try
			{

				string cmd = "RAPID_SP_USR_LOGIN";
				SqlCommand sqlCmd = new SqlCommand(cmd, dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@USR_NAM", System.Data.SqlDbType.VarChar).Value = userName;

				sqlCmd.Parameters.Add("@PWD", System.Data.SqlDbType.VarChar).Value = password;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				dbConn.Open();

				int result = 0;
				int failedLoginCount = 0;

				SqlDataReader dr = sqlCmd.ExecuteReader();

				while (dr.Read())
				{

					result = Convert.ToInt32(dr[0]);
					failedLoginCount = Convert.ToInt32(dr[1]);

				}

				if (result == 0)
				{

					//log the error

					sqlCmd = new SqlCommand("RAPID_SP_UPDATE_FAILED_LOGIN", dbConn);

					sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

					sqlCmd.Parameters.Add("@USR_NAM", System.Data.SqlDbType.VarChar).Value = userName;

					sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

					sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

					failedLoginCount = Convert.ToInt32(sqlCmd.ExecuteScalar());

					//clean up
					sqlCmd.Dispose();

					//if failed login count is equal to 4
					if (failedLoginCount == 4)
					{

						//send email to rapid
						sendNotification();

					}

					//clean up
					sqlCmd.Dispose();
					dbConn.Close();

					return false;

				}

				if (failedLoginCount > 4)
				{

					Literal1.Text = "Your account is locked out, please contact your administrator to unlock your account";

					//clean up
					sqlCmd.Dispose();
					dbConn.Close();

					return false;

				}

				dbConn.Close();

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);

			}

			return true;

		}

		private void sendNotification()
		{

			var Message = new MailMessage();
			Message.From = new MailAddress("receipts@paperlesspurchase.com");
			Message.To.Add(new MailAddress("support@rapidpos.com"));
			Message.Subject = "Rapid Physcial Count Failed Login";
			Message.Body = "User: " + UserName.Text + "has tried to login more than 4 times. Please address this issue.";

			// Replace SmtpMail.SmtpServer = server with the following:
			SmtpClient smtp = new SmtpClient("mail.paperlesspurchase.com");
			smtp.Port = 26;
			smtp.EnableSsl = true;
			smtp.Credentials = new System.Net.NetworkCredential("receipts+paperlesspurchase.com", "ReceiptsRpos!");

			try
			{

				smtp.Send(Message);

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);

			}

		}

		protected void LoginButton_ServerClick(object sender, EventArgs e)
		{

			if (ValidateCredentials(UserName.Text, Password.Text, compLocDropDown.Text) == true)
			{

				// Create user name based cookie and allow entry
				FormsAuthentication.RedirectFromLoginPage(UserName.Text, chkPersistCookie.Checked);

				//go to the main menu
				Response.Redirect("DashBoard.aspx?field1=" + companyName + "&field2=" + location);
			}
			else
			{

				Literal1.Text = "Login credentials are not correct, please try again. If you forgot your password, please contact Rapid to reset your password";
				Literal1.Visible = true;
			}

		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			LoginButton.Click += LoginButton_ServerClick;
		}
	}
}