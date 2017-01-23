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
using System.IO;

namespace RapidInventoryWebApp
{
	public partial class ExtractCount : System.Web.UI.Page
	{

		private bool InstanceFieldsInitialized = false;

		public ExtractCount()
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

		public string companyName;
		public string location;

		protected void Page_Load(object sender, System.EventArgs e)
		{


			if (!Page.IsPostBack)
			{

				companyName = Request.QueryString["field1"];
				location = Request.QueryString["field2"];

			}

			companyName = Request.QueryString["field1"];
			location = Request.QueryString["field2"];

			Session["companyName"] = companyName;
			Session["location"] = location;

			sendCompanyDataToMaster();

		}

		public void sendCompanyDataToMaster()
		{

			this.Master.masterCompany = companyName;
			this.Master.masterLocation = location;

		}

		protected void btnExtract_Click(object sender, EventArgs e)
		{

			if (!Directory.Exists("C:\\RapidInventory\\" + companyName))
			{

				Directory.CreateDirectory("C:\\RapidInventory\\" + companyName);

			}

			dbConn.Open();

			sqlCmd = new SqlCommand("RAPID_EXTRACT_COUNT", dbConn);

			sqlCmd.CommandType = CommandType.StoredProcedure;

			sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

			sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

			sqlCmd.Parameters.Add("@FILEPATH", System.Data.SqlDbType.VarChar, 4000);

			sqlCmd.Parameters["@FILEPATH"].Direction = ParameterDirection.Output;

			sqlCmd.ExecuteNonQuery();

			string filePath = sqlCmd.Parameters["@FILEPATH"].Value.ToString();

			FileInfo file = new FileInfo(filePath);

			if (file.Exists)
			{

				Response.Clear();
				Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
				Response.AddHeader("Content-Length", file.Length.ToString());
				Response.ContentType = "text/plain";
				Response.WriteFile(file.FullName);
				Response.End();

			}

			dbConn.Close();

		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			btnExtract.Click += btnExtract_Click;
		}
	}
}