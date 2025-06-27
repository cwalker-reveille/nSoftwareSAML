using System;
using System.Web;
using System.Web.Configuration;
using nsoftware.CloudSSO;

namespace nSoftwareSAML
{
    public partial class SSOStart : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            string ssoACS = WebConfigurationManager.AppSettings[ "sso-acs" ];
            string ssoIssuer = WebConfigurationManager.AppSettings[ "sso-issuer" ];
            string endpoint = WebConfigurationManager.AppSettings[ "sso-endpoint" ];
            string samlRuntimeLicense = WebConfigurationManager.AppSettings[ "saml-runtime-license" ];
            
            if ( string.IsNullOrEmpty( endpoint ) )
            {
                Response.Write( "<h2>SSO endpoint is not configured.</h2>" );
                Response.End();
                return;
            }
            try
            {
                using ( var saml = new SAMLWeb() )
                {
                    saml.RuntimeLicense = samlRuntimeLicense;
                    saml.RequestIdentityMetadata( endpoint );
                    URI acs = new URI();
                    acs.URIType = SAMLURITypes.sutACS;
                    acs.Location = ssoACS;
                    acs.BindingType = URIBindings.subPost;
                    saml.ServiceProviderURIs.Add( acs );
                    saml.SAMLRequestSettings.Issuer = ssoIssuer;
                    saml.SAMLRequestSettings.Binding = SAMLBindings.sbHTTPPost;
                    saml.BuildAuthnRequest();
                    string idpUrl = saml.SAMLRequestURL;
                    string samlRequest = saml.SAMLRequestBody;
                    string relayState = saml.RelayState;
                    Response.Clear();
                    Response.ContentType = "text/html";
                    Response.Write( $@"
                        <html><head><title>Redirecting...</title></head><body>
                        <form id='samlForm' action='{HttpUtility.HtmlAttributeEncode( idpUrl )}' method='post'>
                            <input type='hidden' name='SAMLRequest' value='{HttpUtility.HtmlAttributeEncode( samlRequest )}' />
                            {( string.IsNullOrEmpty( relayState ) ? "" : $"<input type='hidden' name='RelayState' value='{HttpUtility.HtmlAttributeEncode( relayState )}' />" )}
                        </form>
                        <script>document.getElementById('samlForm').submit();</script>
                        </body></html>" );
                    Response.End();
                }
            }
            catch ( Exception ex )
            {
                Response.Write( $"<h2>Exception during SSO authentication</h2><pre>{HttpUtility.HtmlEncode( ex.Message )}</pre>" );
                Response.End();
            }
        }
    }
}
