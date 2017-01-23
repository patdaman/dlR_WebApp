// using System.Windows.Forms;

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
using System.Globalization;

namespace RapidInventoryWebApp
{
	public partial class EditSection : System.Web.UI.Page
	{
		private bool InstanceFieldsInitialized = false;

		public EditSection()
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
		private string sectionId;

		protected void Page_Load(object sender, System.EventArgs e)
		{

			if (!Page.IsPostBack)
			{

				companyName = Request.QueryString["field1"];
				location = Request.QueryString["field2"];
				sectionId = Request.QueryString["field3"];

				populateCountGrid(companyName, location, sectionId);

				Session["companyName"] = companyName;
				Session["location"] = location;
				Session["sectionId"] = sectionId;

			}

			sendCompanyDataToMaster();

		}

		public void sendCompanyDataToMaster()
		{

			this.Master.masterCompany = companyName;
			this.Master.masterLocation = location;

		}

		public void populateCountGrid(string companyn, string loc, string section)
		{

			try
			{

				dbConn.Open();

				//get list of non validated counts
				sqlCmd = new SqlCommand("RAPID_SP_GET_RAW_SCAN_DATA", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@SECTION_ID", System.Data.SqlDbType.VarChar).Value = sectionId;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				var sqlDataAdapter = new SqlDataAdapter(sqlCmd);

				var sqlDataSet = new DataSet();

				sqlDataAdapter.Fill(sqlDataSet);

				if (sqlDataSet.Tables.Count > 0)
				{

					gridSectionScans.DataSource = sqlDataSet.Tables[0];
					gridSectionScans.AllowPaging = true;
					gridSectionScans.DataBind();

				}

			}
			catch (Exception ex)
			{
                System.Windows.Forms.MessageBox.Show(ex.Message);

			}

			dbConn.Close();

		}

		//funtion called when edit button is pressed
		protected void gridSectionScans_RowEditing(object sender, GridViewEditEventArgs e)
		{

			//Set the edit index.
			gridSectionScans.EditIndex = e.NewEditIndex;
			int rowIndex = e.NewEditIndex;

			//get the location and comapny name from the current session
			location = Session["location"].ToString();
			companyName = Session["companyName"].ToString();
			sectionId = Session["sectionId"].ToString();

			//bind data to grid
			populateCountGrid(companyName, location, sectionId);

		}

		//function called when delete button is pressed
		protected void gridSectionScans_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

			//get the section id of the row selected
			string scanDate = gridSectionScans.DataKeys[e.RowIndex].Values["SCAN_DAT"].ToString();
			string barcode = gridSectionScans.DataKeys[e.RowIndex].Values["BARCOD"].ToString();
			string handheldId = gridSectionScans.DataKeys[e.RowIndex].Values["HANDHELD_ID"].ToString();

			//get the location and comapny name from the current session
			location = Session["location"].ToString();
			companyName = Session["companyName"].ToString();
			sectionId = Session["sectionId"].ToString();

			try
			{

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_SP_DELETE_SCAN_DATA", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@SCAN_DAT", System.Data.SqlDbType.VarChar).Value = scanDate;

				sqlCmd.Parameters.Add("@BARCOD", System.Data.SqlDbType.VarChar).Value = barcode;

				sqlCmd.Parameters.Add("@HANDHELD_ID", System.Data.SqlDbType.VarChar).Value = handheldId;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				sqlCmd.ExecuteScalar();

			}
			catch (Exception ex)
			{
                System.Windows.Forms.MessageBox.Show(ex.Message);

			}

			dbConn.Close();

			//bind the data to the grid
			populateCountGrid(companyName, location, sectionId);

		}

		protected void gridSectionScans_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{

			gridSectionScans.EditIndex = -1;

			//get the location and comapny name from the current session
			location = Session["location"].ToString();
			companyName = Session["companyName"].ToString();
			sectionId = Session["sectionId"].ToString();

			populateCountGrid(companyName, location, sectionId);

		}

		protected void gridSectionScans_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{

			//get the location and comapny name from the current session
			location = Session["location"].ToString();
			companyName = Session["companyName"].ToString();

			try
			{

				//get the index of the row selected
				GridViewRow row = gridSectionScans.Rows[e.RowIndex];

				//get new values from textbox
				TextBox scanDate = row.FindControl("txtScanDate") as TextBox;
				TextBox barcode = row.FindControl("txtBarcode") as TextBox;
				TextBox Descr = row.FindControl("txtDescription") as TextBox;
				TextBox Qty = row.FindControl("txtQuantity") as TextBox;
				TextBox handheldId = row.FindControl("txtHandheldId") as TextBox;

				//get the location and comapny name from the current session
				location = Session["location"].ToString();
				companyName = Session["companyName"].ToString();
				sectionId = Session["sectionId"].ToString();

				//get the section id of the row selected
				string format = "yyyy-MM-dd HH:mm:ss.fff";
				var str = Convert.ToDateTime(gridSectionScans.DataKeys[e.RowIndex].Values["SCAN_DAT"]).ToString(format, CultureInfo.InvariantCulture);
				DateTime dat = DateTime.ParseExact(str, format, CultureInfo.InvariantCulture);
				string OldScanDate = dat.ToString(format, CultureInfo.InvariantCulture);

				string Oldbarcode = gridSectionScans.DataKeys[e.RowIndex].Values["BARCOD"].ToString();
				string OldhandheldId = gridSectionScans.DataKeys[e.RowIndex].Values["HANDHELD_ID"].ToString();

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_SP_UPDATE_RAW_SCAN_DATA", dbConn);

				sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@OLD_SCAN_DAT", System.Data.SqlDbType.VarChar).Value = OldScanDate;

				sqlCmd.Parameters.Add("@OLD_BARCOD", System.Data.SqlDbType.VarChar).Value = Oldbarcode;

				sqlCmd.Parameters.Add("@OLD_HANDHELD_ID", System.Data.SqlDbType.VarChar).Value = OldhandheldId;

				sqlCmd.Parameters.Add("@SCAN_DAT", System.Data.SqlDbType.VarChar).Value = scanDate.Text;

				sqlCmd.Parameters.Add("@BARCOD", System.Data.SqlDbType.VarChar).Value = barcode.Text;

				sqlCmd.Parameters.Add("@QTY", System.Data.SqlDbType.Int).Value = Qty.Text;

				sqlCmd.Parameters.Add("@HANDHELD_ID", System.Data.SqlDbType.VarChar).Value = handheldId.Text;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = companyName;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = location;

				sqlCmd.Parameters.Add("@SECTION_ID", System.Data.SqlDbType.VarChar).Value = sectionId;

				sqlCmd.ExecuteScalar();

			}
			catch (Exception ex)
			{
                System.Windows.Forms.MessageBox.Show(ex.Message);

			}

			dbConn.Close();

			//remove the edit mode from table
			gridSectionScans.EditIndex = -1;

			//refresh grid
			populateCountGrid(companyName, location, sectionId);


		}

		//function is called when data is bound to the data grid
		protected virtual void gridSectionScans_RowDataBound(object sender, GridViewRowEventArgs e)
		{
		}

		protected void OnPageIndexChange(object sender, GridViewPageEventArgs e)
		{

			gridSectionScans.PageIndex = e.NewPageIndex;

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();
			sectionId = Session["sectionId"].ToString();

			populateCountGrid(companyName, location, sectionId);

		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			gridSectionScans.PageIndexChanging += OnPageIndexChange;
		}
	}
}