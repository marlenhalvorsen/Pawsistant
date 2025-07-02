using DotNetEnv;

namespace PawsistantAPI.Helpers
{
    public static class AppSecrets
    {
        public static void LoadSecrets(WebApplicationBuilder builder)
        { 
            //reads from the .env file
            Env.Load();
        
            //get values from .env
            var apiKey = Environment.GetEnvironmentVariable("OpenRouterApiKey");
            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            var jwtSecretKey = Environment.GetEnvironmentVariable("Jwt__SecretKey");
            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");

            //set values from .env into Configuration
            builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
            builder.Configuration["OpenRouter:ApiKey"] = apiKey;
            builder.Configuration["Jwt:SecretKey"] = jwtSecretKey;
            builder.Configuration["Jwt:Issuer"] = jwtIssuer;
            builder.Configuration["Jwt:Audience"] = jwtAudience;
        }

    }
}
