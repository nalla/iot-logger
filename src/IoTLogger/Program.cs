using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace IoTLogger
{
	internal static class Program
	{
		private static IWebHost BuildWebHost( string[] args )
		{
			IConfigurationRoot hostConfig = new ConfigurationBuilder()
				.SetBasePath( Directory.GetCurrentDirectory() )
				.AddJsonFile( "hosting.json", true )
				.Build();

			return WebHost.CreateDefaultBuilder( args )
				.UseConfiguration( hostConfig )
				.UseStartup<Startup>()
				.UseSerilog( ( hostingContext, loggerConfiguration ) => loggerConfiguration.ReadFrom.Configuration( hostingContext.Configuration ) )
				.Build();
		}

		private static void Main( string[] args ) => BuildWebHost( args ).Run();
	}
}
