﻿using System;
using System.Web;
using System.Web.Configuration;
using nsoftware.CloudSSO;

namespace nSoftwareSAML
{
    public partial class SamlAcsHandler : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {
            try
            {
                var pos = Request.InputStream.Position; // Should be 0 if not read
                if ( pos > 0 )
                {
                    Request.InputStream.Seek( 0, System.IO.SeekOrigin.Begin ); // Reset stream position if possible
                }
            }
            catch ( Exception ex )
            {
                System.Diagnostics.Debug.WriteLine( "InputStream already read: " + ex.Message );
            }

            // Use the form's Controls collection to add HTML output
            var htmlContainer = this.form1;

            if ( Request.HttpMethod == System.Net.WebRequestMethods.Http.Post )
            {
                try
                {
                    using ( var saml = new SAMLWeb() )
                    {
                        string samlRuntimeLicense = WebConfigurationManager.AppSettings[ "saml-runtime-license" ];
                        saml.RuntimeLicense = samlRuntimeLicense;

                        // Process/validate the SAML response
                        saml.ProcessSAMLResponse();

                        if ( !String.IsNullOrEmpty( saml.RelayState ) )
                            Response.Redirect( saml.RelayState );
                        else
                            Response.Redirect( "~/Default.aspx" );
                    }
                }
                catch ( Exception ex )
                {
                    htmlContainer.Controls.Add(new System.Web.UI.LiteralControl($"<h2>SAML Authentication Failed</h2><pre>{HttpUtility.HtmlEncode( ex.Message )}</pre>"));
                }
            }
            else
            {
                htmlContainer.Controls.Add(new System.Web.UI.LiteralControl("<h2>No SAMLResponse found.</h2>"));
            }
        }
    }
}