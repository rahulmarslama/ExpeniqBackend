using Duende.IdentityModel;
using IdentityServer.IdentityExtensions.ClaimsConfiguration;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configurations
{
    public static class InMemoryConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "additional_user_info",
                    DisplayName = "Additional User Information",
                    Description = "Additional User Information",
                    Emphasize = true,
                    Required = true,
                    ShowInDiscoveryDocument = true,
                    UserClaims = new List<string>
                    {
                        UserInfoClaims.LangOption,
                        UserInfoClaims.ThemeOption,
                        UserInfoClaims.UserName,
                        UserInfoClaims.Name,
                    }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
           new List<ApiScope>
           {
               new ApiScope()
               {
                   Name="expendiqApiScope",
                   UserClaims= new List<string>
                    {
                        UserInfoClaims.LangOption,
                        UserInfoClaims.ThemeOption,
                        UserInfoClaims.UserName,
                        UserInfoClaims.Name
                    }
               }
           };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource()
                {
                    Name="ExpendiqApi",
                    DisplayName="Expendiq API",
                    Scopes= new List<string>
                    {
                        "expendiqApiScope"
                    },
                }
            };


        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = { "expendiqApiScope"}
                },
                new Client {
                    ClientId = "postman-api",
                    ClientSecrets = { new Secret("postman_secret123".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    RedirectUris =
                    {
                        "https://oauth.pstmn.io/v1/callback"
                    },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "expendiqApiScope",
                        "additional_user_info",
                    },

                    // puts all the claims in the id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser =true,
                    AllowOfflineAccess = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "expendiq_client",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:3600/api/auth/callback/okta",
                        "http://localhost:2500/api/auth/callback/okta",
                    },
                    PostLogoutRedirectUris =
                    {
                       "http://localhost:3600/signout",
                       "http://localhost:2500/signout",
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:3600",
                        "http://localhost:2500",
                    },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "additional_user_info",
                        "expendiqApiScope",
                    },

                    AllowOfflineAccess=true,

                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }

            };
    }
}
