using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System;

namespace Api.Covid19
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureAppConfiguration((context, config) =>
             {
                 var keyVaultEndpoint = Environment.GetEnvironmentVariable("VaultUri");
                 if (!string.IsNullOrEmpty(keyVaultEndpoint))
                 {
                     var azureServiceTokenProvider = new AzureServiceTokenProvider();
                     var keyVaultClient = new KeyVaultClient(
                         new KeyVaultClient.AuthenticationCallback(
                             azureServiceTokenProvider.KeyVaultTokenCallback));
                     config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                 }
             })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             });
    }
}