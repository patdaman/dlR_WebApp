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

using System.IO;
using System.Data.SqlClient;

namespace RapidInventoryWebApp
{
	public partial class UploadItemData : System.Web.UI.Page
	{
		private bool InstanceFieldsInitialized = false;

		public UploadItemData()
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
		private SqlCommand sqlCmd;

		private string companyName;
		private string location;

		protected void Page_Load(object sender, System.EventArgs e)
		{

			if (!Page.IsPostBack)
			{

				companyName = Request.QueryString["field1"];
				location = Request.QueryString["field2"];

				populateDataExportQuery(location, companyName);

			}

			sendCompanyDataToMaster();

		}

		public void sendCompanyDataToMaster()
		{

			this.Master.masterCompany = companyName;
			this.Master.masterLocation = location;

		}

		protected void btnSubmit_Click(object sender, EventArgs e)
		{

			if (ItemFileUpload.HasFile)
			{

				if (ItemFileUpload.PostedFile.ContentLength > 20971520)
				{

					lblErrMsg.ForeColor = System.Drawing.Color.Red;
					lblErrMsg.Text = "File size must be under 20 MB";

				}
				else
				{

					//check to see if directory exists
					if (!System.IO.Directory.Exists("C:\\RapidInventory\\" + companyName))
					{

						System.IO.Directory.CreateDirectory("C:\\RapidInventory\\" + companyName);

					}

					ItemFileUpload.SaveAs("C:\\RapidInventory\\" + companyName + "\\" + companyName + "_" + location + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Second + ".csv");

					string filePath = "C:\\RapidInventory\\" + companyName + "\\" + companyName + "_" + location + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Second + ".csv";

					lblErrMsg.Text = "This process can take up to 2 mins";

					companyName = Request.QueryString["field1"];
					location = Request.QueryString["field2"];

					insertItemData(filePath, companyName, location);

				}
			}

		}

		public void populateDataExportQuery(string location, string company)
		{

			try
			{

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_QUERY_FOR_ITEM_UPLOAD", dbConn);

				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = company;

				sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location;

				string result = sqlCmd.ExecuteScalar().ToString();

				txtQueryforExport.Text = result;

			}
			catch (Exception ex)
			{
				string script = "alert(" + ex.Message + ");";
				ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

			}
			finally
			{

				dbConn.Close();

			}

		}


		public void insertItemData(string filePath, string location, string company)
		{

			dbConn.Open();

			SqlCommand sqlCmd2 = new SqlCommand("ALTER DATABASE RAPIDINVENTORY SET RECOVERY BULK_LOGGED", dbConn);

			sqlCmd2.ExecuteNonQuery();

			sqlCmd = dbConn.CreateCommand();

			SqlTransaction tran = dbConn.BeginTransaction();

			sqlCmd.Connection = dbConn;

			sqlCmd.Transaction = tran;

			try
			{

				sqlCmd.CommandTimeout = 240;

				sqlCmd.CommandText = "RAPID_SP_UPLOAD_ITEM_FILE";

				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = filePath;

				sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location;

				sqlCmd.ExecuteNonQuery();

				tran.Commit();

				lblErrMsg.ForeColor = System.Drawing.Color.Green;
				lblErrMsg.Text = "Upload Successful!";

			}
			catch (Exception ex)
			{
				string script = "alert(" + ex.Message + ");";
				ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);

				tran.Rollback();

			}
			finally
			{
				SqlCommand sqlCmd3 = new SqlCommand("ALTER DATABASE RAPIDINVENTORY SET RECOVERY FULL", dbConn);

				sqlCmd3.ExecuteNonQuery();

				dbConn.Close();
			}

			dbConn.Close();


		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			btnSubmit.Click += btnSubmit_Click;
		}
	}
}