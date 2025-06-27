# Test nSoftware CloudSSO for asp.net 4.8

This project demonstrates how to integrate and test the nSoftware CloudSSO library in an ASP.NET 4.8 web application. It provides a sample implementation for initiating SAML-based Single Sign-On (SSO) using the nSoftware CloudSSO components.

## Features
- SAML SSO authentication flow
- Configuration via `Web.config`
- Example usage of `SAMLWeb` and related classes

## Getting Started
1. Clone the repository.
2. Configure your SSO settings using XML transforms:
   - The final `Web.config` is generated from `Web.base.config` and environment-specific files like `Web.Debug.config`, `Web.Release.config`, etc., using XML transformations during build or publish.
   - If the `Web.{configuration}.config` file for your build configuration is not present, then `Web.base.config` will be copied directly to `Web.config`.
   - Note: `Web.config` is overwritten on every build (Release or Debug), so make changes in the transform or base config files, not directly in `Web.config`.
   - Update your SSO settings (e.g., `sso-acs`, `sso-issuer`, `sso-endpoint`, `saml-runtime-license`) in the appropriate config transform file for your environment.
3. Build and run the project in Visual Studio targeting .NET Framework 4.8.

## License
This project is for demonstration and testing purposes only. Refer to nSoftware licensing for CloudSSO usage.

