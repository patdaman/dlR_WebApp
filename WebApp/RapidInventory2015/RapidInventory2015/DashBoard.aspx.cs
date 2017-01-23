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
	public partial class DashBoard : System.Web.UI.Page
	{
		private bool InstanceFieldsInitialized = false;

		public DashBoard()
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

				populateMenuBar();
				populateDashboard(companyName, location);

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

		public void populateMenuBar()
		{

			//menuBar.Items.Add(menu2)
			companyName = Request.QueryString["field1"];
			location = Request.QueryString["field2"];

		}

		public void populateDashboard(string companyName, string location)
		{

			//set the resreshed label to the current time
			lblLastRefreshed.Text = "Last Refreshed " + DateTime.Now;

			try
			{

				dbConn.Open();

				//get counts of section scanned
				sqlCmd = new SqlCommand("RAPID_SP_GET_SECTION_COUNTS", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				lblCounts.Text = sqlCmd.ExecuteScalar().ToString();

				//get list of non validated counts
				sqlCmd = new SqlCommand("RAPID_SP_LIST_OF_NON_VALID_COUNTS", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				var sqlDataAdapter = new SqlDataAdapter(sqlCmd);

				var sqlDataSet = new DataSet();

				sqlDataAdapter.Fill(sqlDataSet);

				if (sqlDataSet.Tables.Count > 0)
				{

					gridNonValidCounts.DataSource = sqlDataSet.Tables[0];
					gridNonValidCounts.AllowPaging = true;
					gridNonValidCounts.DataBind();

				}

				//get list of sections counted 
				sqlCmd = new SqlCommand("RAPID_SP_LIST_OF_SECTIONS_COUNTED", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				sqlDataAdapter = new SqlDataAdapter(sqlCmd);

				sqlDataSet = new DataSet();

				sqlDataAdapter.Fill(sqlDataSet);

				if (sqlDataSet.Tables.Count > 0)
				{

					gridSectionsCounted.DataSource = sqlDataSet.Tables[0];
					gridSectionsCounted.AllowPaging = true;
					gridSectionsCounted.DataBind();

				}

				//get list of non validated counts
				sqlCmd = new SqlCommand("RAPID_SP_LIST_OF_NON_STOCKED_COUNT", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				sqlDataAdapter = new SqlDataAdapter(sqlCmd);

				sqlDataSet = new DataSet();

				sqlDataAdapter.Fill(sqlDataSet);

				if (sqlDataSet.Tables.Count > 0)
				{

					gridNonStocked.DataSource = sqlDataSet.Tables[0];
					gridNonStocked.AllowPaging = true;
					gridNonStocked.DataBind();

				}


			}
			catch (Exception ex)
			{
				string script = "alert(" + ex.Message + ");";
				ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
			}

			sqlCmd.Dispose();

			dbConn.Close();

		}

		public void deleteSectionCount(string sectionId, string companyName, string location)
		{

			try
			{

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_SP_DELETE_SECTION_COUNT", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@SECTION_ID", System.Data.SqlDbType.VarChar).Value = sectionId;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				int sqlResult = sqlCmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				string script = "alert(" + ex.Message + ");";
				ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
			}

			sqlCmd.Dispose();

			dbConn.Close();

			populateDashboard(companyName, location);

		}

		protected void btnRefresh_Click(object sender, EventArgs e)
		{

			//refresh page
			Response.Redirect("DashBoard.aspx?field1=" + companyName + "&field2=" + location);

		}

		protected void gridSectionsCounted_RowCommand(object sender, GridViewCommandEventArgs e)
		{

			if (e.CommandName == "ChangeSectionNumber")
			{

				//get the index of the row selected 
				int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());

				//set the edit index to the row selected
				gridSectionsCounted.EditIndex = rowIndex;

				companyName = Session["companyName"].ToString();
				location = Session["location"].ToString();

				//populate the grid 
				populateDashboard(companyName, location);
			}

		}

		//funtion called when edit button is pressed
		protected void gridSectionsCounted_RowEditing(object sender, GridViewEditEventArgs e)
		{

			//get the sectionId of the row selected
			string sectionId = gridSectionsCounted.DataKeys[e.NewEditIndex].Values["SECTION_ID"].ToString();

			//go to the section edit screen
			Response.Redirect("EditSection.aspx?field1=" + companyName + "&field2=" + location + "&field3=" + sectionId);

		}


		//function called when delete button is pressed
		protected void gridSectionsCounted_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

			//get the sectionId of the row selected
			string sectionId = gridSectionsCounted.DataKeys[e.RowIndex].Values["SECTION_ID"].ToString();

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

			//delete the section 
			deleteSectionCount(sectionId, companyName, location);

			//bind the data to the grid
			populateDashboard(companyName, location);

		}

		//function is called when data is bound to the data grid
		protected void gridSectionsCounted_RowDataBound(object sender, GridViewRowEventArgs e)
		{

		}


		protected void gridSectionsCounted_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

			//get the index of the row selected
			GridViewRow row = gridSectionsCounted.Rows[e.RowIndex];

			var oldSectionId = gridSectionsCounted.DataKeys[e.RowIndex].Values["SECTION_ID"].ToString();

			TextBox newSectionId = row.FindControl("txtSectionId") as TextBox;

			try
			{

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_SP_UPDATE_SECTION_NUMBER", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@OLD_SECTION_ID", System.Data.SqlDbType.VarChar).Value = oldSectionId;

				sqlCmd.Parameters.Add("@NEW_SECTION_ID", System.Data.SqlDbType.VarChar).Value = newSectionId.Text;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				sqlCmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				string script = "alert(" + ex.Message + ");";
				ScriptManager.RegisterStartupScript(this, GetType(), "ServerControlScript", script, true);
			}

			sqlCmd.Dispose();

			dbConn.Close();

			gridSectionsCounted.EditIndex = -1;

			populateDashboard(companyName, location);

		}

		private void OnPageIndexChange(object sender, GridViewPageEventArgs e)
		{

			gridNonValidCounts.PageIndex = e.NewPageIndex;

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

			populateDashboard(companyName, location);

		}

		protected void gridSectionsCounted_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{

			gridSectionsCounted.EditIndex = -1;
			populateDashboard(companyName, location);

		}

		protected void Timer1_Tick(object sender, EventArgs e)
		{

			populateDashboard(companyName, location);

		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			btnRefresh.Click += btnRefresh_Click;
			gridNonValidCounts.PageIndexChanging += OnPageIndexChange;
			Timer1.Tick += Timer1_Tick;
		}
	}
}