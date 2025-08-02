using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Adasit.Andor.Auth;

public static class JwtAuthenticationMiddleware
{
    private static RsaSecurityKey BuildRSAKey(IdentityProvider authOptions)
    {
        RSA rsa = RSA.Create();

        rsa.ImportSubjectPublicKeyInfo(
            source: Convert.FromBase64String(authOptions?.PublicKeyJwt!),
            bytesRead: out _
        );

        var IssuerSigningKey = new RsaSecurityKey(rsa);
        return IssuerSigningKey;
    }

    public static IServiceCollection ConfigureJWT(this IServiceCollection services,
        IConfiguration configuration)
    {
        var authOptions = configuration
            .GetSection(nameof(IdentityProvider))
            .Get<IdentityProvider>();

        if (authOptions?.PublicKeyJwt is null)
            return services;

        services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

        services.AddAuthorization(o =>
        {
            o.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            o.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            #region == JWT Token Validation ===
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidIssuers = new[] { authOptions?.Authority! },
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = BuildRSAKey(authOptions),
                ValidateLifetime = true
            };
            #endregion

            o.Events = new JwtBearerEvents()
            {
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/plain";

                    return context.Response.WriteAsync("An error occurred processing your authentication.");
                }
            };
        });

        return services;
    }
}
