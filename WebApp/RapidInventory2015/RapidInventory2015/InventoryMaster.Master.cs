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
	public partial class InventoryMaster : System.Web.UI.MasterPage
	{
		//Dim companyName As String
		//Dim location As String

		private bool InstanceFieldsInitialized = false;

		public InventoryMaster()
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
		private string isAdmin;

		public string masterCompany
		{
			get
			{
				return companyName;
			}
			set
			{
				companyName = value;
			}
		}

		public string masterLocation
		{
			get
			{
				return location;
			}
			set
			{
				location = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!Page.IsPostBack)
			{

				getMenu();

				companyName = Session["companyName"].ToString();
				location = Session["location"].ToString();

			}

			companyName = Session["companyName"].ToString();
			location = Session["location"].ToString();

		}
		private void getMenu()
		{

			dbConn.Open();

			if (Page.User.Identity.Name == "RapidAdmin")
			{

				DataSet ds = new DataSet();

				DataTable dt = new DataTable();

				string sql = "SELECT * FROM RAPID_ADMIN_MENU";

				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);

				da.Fill(ds);

				dt = ds.Tables[0];

				DataRow[] drowpar = dt.Select("ParentID = 0");

				foreach (DataRow dr in drowpar)
				{
					menuBar.Items.Add(new MenuItem(dr["MenuName"].ToString(), dr["MenuID"].ToString(), "", dr["MenuLocation"].ToString() + "?field1=" + companyName + "&field2=" + location));
				}

				foreach (DataRow dr in dt.Select("ParentID > 0"))
				{
					MenuItem mnu = new MenuItem(dr["MenuName"].ToString(), dr["MenuID"].ToString(), "", dr["MenuLocation"].ToString() + "?field1=" + companyName + "&field2=" + location);
					menuBar.FindItem(dr["ParentID"].ToString()).ChildItems.Add(mnu);
				}

			}
			else
			{

				DataSet ds = new DataSet();

				DataTable dt = new DataTable();

				string sql = "SELECT * FROM RAPID_USR_MENU";

				SqlDataAdapter da = new SqlDataAdapter(sql, dbConn);

				da.Fill(ds);

				dt = ds.Tables[0];

				DataRow[] drowpar = dt.Select("ParentID = 0");

				foreach (DataRow dr in drowpar)
				{
					menuBar.Items.Add(new MenuItem(dr["MenuName"].ToString(), dr["MenuID"].ToString(), "", dr["MenuLocation"].ToString() + "?field1=" + companyName + "&field2=" + location));
				}

				foreach (DataRow dr in dt.Select("ParentID > 0"))
				{
					MenuItem mnu = new MenuItem(dr["MenuName"].ToString(), dr["MenuID"].ToString(), "", dr["MenuLocation"].ToString() + "?field1=" + companyName + "&field2=" + location);
					menuBar.FindItem(dr["ParentID"].ToString()).ChildItems.Add(mnu);
				}

			}

			dbConn.Close();

		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
		}
	}
}