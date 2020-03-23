using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                   Name = "rc.scope",
                   UserClaims = { "rc.garndma" }
                }
            };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo", new string[] {"rc.api.garndma" })
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                     ClientId = "client_id",
                     ClientSecrets = { new Secret("client_secret".ToSha256())},

                     AllowedGrantTypes = GrantTypes.ClientCredentials,
                     AllowedScopes = { "ApiOne" }
                },
                new Client
                {
                     ClientId = "client_id_mvc",
                     ClientSecrets = { new Secret("client_secret_mvc".ToSha256())},
                     AllowedGrantTypes = GrantTypes.Code,
                     AllowedScopes =
                     {
                         "ApiOne",
                         "ApiTwo",
                         IdentityServerConstants.StandardScopes.OpenId,
                         // IdentityServerConstants.StandardScopes.Profile,
                         "rc.scope"
                     },
                     AllowOfflineAccess =true,
                     // puts all the claims in the id token
                     // AlwaysIncludeUserClaimsInIdToken = true,
                     RedirectUris = { "http://localhost:6003/signin-oidc" },
                     PostLogoutRedirectUris = { "http://localhost:6003/Home/Index" },
                     RequireConsent = false
                },
                new Client
                {
                    ClientId = "client_id_js",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes =
                     {
                         "ApiOne",
                         "ApiTwo",
                         IdentityServerConstants.StandardScopes.OpenId,
                         "rc.scope"
                     },
                    AccessTokenLifetime = 1,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { "http://localhost:6004/home/signin" },
                    PostLogoutRedirectUris = { "http://localhost:6004/home/index" },
                    RequireConsent = false,
                    AllowedCorsOrigins = { "http://localhost:6004" }
                }
            };
    }
}
