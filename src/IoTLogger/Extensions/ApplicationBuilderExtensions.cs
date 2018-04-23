using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
	public static class ApplicationBuilderExtensions
	{
		public static void UseSwagger( this IApplicationBuilder app, IConfiguration configuration, IHostingEnvironment env )
		{
			string version = configuration.GetSection( "SwaggerDoc" ).GetValue<string>( "Version" );

			app.UseSwagger( c => { c.PreSerializeFilters.Add( ( document, request ) => { document.BasePath = configuration.GetValue<string>( "SwaggerBasePath" ); } ); } );
			app.UseSwaggerUI( c => { c.SwaggerEndpoint( $"{version}/swagger.json", env.ApplicationName ); } );
		}
	}
}
