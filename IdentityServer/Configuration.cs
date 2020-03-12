﻿using IdentityModel;
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
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo")
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
                         IdentityServerConstants.StandardScopes.Profile
                     },
                     RedirectUris = { "http://localhost:6003/signin-oidc" },
                     RequireConsent = false
                }
            };
    }
}
