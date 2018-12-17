using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SyCoin.DataProvider.Mongo;
using SyCoin.DataProvider;
using SyCoin.Models;
using SyCoin.Core;

namespace SyCoin.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<AppSettingModel>(Configuration.GetSection("ApplicationSettings"));
            services.AddSingleton<IBlockDataProvider>(sp => new MongoBlockDataProvider(sp.GetService<IOptions<AppSettingModel>>()));
            services.AddSingleton<IUTXODataProvider>(sp => new MongoUTXODataProvider(sp.GetService<IOptions<AppSettingModel>>()));
            services.AddSingleton<UTXOManager>();
            services.AddSingleton<SyCoinProtocol>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
