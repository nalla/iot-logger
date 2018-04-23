using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace IoTLogger
{
	public class Startup
	{
		private IConfiguration Configuration { get; }

		private IHostingEnvironment Env { get; }

		public Startup( IConfiguration configuration, IHostingEnvironment env )
		{
			Configuration = configuration;
			Env = env;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app )
		{
			app.UseCors( nameof(CorsPolicy) );
			app.UseSwagger( Configuration, Env );
			app.UseMetricServer();
			app.UseStatusCodePages();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services )
		{
			services.AddCors( cors => cors.AddPolicy( nameof(CorsPolicy), Configuration.GetSection( nameof(CorsPolicy) ).Get<CorsPolicy>() ) );
			services.AddSwagger( Configuration, Env );
			services.AddMvc();
		}
	}
}
