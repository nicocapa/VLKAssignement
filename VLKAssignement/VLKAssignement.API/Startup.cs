using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using VLKAssignement.DataAccess;
using VLKAssignement.DataAccess.Repositories;
using VLKAssignement.DataAccess.Repositories.Interfaces;
using Microsoft.OpenApi.Models;
using VLKAssignement.Service.Interfaces;
using VLKAssignement.Service;
using System.Net.Mime;
using VLKAssignement.DataAccess.Models;

namespace VLKAssignement.API
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
            services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer("name=ConnectionStrings:DefaultConnection")
                        .UseLazyLoadingProxies());
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ITransferCartRepository, TransferCartRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICachedExchangeRateRepository, CachedExchangeRateRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<ITransferCartService, TransferCartService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<IUserService, UserService>();
            
            services.AddTransient<IWrapperExchangeRateAPI, WrapperExchangeRateAPI>();
            services.AddTransient<IExchangeRateService, ExchangeRateService>();
            services.AddTransient<IValidator<Transaction>, TransactionValidator>();
            services.AddTransient<IValidator<Transfer>, TransferValidator>();
            
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new BadRequestObjectResult(context.ModelState);                        
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);                        
                        return result;
                    };
                });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VLK Assignement", Version = "v1" });
                c.EnableAnnotations();
            });
            services.AddHttpClient("exchangerate", c =>
            {
                c.BaseAddress = new Uri("https://api.exchangeratesapi.io/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler("/error");
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.EnsureCreated();
            }
            app.UseSwagger();
        }
    }
}
