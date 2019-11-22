namespace ChemRegulator.WebApi.Test
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class ControllersFixture : WebApplicationFactory<Program>
    {
        private const string AppDataPath = @"..\..\..\..\ChemRegulator.WebApi\App_data";
        private readonly string _tempAppDataPath;
        private readonly string _tempContentRootPath;

        public ControllersFixture()
        {
            _tempContentRootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            _tempAppDataPath = Path.Combine(_tempContentRootPath, "App_Data");
            Directory.CreateDirectory(_tempAppDataPath);
            _CopyToTempAppDataPath("connections.json");
            _CopyToTempAppDataPath("SwaggerInfo.md");
            Client = CreateClient();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _GetJWT());
        }

        public HttpClient Client { get; }

        private static string _GetJWT()
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, "john.doe"),
                new Claim(ClaimTypes.Name, "john.doe"),
                new Claim(ClaimTypes.Role, "Administrator"),
                new Claim(ClaimTypes.Role, "Editor")
            };

            var config = new ConfigurationBuilder().AddJsonFile("appsettings-test.json").Build();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:SecurityKey"]));
            var token = new JwtSecurityToken(
                config["Tokens:Issuer"],
                config["Tokens:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("AppConfiguration:ContentRootPath", _tempContentRootPath);
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings-test.json");
            builder.ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile(configPath);
            });

            base.ConfigureWebHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Directory.Exists(_tempContentRootPath))
            {
                Directory.Delete(_tempContentRootPath, true);
            }
        }

        private void _CopyToTempAppDataPath(string sourceFileName, string sourceDir = AppDataPath)
        {
            var destinationFilePath = Path.Combine(_tempAppDataPath, sourceFileName);
            File.Copy(Path.Combine(sourceDir, sourceFileName), destinationFilePath);
            new FileInfo(destinationFilePath).IsReadOnly = false;
        }
    }
}