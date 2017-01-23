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

namespace RapidInventoryWebApp
{
	public partial class LocationMaintenance : System.Web.UI.Page
	{
		private bool InstanceFieldsInitialized = false;

		public LocationMaintenance()
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
		private string username;

		protected void Page_Load(object sender, System.EventArgs e)
		{

			if (!Page.IsPostBack)
			{

				companyName = Request.QueryString["field1"];
				location = Request.QueryString["field2"];

				BindGrid();

			}

			Session["companyName"] = companyName;
			Session["location"] = location;

			sendCompanyDataToMaster();

		}

		public void sendCompanyDataToMaster()
		{

			this.Master.masterCompany = companyName;
			this.Master.masterLocation = location;

		}

		private void BindGrid()
		{

			dbConn.Open();

			sqlCmd = new SqlCommand("RAPID_SP_GET_USR_LOGINS", dbConn);

			sqlCmd.CommandType = CommandType.StoredProcedure;

			var sqlDataAdapter = new SqlDataAdapter(sqlCmd);

			var sqlDataSet = new DataSet();

			sqlDataAdapter.Fill(sqlDataSet);

			if (sqlDataSet.Tables.Count > 0)
			{

				gridLocation.DataSource = sqlDataSet.Tables[0];
				gridLocation.AllowPaging = true;
				gridLocation.DataBind();

			}

			dbConn.Close();

		}




		protected void gridLocation_RowCommand(object sender, GridViewCommandEventArgs e)
		{

			if (e.CommandName == "InsertNew")
			{

				GridViewRow row = gridLocation.FooterRow;

				TextBox txtCompany = row.FindControl("txtCompanyNameNew") as TextBox;

				TextBox txtLocation = row.FindControl("txtLocationNew") as TextBox;

				TextBox txtUsername = row.FindControl("txtUsernameNew") as TextBox;

				TextBox txtPassword = row.FindControl("txtPasswordNew") as TextBox;

				Label txtFldNew = row.FindControl("txtFldNew") as Label;

				DropDownList txtForceNew = row.FindControl("txtForceNew") as DropDownList;

				DropDownList txtManagerNew = row.FindControl("txtManagerNew") as DropDownList;

				if (txtCompany != null && txtLocation != null && txtUsername != null && txtPassword != null && txtFldNew != null && txtForceNew != null && txtManagerNew != null)
				{

					dbConn.Open();

					sqlCmd = new SqlCommand("RAPID_SP_CREATE_USR", dbConn);

					sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

					sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = txtCompany.Text;

					sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = txtLocation.Text;

					sqlCmd.Parameters.Add("@USR_NAM", System.Data.SqlDbType.VarChar).Value = txtUsername.Text;

					sqlCmd.Parameters.Add("@PWD", System.Data.SqlDbType.VarChar).Value = txtPassword.Text;

					sqlCmd.Parameters.Add("@ALLOW_FORCE", System.Data.SqlDbType.VarChar).Value = txtForceNew.Text;

					sqlCmd.Parameters.Add("@IS_MANAGER", System.Data.SqlDbType.VarChar).Value = txtManagerNew.Text;

					int sqlResult = sqlCmd.ExecuteNonQuery();

					sqlCmd.Dispose();

					dbConn.Close();

					BindGrid();
				}
			}
		}

		protected void gridLocation_RowDataBound(object sender, GridViewRowEventArgs e)
		{

			//populate Allow Force drop down list
			DropDownList ddlAllowForce = null;

			if (e.Row.RowType == DataControlRowType.Footer)
			{
				ddlAllowForce = e.Row.FindControl("txtForceNew") as DropDownList;
			}

			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				ddlAllowForce = e.Row.FindControl("txtForce") as DropDownList;
			}

			if (ddlAllowForce != null)
			{
				ddlAllowForce.Items.Add("Y");
				ddlAllowForce.Items.Add("N");

				if (e.Row.RowType == DataControlRowType.DataRow)
				{

					ddlAllowForce.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ALLOW_FORCE").ToString();

				}
			}

			//populate is manager drop down list
			DropDownList ddlIsManager = null;

			if (e.Row.RowType == DataControlRowType.Footer)
			{
				ddlIsManager = e.Row.FindControl("txtManagerNew") as DropDownList;
			}

			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				ddlIsManager = e.Row.FindControl("txtManager") as DropDownList;
			}

