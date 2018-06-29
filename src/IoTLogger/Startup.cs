using IoTLogger.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoTLogger
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			app.UseCors(nameof(CorsPolicy));
			app.UseStatusCodePages();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IDataProcessor, InfluxdbProcessor>();
			services.AddCors(cors => cors.AddPolicy(nameof(CorsPolicy), _configuration.GetSection(nameof(CorsPolicy)).Get<CorsPolicy>()));
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}
	}
}
