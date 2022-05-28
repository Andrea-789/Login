using System;
using GestUser.Helpers;
using GestUser.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

namespace GestUser
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      this.Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

      var connectionString = Configuration["connectionStrings:DbConString"];
      services.AddDbContext<LoginDbContext>(c => c.UseSqlServer(connectionString));

      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);

      var appEmailSection = Configuration.GetSection("EmailSettings");
      services.Configure<EmailSettings>(appEmailSection);

      var appInterfaceSection = Configuration.GetSection("InterfaceSettings");
      services.Configure<InterfaceSettings>(appInterfaceSection);      

      services.AddScoped<IUserService, UserService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

#if DEBUG
      app.UseCors(options =>
          options
              .WithOrigins("http://localhost:4200")
              .WithMethods("POST", "PUT", "DELETE", "GET")
              .AllowAnyHeader()
      );
#else
            app.UseCors(options =>
                options
                    .WithOrigins("http://localhost:6001")
                    .WithMethods("POST", "PUT", "DELETE", "GET")
                    .AllowAnyHeader()
            );
#endif
      app.UseEndpoints(endpoints =>
            {
              endpoints.MapControllers();
            });
    }
  }
}
