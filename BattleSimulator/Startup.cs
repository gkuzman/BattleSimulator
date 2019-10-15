using BattleSimulator.DAL.Contexts;
using BattleSimulator.Entities.Options;
using BattleSimulator.Services.Interfaces;
using BattleSimulator.Services.Pipelines;
using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using BattleSimulator.Services.Services;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Reflection;

namespace BattleSimulator
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContextPool<TrackingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TrackingContext"));
            });

            services.AddDbContextPool<NonTrackingContext>(options =>
            {
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).UseSqlServer(Configuration.GetConnectionString("TrackingContext"));
            });

            services.AddMediatR((config) =>
            {
                config.AsTransient();
            },
                Assembly.GetAssembly(typeof(AddArmyService))
            );

            services.AddTransient(typeof(IPipelineBehavior<AddArmyRequest, AddArmyResponse>), typeof(AddArmyPipeline));
            services.Decorate(typeof(IRequestHandler<,>), typeof(ProcessingPipeline<,>));

            services.Scan(scan =>
            {
                scan.FromAssembliesOf(typeof(ITransientService))
                .AddClasses(x => x.AssignableTo(typeof(ITransientService)))
                .AsImplementedInterfaces().WithTransientLifetime();
            });

            services.Configure<ArmyOptions>(options => Configuration.GetSection("ArmyOptions").Bind(options));
            services.Configure<BattleOptions>(options => Configuration.GetSection("BattleOptions").Bind(options));

            var hangfireSqlOptions = new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.Zero,
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            };

            services.AddHangfire(hf =>
            {
                hf.UseSerilogLogProvider();
                hf.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), hangfireSqlOptions);
            });

            services.AddTransient<IBackgroundJobClient>(src => 
                new BackgroundJobClient(new SqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), hangfireSqlOptions)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            loggerFactory.AddSerilog();
            app.UseHttpsRedirection();

            GlobalConfiguration.Configuration.UseActivator(new ContainerJobActivator(serviceProvider));
            app.UseHangfireServer();
            app.UseMvc();
        }
    }
}
