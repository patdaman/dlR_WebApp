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

namespace RapidInventoryWebApp
{
	public partial class SiteMaster : MasterPage
	{
		private const string AntiXsrfTokenKey = "__AntiXsrfToken";
		private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
		private string _antiXsrfTokenValue;

		protected void Page_Init(object sender, System.EventArgs e)
		{
			// The code below helps to protect against XSRF attacks
			HttpCookie requestCookie = Request.Cookies[AntiXsrfTokenKey];
			Guid requestCookieGuidValue = new Guid();
			if ((requestCookie != null) && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
			{
				// Use the Anti-XSRF token from the cookie
				_antiXsrfTokenValue = requestCookie.Value;
				Page.ViewStateUserKey = _antiXsrfTokenValue;
			}
			else
			{
				// Generate a new Anti-XSRF token and save to the cookie
				_antiXsrfTokenValue = Guid.NewGuid().ToString("N");
				Page.ViewStateUserKey = _antiXsrfTokenValue;

				HttpCookie responseCookie = new HttpCookie(AntiXsrfTokenKey) {HttpOnly = true, Value = _antiXsrfTokenValue};
				if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
				{
					responseCookie.Secure = true;
				}
				Response.Cookies.Set(responseCookie);
			}

			Page.PreLoad += master_Page_PreLoad;
//INSTANT C# NOTE: Converted event handler wireups:
			this.Load += Page_Load;
		}

		private void master_Page_PreLoad(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				// Set Anti-XSRF token
				ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
				ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? string.Empty;
			}
			else
			{
				// Validate the Anti-XSRF token
				if (!(((string)ViewState[AntiXsrfTokenKey]) == _antiXsrfTokenValue) || !(((string)ViewState[AntiXsrfUserNameKey]) == (Context.User.Identity.Name ?? string.Empty)))
				{
					throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{

		}
	}
}