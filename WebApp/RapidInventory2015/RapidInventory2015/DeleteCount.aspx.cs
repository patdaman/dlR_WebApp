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

namespace RapidInventoryWebApp
{
	public partial class DeleteCount : System.Web.UI.Page
	{
		private bool InstanceFieldsInitialized = false;

		public DeleteCount()
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

				populateCountGrid(companyName, location, Page.User.Identity.Name);

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

		public void populateCountGrid(string company, string location, string username)
		{

			dbConn.Open();

			try
			{

				sqlCmd = new SqlCommand("RAPID_SP_GET_COUNT_LIST", dbConn);

				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = company;

				sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location;

				sqlCmd.Parameters.Add("@USR_NAM", SqlDbType.VarChar).Value = username;

				var sqlDataAdapter = new SqlDataAdapter(sqlCmd);

				var sqlDataSet = new DataSet();

				sqlDataAdapter.Fill(sqlDataSet);

				if (sqlDataSet.Tables.Count > 0)
				{

					gridDeleteCount.DataSource = sqlDataSet.Tables[0];
					gridDeleteCount.AllowPaging = true;
					gridDeleteCount.DataBind();

				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);

			}
			finally
			{

				sqlCmd.Dispose();

				dbConn.Close();

			}

		}

		public void deleteCount(string companyName, string location)
		{

			dbConn.Open();

			try
			{

				sqlCmd = new SqlCommand("RAPID_SP_DELETE_COUNT", dbConn);

				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location;

				sqlCmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

				MessageBox.Show(ex.Message);

			}
			finally
			{

				sqlCmd.Dispose();

				dbConn.Close();

			}


		}

		//function called when delete button is pressed
		protected void gridDeleteCount_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

				//get the company and location of the row selected
			int rowIndex = e.RowIndex;

			string deletecompany = gridDeleteCount.DataKeys[e.RowIndex].Values["COMPANY_NAM"].ToString();
			string deleteloc = gridDeleteCount.DataKeys[e.RowIndex].Values["LOC_ID"].ToString();

			//delete the section 
			deleteCount(deletecompany, deleteloc);

			//bind the data to the grid
			populateCountGrid(companyName, location, Page.User.Identity.Name);

		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
		}
	}
}