			if (ddlAllowForce != null)
			{
				ddlIsManager.Items.Add("Y");
				ddlIsManager.Items.Add("N");

				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					ddlIsManager.SelectedValue = DataBinder.Eval(e.Row.DataItem, "IS_MANAGER").ToString();
				}
			}
		}

		protected void gridLocation_RowEditing(object sender, GridViewEditEventArgs e)
		{

			gridLocation.EditIndex = e.NewEditIndex;
			BindGrid();

		}
		protected void gridLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{

			gridLocation.EditIndex = -1;
			BindGrid();

		}

		private void OnPageIndexChange(object sender, GridViewPageEventArgs e)
		{

			gridLocation.PageIndex = e.NewPageIndex;

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

			BindGrid();

		}

		protected void gridLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

			//get the index of the row selected
			GridViewRow row = gridLocation.Rows[e.RowIndex];

			Label txtCompany = row.FindControl("lblCompanyName") as Label;

			Label txtLocation = row.FindControl("lblLocation") as Label;

			TextBox txtUsername = row.FindControl("txtUsername") as TextBox;

			TextBox txtPassword = row.FindControl("txtPassword") as TextBox;

			TextBox txtFldNew = row.FindControl("txtFld") as TextBox;

			DropDownList txtForceNew = row.FindControl("txtForce") as DropDownList;

			DropDownList txtManagerNew = row.FindControl("txtManager") as DropDownList;

			if (txtCompany != null && txtLocation != null && txtUsername != null && txtPassword != null && txtFldNew != null && txtForceNew != null && txtManagerNew != null)
			{

				var oldCompany = gridLocation.DataKeys[e.RowIndex].Values["COMPANY_NAM"].ToString();
				var oldLoc = gridLocation.DataKeys[e.RowIndex].Values["LOC_ID"].ToString();
				var oldUserName = gridLocation.DataKeys[e.RowIndex].Values["USR_NAM"].ToString();

				try
				{

					dbConn.Open();

					sqlCmd = new SqlCommand("RAPID_SP_UPDATE_COMPANY_DATA", dbConn);

					sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;

					sqlCmd.Parameters.Add("@OLD_USR_NAM", System.Data.SqlDbType.VarChar).Value = oldUserName;

					sqlCmd.Parameters.Add("@OLD_COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = oldCompany;

					sqlCmd.Parameters.Add("@OLD_LOC_ID", System.Data.SqlDbType.VarChar).Value = oldLoc;

					sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = txtCompany.Text;

					sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = txtLocation.Text;

					sqlCmd.Parameters.Add("@USR_NAM", System.Data.SqlDbType.VarChar).Value = txtUsername.Text;

					sqlCmd.Parameters.Add("@PASSWORD", System.Data.SqlDbType.VarChar).Value = txtPassword.Text;

					sqlCmd.Parameters.Add("@FLD_LGN_CNT", System.Data.SqlDbType.VarChar).Value = txtFldNew.Text;

					sqlCmd.Parameters.Add("@ALLOW_FORCE", System.Data.SqlDbType.VarChar).Value = txtForceNew.Text;

					sqlCmd.Parameters.Add("@IS_MANAGER", System.Data.SqlDbType.VarChar).Value = txtManagerNew.Text;

					int sqlResult = sqlCmd.ExecuteNonQuery();

				}
				catch (Exception ex)
				{

					System.Windows.Forms.MessageBox.Show(ex.Message);

				}

				sqlCmd.Dispose();

				dbConn.Close();

				gridLocation.EditIndex = -1;

				BindGrid();

			}

		}

		protected void gridLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

			string company = gridLocation.DataKeys[e.RowIndex].Values["COMPANY_NAM"].ToString();
			string loc = gridLocation.DataKeys[e.RowIndex].Values["LOC_ID"].ToString();
			string username = gridLocation.DataKeys[e.RowIndex].Values["USR_NAM"].ToString();

			try
			{

				dbConn.Open();

				sqlCmd = new SqlCommand("RAPID_SP_DELETE_USR", dbConn);

				sqlCmd.CommandType = CommandType.StoredProcedure;

				sqlCmd.Parameters.Add("@USR_NAM", System.Data.SqlDbType.VarChar).Value = username;

				sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = company;

				sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = loc;

				sqlCmd.ExecuteNonQuery();

				sqlCmd.Dispose();

			}
			catch (Exception ex)
			{
                System.Windows.Forms.MessageBox.Show(ex.Message);

			}

			dbConn.Close();

			BindGrid();

		}

		//function called when new location button is pressed
		protected void btnAddnewLocation_Click(object sender, EventArgs e)
		{

			try
			{

				if (txtboxnewComp != null && txtboxnewLoc != null)
				{

					string newComp = txtboxnewComp.Text;
					string newLoc = txtboxnewLoc.Text;

					dbConn.Open();

					sqlCmd = new SqlCommand("RAPID_SP_CREATE_NEW_COMPANY_LOCATION", dbConn);

					sqlCmd.CommandType = CommandType.StoredProcedure;

					sqlCmd.Parameters.Add("@COMPANY_NAM", System.Data.SqlDbType.VarChar).Value = newComp;

					sqlCmd.Parameters.Add("@LOC_ID", System.Data.SqlDbType.VarChar).Value = newLoc;

					sqlCmd.ExecuteNonQuery();


				}

			}
			catch (Exception ex)
			{
                System.Windows.Forms.MessageBox.Show(ex.Message);
			}

			dbConn.Close();

			BindGrid();

		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
			gridLocation.PageIndexChanging += OnPageIndexChange;
			btnAddnewLocation.Click += btnAddnewLocation_Click;
		}
	}
}