using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Swagger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static void AddSwagger( this IServiceCollection services, IConfiguration configuration, IHostingEnvironment env )
		{
			string filename = Path.Combine( AppContext.BaseDirectory, $"{env.ApplicationName}.xml" );
			var info = configuration.GetSection( "SwaggerDoc" ).Get<Info>();

			services.AddSwaggerGen( options =>
			{
				options.SwaggerDoc( info.Version, info );
				options.DescribeAllEnumsAsStrings();

				if( File.Exists( filename ) )
					options.IncludeXmlComments( filename );
			} );
		}
	}
}
