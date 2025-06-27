using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Routing;
using System.Web.UI.WebControls;

namespace nSoftwareSAML
{
    public class Global : HttpApplication
    {
        void Application_Start( object sender, EventArgs e )
        {
            // SAML/SSO route
            RouteTable.Routes.MapPageRoute( "SAML_ACS", "sso/acs", "~/SamlAcsHandler.aspx" );

        }
        protected void Application_AuthenticateRequest( object sender, EventArgs e )
        {
            string path = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath?.ToLowerInvariant();

            // Allow SSOStart and SAML ACS endpoint to bypass SSO redirect
            if ( path != "~/ssostart.aspx" && path != "~/sso/acs" )
            {
                Response.Redirect( "~/SSOStart.aspx" );
                return;
            }
            // If already on SSOStart.aspx or SAML ACS, do nothing (let the page handle SSO)

        }
    }
}